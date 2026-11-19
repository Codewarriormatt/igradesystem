# iGradesystem: Digital Grading E-Record

A Windows desktop app for managing students, subjects and grades.
An **Admin** sets up users, students, subjects and class assignments.
A **Teacher** enters grades for their own subjects and prints reports.

Class project for a Database Systems course (June 2025), shared here as a portfolio piece.

## What the app does

**Admin**
- Manage users (Admin or Teacher), students and subjects, each with live search.
- Assign subjects to teachers, and students to subjects.
- View the activity log (who added, changed or deleted what), with search and a date range.
- Print 5 reports: Users, Students, Subjects, Assignments, Activity Log.

**Teacher**
- My Subjects: see the subjects assigned to you, filter by units.
- Manage Grades: enter grades from 1 to 100. The database marks each grade Passed (75 and up) or Failed.
- Print 3 reports: Grades per Subject, Pass/Fail Summary, Students per Subject.

**Database design (MySQL / MariaDB)**
- 7 tables with foreign keys.
- 9 stored procedures for add, update and delete.
- 10 triggers that write every change to the activity log.
- 3 views for active (not deleted) records.
- Soft delete: rows are marked with `DeletedBy` instead of being removed.
- A generated column (`Remarks` = Passed/Failed) and a `CHECK` constraint on grades.
- Every query uses parameters or stored procedures, so user input is never pasted into SQL.

## Tech stack

- C# Windows Forms, .NET Framework 4.7.2
- MySQL 8.0.16+ or MariaDB 10.4+ (XAMPP), using MySql.Data 9.3 (NuGet)
- RDLC reports with Microsoft ReportViewer for WinForms 15 (NuGet)
- Visual Studio 2022

## How to run it on a fresh Windows PC

1. **Install Visual Studio 2022** (Community is fine) with the **".NET desktop development"** workload.
   In the Visual Studio Installer, also tick **".NET Framework 4.7.2 targeting pack"** (Individual components).
2. **Install XAMPP** (or MySQL 8.0.16+). Open the XAMPP Control Panel and start **MySQL**.
3. **Create the database.** Open phpMyAdmin (http://localhost/phpmyadmin), click **SQL**, and run:
   ```sql
   CREATE DATABASE gradingsystemdb CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
   ```
4. **Import the demo data.** In phpMyAdmin, select `gradingsystemdb`, click **Import**, and choose
   `database/gradingsystem_demo.sql`. Or use the command line:
   ```
   mysql -u root -p < database/gradingsystem_demo.sql
   ```
   (XAMPP's `mysql.exe` is in `C:\xampp\mysql\bin\`. Press Enter at the password prompt if root has no password.)
   The script resets the demo tables each time you run it.
5. **Open `GradingSystem1.sln`** in Visual Studio. NuGet packages restore on the first build.
   If they do not, right-click the solution and choose **Restore NuGet Packages**.
6. **Set the connection string once**, in `GradingSystem1/App.config`. It is the only place the app reads it from:
   ```xml
   <add name="GradingSystemDB"
        connectionString="server=localhost;database=gradingsystemdb;user=root;password=;" ... />
   ```
   The demo value fits a default XAMPP install (user `root`, empty password). Change it to match your own MySQL server.
7. **Press F5** and log in with a demo account below.

## Demo logins

| Role    | Username  | Password        |
|---------|-----------|-----------------|
| Admin   | `admin`   | `Admin@Demo1`   |
| Teacher | `teacher` | `Teacher@Demo1` |

These are demo accounts for a local test database only. All students, subjects and grades in the demo data are made up.

## Screenshots

Coming soon. They will be added to [`docs/screenshots/`](docs/screenshots/).

## Post-submission fixes

The code here is the version the team submitted, plus these fixes made before publishing:

- **Password hashing.** Passwords are stored as PBKDF2 hashes (`Rfc2898DeriveBytes`, SHA-256, 16-byte random salt, 100,000 iterations), not plain text.
  The login checks the hash in code. New and edited users get a fresh hash. See `GradingSystem1/PasswordHasher.cs`.
  Stored format: `PBKDF2-SHA256$<iterations>$<salt Base64>$<hash Base64>` (about 90 characters, fits the `varchar(100)` column).
- **Passwords are never shown.** The Password column was removed from the User Manager grid, from its search, and from the `viewactiveusers` view.
- **Deleted users cannot log in.** The login query now skips soft-deleted users (`DeletedBy IS NULL`).
- **Edit without retyping the password.** When you update a user, leave the password box blank to keep the current password.
- **One connection string.** It now lives in `App.config` (`<connectionStrings>`) and is read with `ConfigurationManager`. Before, it was copied into 11 files.
- **Reports load from the app folder.** RDLC report paths now use `Application.StartupPath`, so reports work whatever the working folder is.
- **Clean report files.** Removed machine-specific `rd:SchemaPath` lines from the 8 `.rdlc` files.
- **Clean packages.** Removed an unused old ReportViewer 10 entry from `packages.config`.
- **Grade search fix.** Typing an apostrophe (`'`) in the Manage Grades search box no longer causes an error.
- **New demo database script** (`database/gradingsystem_demo.sql`): same schema, `CREATE DATABASE` + `USE` added, `DEFINER` clauses removed, fake data only, empty activity log.

## Known limitations (class project)

- Error messages show the raw exception text (`ex.Message`) to the user.
- There is no lockout or delay after many failed logins.
- The login trims spaces around the typed password, but new passwords are saved as typed. A password that starts or ends with a space will not work.
- The password box in User Manager does not hide the characters you type.
- The demo setup uses the MySQL `root` account with an empty password (XAMPP default). Use a separate account with a password for anything beyond a local demo.
- Roles (admin, teacher) are checked only in the app's screens. Every copy of the app uses the same database account from `App.config`, so anyone with that account can change data directly in MySQL.
- Windows only. The code writes table names in mixed case (`Users`, `ActivityLog`), which works with the Windows default of case-insensitive table names.
- **What-If Calculator: planned, not finished.** `WhatIfCalculatorForm` is an empty form. The Teacher dashboard does not open it yet (that code is commented out).

## Team and credits

Built by a 3-member student team.

My part (Codewarriormatt): requirements, workflow and logic design, and testing. I also co-wrote parts of the C# code with AI assistance.

Images: the round iGRADE SYSTEM badge was made by the team. Some icons and background images in the forms come from free online sources; their rights belong to their owners. If you own one and want it removed, open an issue on this repo and I will replace it.

## License

All rights reserved. Shared for portfolio viewing only.

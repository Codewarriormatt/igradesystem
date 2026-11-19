-- --------------------------------------------------------
-- iGradesystem demo database
--
-- Schema: the class project's MariaDB schema (7 tables, 9 stored procedures,
--         10 audit triggers, 3 views). DEFINER clauses removed so it imports
--         under any MySQL/MariaDB account.
-- Data:   FAKE demo data only (made-up students, sample subjects and grades).
--         The activity log starts empty.
--
-- Demo logins (passwords are stored as PBKDF2-SHA256 hashes, never plain text):
--   admin   / Admin@Demo1     (Admin)
--   teacher / Teacher@Demo1   (Teacher)
--
-- Re-running this file RESETS the demo database (it drops and recreates the
-- tables, views and procedures below).
-- Tested target: MariaDB 10.4+ (XAMPP). MySQL 8.0.16+ should also work.
-- --------------------------------------------------------

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

CREATE DATABASE IF NOT EXISTS `gradingsystemdb` CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;
USE `gradingsystemdb`;

-- Clean slate (safe to re-run)
DROP VIEW IF EXISTS `viewactivestudents`;
DROP VIEW IF EXISTS `viewactivesubjects`;
DROP VIEW IF EXISTS `viewactiveusers`;
DROP TABLE IF EXISTS `grades`;
DROP TABLE IF EXISTS `studentsubjects`;
DROP TABLE IF EXISTS `teachersubjects`;
DROP TABLE IF EXISTS `students`;
DROP TABLE IF EXISTS `subjects`;
DROP TABLE IF EXISTS `users`;
DROP TABLE IF EXISTS `activitylog`;
DROP PROCEDURE IF EXISTS `DeleteStudentStudentForm`;
DROP PROCEDURE IF EXISTS `DeleteSubjectSubjectForm`;
DROP PROCEDURE IF EXISTS `DeleteUserUserForm`;
DROP PROCEDURE IF EXISTS `InsertStudentStudentForm`;
DROP PROCEDURE IF EXISTS `InsertSubjectSubjectForm`;
DROP PROCEDURE IF EXISTS `InsertUserUserForm`;
DROP PROCEDURE IF EXISTS `UpdateStudentStudentForm`;
DROP PROCEDURE IF EXISTS `UpdateSubjectSubjectForm`;
DROP PROCEDURE IF EXISTS `UpdateUserUserForm`;

START TRANSACTION;

DELIMITER $$
--
-- Procedures
--
CREATE PROCEDURE `DeleteStudentStudentForm` (IN `p_studentid` INT, IN `p_deletedby` VARCHAR(50))   BEGIN
    UPDATE Students
    SET DeletedBy = p_deletedby
    WHERE StudentID = p_studentid;
END$$

CREATE PROCEDURE `DeleteSubjectSubjectForm` (IN `p_subjectid` INT, IN `p_deletedby` VARCHAR(50))   BEGIN
    UPDATE Subjects
    SET DeletedBy = p_deletedby
    WHERE SubjectID = p_subjectid;
END$$

CREATE PROCEDURE `DeleteUserUserForm` (IN `p_userid` INT, IN `p_performedby` VARCHAR(50))   BEGIN
    UPDATE Users
    SET DeletedBy = p_performedby
    WHERE UserID = p_userid;
END$$

CREATE PROCEDURE `InsertStudentStudentForm` (IN `p_firstname` VARCHAR(50), IN `p_lastname` VARCHAR(50), IN `p_course` VARCHAR(50), IN `p_yearlevel` INT, IN `p_createdby` VARCHAR(50))   BEGIN
    -- Insert new student record
    INSERT INTO Students (FirstName, LastName, Course, YearLevel, CreatedBy)
    VALUES (p_firstname, p_lastname, p_course, p_yearlevel, p_createdby);
END$$

CREATE PROCEDURE `InsertSubjectSubjectForm` (IN `p_subjectcode` VARCHAR(20), IN `p_subjectname` VARCHAR(100), IN `p_units` INT, IN `p_createdby` VARCHAR(50))   BEGIN
    INSERT INTO Subjects (SubjectCode, SubjectName, Units, CreatedBy)
    VALUES (p_subjectcode, p_subjectname, p_units, p_createdby);
END$$

CREATE PROCEDURE `InsertUserUserForm` (IN `p_username` VARCHAR(50), IN `p_password` VARCHAR(100), IN `p_role` VARCHAR(20), IN `p_performedby` VARCHAR(50))   BEGIN
    -- for inserting the user (p_password is a PBKDF2 hash made by the app)
    INSERT INTO Users (Username, Password, Role, CreatedBy)
    VALUES (p_username, p_password, p_role, p_performedby);
END$$

CREATE PROCEDURE `UpdateStudentStudentForm` (IN `p_firstname` VARCHAR(50), IN `p_lastname` VARCHAR(50), IN `p_course` VARCHAR(50), IN `p_yearlevel` INT, IN `p_studentid` INT, IN `p_updatedby` VARCHAR(50))   BEGIN
    UPDATE Students
    SET
        FirstName = p_firstname,
        LastName = p_lastname,
        Course = p_course,
        YearLevel = p_yearlevel,
        UpdatedBy = p_updatedby
    WHERE StudentID = p_studentid;
END$$

CREATE PROCEDURE `UpdateSubjectSubjectForm` (IN `p_subjectcode` VARCHAR(20), IN `p_subjectname` VARCHAR(100), IN `p_units` INT, IN `p_subjectid` INT, IN `p_updatedby` VARCHAR(50))   BEGIN
    UPDATE Subjects
    SET
        SubjectCode = p_subjectcode,
        SubjectName = p_subjectname,
        Units = p_units,
        UpdatedBy = p_updatedby
    WHERE SubjectID = p_subjectid;
END$$

CREATE PROCEDURE `UpdateUserUserForm` (IN `p_userid` INT, IN `p_username` VARCHAR(50), IN `p_password` VARCHAR(100), IN `p_role` VARCHAR(20), IN `p_performedby` VARCHAR(50))   BEGIN
    -- Update the user info
    -- Post-submission fix: a NULL or empty p_password keeps the current password hash
    UPDATE Users
    SET Username = p_username,
        Password = COALESCE(NULLIF(p_password, ''), Password),
        Role = p_role,
        UpdatedBy = p_performedby
    WHERE UserID = p_userid;
END$$

DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `activitylog` (starts empty; filled by the triggers)
--

CREATE TABLE `activitylog` (
  `LogID` int(11) NOT NULL,
  `ActionType` varchar(10) DEFAULT NULL,
  `TableName` varchar(50) DEFAULT NULL,
  `RecordID` int(11) DEFAULT NULL,
  `Timestamp` datetime DEFAULT current_timestamp(),
  `PerformedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `grades`
--

CREATE TABLE `grades` (
  `GradeID` int(11) NOT NULL,
  `StudentID` int(11) NOT NULL,
  `SubjectID` int(11) NOT NULL,
  `Grade` decimal(5,2) NOT NULL CHECK (`Grade` between 1 and 100),
  `Remarks` varchar(10) GENERATED ALWAYS AS (case when `Grade` >= 75 then 'Passed' else 'Failed' end) STORED,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo data for table `grades` (Remarks is computed by the database)
--

INSERT INTO `grades` (`GradeID`, `StudentID`, `SubjectID`, `Grade`, `CreatedBy`, `UpdatedBy`, `DeletedBy`) VALUES
(1, 1, 1, 92.00, 'teacher', NULL, NULL),
(2, 2, 1, 88.50, 'teacher', NULL, NULL),
(3, 3, 1, 74.00, 'teacher', NULL, NULL),
(4, 4, 1, 81.00, 'teacher', NULL, NULL),
(5, 5, 1, 67.50, 'teacher', NULL, NULL),
(6, 6, 1, 95.00, 'teacher', NULL, NULL),
(7, 3, 3, 85.00, 'teacher', NULL, NULL),
(8, 4, 3, 78.00, 'teacher', NULL, NULL),
(9, 5, 3, 90.00, 'teacher', NULL, NULL),
(10, 6, 3, 72.00, 'teacher', NULL, NULL),
(11, 7, 3, 83.50, 'teacher', NULL, NULL),
(12, 8, 3, 76.00, 'teacher', NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `students`
--

CREATE TABLE `students` (
  `StudentID` int(11) NOT NULL,
  `FirstName` varchar(50) DEFAULT NULL,
  `LastName` varchar(50) DEFAULT NULL,
  `Course` varchar(50) DEFAULT NULL,
  `YearLevel` int(11) DEFAULT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo data for table `students` (made-up names)
--

INSERT INTO `students` (`StudentID`, `FirstName`, `LastName`, `Course`, `YearLevel`, `CreatedBy`, `UpdatedBy`, `DeletedBy`) VALUES
(1, 'Ada', 'Placeholder', 'BSCS', 1, 'admin', NULL, NULL),
(2, 'Ben', 'Example', 'BSIT', 2, 'admin', NULL, NULL),
(3, 'Cara', 'Sample', 'BSCS', 3, 'admin', NULL, NULL),
(4, 'Dan', 'Demo', 'BSIS', 1, 'admin', NULL, NULL),
(5, 'Eva', 'Testcase', 'BSIT', 4, 'admin', NULL, NULL),
(6, 'Finn', 'Mockup', 'BSCS', 2, 'admin', NULL, NULL),
(7, 'Gia', 'Fixture', 'BSIS', 3, 'admin', NULL, NULL),
(8, 'Hugo', 'Stub', 'BSCS', 1, 'admin', NULL, NULL),
(9, 'Iris', 'Dummy', 'BSIT', 2, 'admin', NULL, NULL),
(10, 'Jules', 'Prototype', 'BSCS', 4, 'admin', NULL, NULL);

--
-- Triggers `students`
--
DELIMITER $$
CREATE TRIGGER `TriggerInsertStudentLog` AFTER INSERT ON `students` FOR EACH ROW BEGIN
    INSERT INTO ActivityLog (ActionType, TableName, RecordID, PerformedBy)
    VALUES ('INSERT', 'Students', NEW.StudentID, NEW.CreatedBy);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TriggerUpdateDeleteStudentLog` AFTER UPDATE ON `students` FOR EACH ROW BEGIN
    DECLARE actionType VARCHAR(10);
    DECLARE performedby VARCHAR(50);

    IF NEW.DeletedBy IS NOT NULL THEN
        SET actionType = 'DELETE';
        Set performedby = New.DeletedBy;
    ELSE
        SET actionType = 'UPDATE';
        SET performedby = New.UpdatedBy;
    END IF;

    INSERT INTO activitylog (
        ActionType,
        TableName,
        RecordId,
        PerformedBy
    )
    VALUES (
        actionType,
        'Students',
        NEW.StudentID,
        performedby
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `studentsubjects`
--

CREATE TABLE `studentsubjects` (
  `ID` int(11) NOT NULL,
  `StudentID` int(11) NOT NULL,
  `SubjectID` int(11) NOT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL,
  `Timestamp` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo data for table `studentsubjects`
--

INSERT INTO `studentsubjects` (`ID`, `StudentID`, `SubjectID`, `CreatedBy`, `DeletedBy`, `Timestamp`) VALUES
(1, 1, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(2, 2, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(3, 3, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(4, 4, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(5, 5, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(6, 6, 1, 'admin', NULL, '2025-06-02 09:00:00'),
(7, 3, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(8, 4, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(9, 5, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(10, 6, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(11, 7, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(12, 8, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(13, 9, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(14, 10, 3, 'admin', NULL, '2025-06-02 09:10:00'),
(15, 1, 6, 'admin', NULL, '2025-06-02 09:20:00'),
(16, 2, 6, 'admin', NULL, '2025-06-02 09:20:00'),
(17, 8, 6, 'admin', NULL, '2025-06-02 09:20:00'),
(18, 9, 6, 'admin', NULL, '2025-06-02 09:20:00'),
(19, 7, 2, 'admin', NULL, '2025-06-02 09:30:00'),
(20, 10, 2, 'admin', NULL, '2025-06-02 09:30:00');

--
-- Triggers `studentsubjects`
--
DELIMITER $$
CREATE TRIGGER `TriggerInsertStudentSubjectLog` AFTER INSERT ON `studentsubjects` FOR EACH ROW INSERT INTO ActivityLog (ActionType, TableName, RecordID, PerformedBy)
VALUES ('INSERT', 'StudentSubjects', NEW.ID, NEW.CreatedBy)
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TriggerUpdateDeleteStudentSubjectLog` AFTER UPDATE ON `studentsubjects` FOR EACH ROW BEGIN
    DECLARE actionType VARCHAR(10);
    DECLARE performedBy VARCHAR(50);

    IF NEW.DeletedBy IS NOT NULL THEN
        SET actionType = 'DELETE';
        SET performedBy = NEW.DeletedBy;
    ELSE
        SET actionType = 'UPDATE';
        SET performedBy = NEW.CreatedBy;
    END IF;

    INSERT INTO ActivityLog (
        ActionType,
        TableName,
        RecordID,
        PerformedBy
    ) VALUES (
        actionType,
        'StudentSubjects',
        NEW.ID,
        performedBy
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `subjects`
--

CREATE TABLE `subjects` (
  `SubjectID` int(11) NOT NULL,
  `SubjectCode` varchar(20) NOT NULL,
  `SubjectName` varchar(100) NOT NULL,
  `Units` int(11) NOT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo data for table `subjects`
--

INSERT INTO `subjects` (`SubjectID`, `SubjectCode`, `SubjectName`, `Units`, `CreatedBy`, `UpdatedBy`, `DeletedBy`) VALUES
(1, 'CS101', 'Introduction to Computing', 3, 'admin', NULL, NULL),
(2, 'CS102', 'Computer Programming 1', 3, 'admin', NULL, NULL),
(3, 'DB201', 'Database Systems', 3, 'admin', NULL, NULL),
(4, 'WD202', 'Web Development', 3, 'admin', NULL, NULL),
(5, 'MA103', 'Discrete Mathematics', 3, 'admin', NULL, NULL),
(6, 'PE101', 'Physical Education 1', 2, 'admin', NULL, NULL);

--
-- Triggers `subjects`
--
DELIMITER $$
CREATE TRIGGER `TriggerInsertSubjectLog` AFTER INSERT ON `subjects` FOR EACH ROW BEGIN
    INSERT INTO ActivityLog (ActionType, TableName, RecordID, PerformedBy)
    VALUES ('INSERT', 'Subjects', NEW.SubjectID, NEW.CreatedBy);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TriggerUpdateDeleteSubjectLog` AFTER UPDATE ON `subjects` FOR EACH ROW BEGIN
    DECLARE actionType VARCHAR(10);
    DECLARE performedby VARCHAR(50);

    IF NEW.DeletedBy IS NOT NULL THEN
        SET actionType = 'DELETE';
        SET performedby = NEW.DeletedBy;
    ELSE
        SET actionType = 'UPDATE';
        SET performedby = NEW.UpdatedBy;
    END IF;

    INSERT INTO ActivityLog (
        ActionType,
        TableName,
        RecordID,
        PerformedBy
    )
    VALUES (
        actionType,
        'Subjects',
        NEW.SubjectID,
        performedby
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `teachersubjects`
--

CREATE TABLE `teachersubjects` (
  `ID` int(11) NOT NULL,
  `TeacherID` int(11) NOT NULL,
  `SubjectID` int(11) NOT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo data for table `teachersubjects` (the demo teacher is UserID 2)
--

INSERT INTO `teachersubjects` (`ID`, `TeacherID`, `SubjectID`, `CreatedBy`, `DeletedBy`) VALUES
(1, 2, 1, 'admin', NULL),
(2, 2, 3, 'admin', NULL),
(3, 2, 6, 'admin', NULL);

--
-- Triggers `teachersubjects`
--
DELIMITER $$
CREATE TRIGGER `TriggerInsertTeacherSubjectLog` AFTER INSERT ON `teachersubjects` FOR EACH ROW BEGIN
    INSERT INTO ActivityLog (
        ActionType,
        TableName,
        RecordID,
        PerformedBy
    )
    VALUES (
        'INSERT',
        'TeacherSubjects',
        NEW.ID,
        NEW.CreatedBy
    );
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TriggerUpdateDeleteTeacherSubjectLog` AFTER UPDATE ON `teachersubjects` FOR EACH ROW BEGIN
    IF NEW.DeletedBy IS NOT NULL THEN
        INSERT INTO ActivityLog (
            ActionType,
            TableName,
            RecordID,
            PerformedBy
        )
        VALUES (
            'DELETE',
            'TeacherSubjects',
            NEW.ID,
            NEW.DeletedBy
        );
    END IF;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `users`
-- Password holds a PBKDF2 hash string: PBKDF2-SHA256$<iterations>$<salt Base64>$<hash Base64>
--

CREATE TABLE `users` (
  `UserID` int(11) NOT NULL,
  `Username` varchar(50) NOT NULL,
  `Password` varchar(100) NOT NULL,
  `Role` enum('Admin','Teacher') NOT NULL,
  `CreatedBy` varchar(50) DEFAULT NULL,
  `UpdatedBy` varchar(50) DEFAULT NULL,
  `DeletedBy` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Demo users: admin / Admin@Demo1 and teacher / Teacher@Demo1
--

INSERT INTO `users` (`UserID`, `Username`, `Password`, `Role`, `CreatedBy`, `UpdatedBy`, `DeletedBy`) VALUES
(1, 'admin', 'PBKDF2-SHA256$100000$d8eV7i9tjWZE5memFthShA==$Efvaaw0eI8qTWZuDTMmTfu4u/gVMn9UgJtR/dFRgQdk=', 'Admin', NULL, NULL, NULL),
(2, 'teacher', 'PBKDF2-SHA256$100000$UcUNbGAMC+JPYOuHUa42eA==$y+fvMzMO+MM0RUh90LeHp24fiB9wwY46AgMv33UltNg=', 'Teacher', 'admin', NULL, NULL);

--
-- Triggers `users`
--
DELIMITER $$
CREATE TRIGGER `TriggerInsertUserLog` AFTER INSERT ON `users` FOR EACH ROW BEGIN
    INSERT INTO activitylog (ActionType, TableName, RecordId, PerformedBy)
    VALUES ('INSERT', 'Users', New.UserID, New.Createdby);
END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TriggerUpdateDeleteUserLog` AFTER UPDATE ON `users` FOR EACH ROW BEGIN
    DECLARE actionType VARCHAR(10);
    DECLARE performedby VARCHAR(50);

    IF NEW.DeletedBy IS NOT NULL THEN
        SET actionType = 'DELETE';
        Set performedby = New.DeletedBy;
    ELSE
        SET actionType = 'UPDATE';
        SET performedby = New.UpdatedBy;
    END IF;

    INSERT INTO activitylog (
        ActionType,
        TableName,
        RecordId,
        PerformedBy
    )
    VALUES (
        actionType,
        'Users',
        NEW.UserID,
        performedby
    );
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Views (DEFINER removed)
--

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `viewactivestudents`  AS SELECT `students`.`StudentID` AS `StudentID`, `students`.`FirstName` AS `FirstName`, `students`.`LastName` AS `LastName`, `students`.`Course` AS `Course`, `students`.`YearLevel` AS `YearLevel` FROM `students` WHERE `students`.`DeletedBy` is null ;

CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `viewactivesubjects`  AS SELECT `subjects`.`SubjectID` AS `SubjectID`, `subjects`.`SubjectCode` AS `SubjectCode`, `subjects`.`SubjectName` AS `SubjectName`, `subjects`.`Units` AS `Units` FROM `subjects` WHERE `subjects`.`DeletedBy` is null ;

-- Post-submission fix: the Password column is no longer exposed by this view
CREATE ALGORITHM=UNDEFINED SQL SECURITY DEFINER VIEW `viewactiveusers`  AS SELECT `users`.`UserID` AS `UserID`, `users`.`Username` AS `Username`, `users`.`Role` AS `Role` FROM `users` WHERE `users`.`DeletedBy` is null ;

--
-- Indexes for tables
--

ALTER TABLE `activitylog`
  ADD PRIMARY KEY (`LogID`);

ALTER TABLE `grades`
  ADD PRIMARY KEY (`GradeID`),
  ADD KEY `StudentID` (`StudentID`),
  ADD KEY `SubjectID` (`SubjectID`);

ALTER TABLE `students`
  ADD PRIMARY KEY (`StudentID`);

ALTER TABLE `studentsubjects`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `StudentID` (`StudentID`),
  ADD KEY `SubjectID` (`SubjectID`);

ALTER TABLE `subjects`
  ADD PRIMARY KEY (`SubjectID`),
  ADD UNIQUE KEY `SubjectCode` (`SubjectCode`),
  ADD UNIQUE KEY `SubjectCode_2` (`SubjectCode`);

ALTER TABLE `teachersubjects`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `TeacherID` (`TeacherID`),
  ADD KEY `SubjectID` (`SubjectID`);

ALTER TABLE `users`
  ADD PRIMARY KEY (`UserID`),
  ADD UNIQUE KEY `Username` (`Username`);

--
-- AUTO_INCREMENT for tables
--

ALTER TABLE `activitylog`
  MODIFY `LogID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=1;

ALTER TABLE `grades`
  MODIFY `GradeID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

ALTER TABLE `students`
  MODIFY `StudentID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

ALTER TABLE `studentsubjects`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

ALTER TABLE `subjects`
  MODIFY `SubjectID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

ALTER TABLE `teachersubjects`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

ALTER TABLE `users`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Foreign keys
--

ALTER TABLE `grades`
  ADD CONSTRAINT `grades_ibfk_1` FOREIGN KEY (`StudentID`) REFERENCES `students` (`StudentID`),
  ADD CONSTRAINT `grades_ibfk_2` FOREIGN KEY (`SubjectID`) REFERENCES `subjects` (`SubjectID`);

ALTER TABLE `studentsubjects`
  ADD CONSTRAINT `studentsubjects_ibfk_1` FOREIGN KEY (`StudentID`) REFERENCES `students` (`StudentID`),
  ADD CONSTRAINT `studentsubjects_ibfk_2` FOREIGN KEY (`SubjectID`) REFERENCES `subjects` (`SubjectID`);

ALTER TABLE `teachersubjects`
  ADD CONSTRAINT `teachersubjects_ibfk_1` FOREIGN KEY (`TeacherID`) REFERENCES `users` (`UserID`),
  ADD CONSTRAINT `teachersubjects_ibfk_2` FOREIGN KEY (`SubjectID`) REFERENCES `subjects` (`SubjectID`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;

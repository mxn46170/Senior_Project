Adding new views: Any new views will be UserControls. See the Administration module for an example.

If you're new to github: Please fork, and create pull requests when wanting to check something in.
I will review all pull requests.

# Follow these instructions prior to running
1. **Open the SchoolU.sln file.**
2. **Within the Solution Explorer, click the SchoolU_DB.mdf file (within SchoolU_Database module) to open the Server Explorer.**
3. **Right click in the Server Explorer and select “New Query”.**
4. **Copy and paste the following script and execute (Ctrl+Shift+E):**
```
ALTER TABLE Building ALTER COLUMN BuildingName VARCHAR(50) NOT NULL;
 
ALTER TABLE Major ADD MajorAbbr VARCHAR (20) NULL;
 
ALTER TABLE dbo.Department ADD DepartmentAbbr VARCHAR(20) NULL;
 
ALTER TABLE dbo.Semester ADD StartDate DATE NULL;
ALTER TABLE dbo.Semester ADD EndDate DATE NULL;
 
ALTER TABLE dbo.Events ADD EndDate DATE NULL;
 
ALTER TABLE Events ALTER COLUMN EventDescription VARCHAR(255) NULL;
 
ALTER TABLE dbo.StudentYear ALTER COLUMN StudentYearDescription VARCHAR(20) NOT NULL;
 
ALTER TABLE dbo.Student ALTER COLUMN StudentYearId INT NOT NULL;
 
ALTER TABLE Events ALTER COLUMN EventStartTime DATETIME NULL;
ALTER TABLE Events ALTER COLUMN EventEndTime DATETIME NULL;
 
ALTER TABLE dbo.Class ALTER COLUMN ClassStartTime DATETIME NULL;
ALTER TABLE dbo.Class ALTER COLUMN ClassEndTime DATETIME NULL;
 
ALTER TABLE Professor DROP COLUMN ProfessorSalary
 
ALTER TABLE dbo.Student ALTER COLUMN StudentFirstName VARCHAR(50) NULL;
ALTER TABLE dbo.Student ALTER COLUMN StudentLastName VARCHAR(50) NULL;
ALTER TABLE dbo.Student ALTER COLUMN StudentEmail VARCHAR(50) NULL;
 
ALTER TABLE dbo.Professor ALTER COLUMN ProfessorFirstName VARCHAR(50) NULL;
ALTER TABLE dbo.Professor ALTER COLUMN ProfessorLastName VARCHAR(50) NULL;
ALTER TABLE dbo.Professor ALTER COLUMN ProfessorPhoneNumber VARCHAR(20) NULL;
ALTER TABLE dbo.Professor ALTER COLUMN ProfessorEmail VARCHAR (50) NULL;
ALTER TABLE dbo.Professor ALTER COLUMN DepartmentId INT NULL;
 
ALTER TABLE dbo.Class DROP CONSTRAINT FK_Course_PrerequisiteCourse;
ALTER TABLE dbo.Class DROP COLUMN CoursePreRequisite
 
CREATE TABLE [dbo].[PreRequisites] (
    [PreRequisitesId]   INT         IDENTITY (1, 1) NOT NULL,
    [CourseId]   INT NOT NULL,
    [PrereqId]   INT NOT NULL,
    PRIMARY KEY CLUSTERED ([PreRequisitesId] ASC),
    CONSTRAINT [FK_CourseId_Prereqs] FOREIGN KEY ([CourseID]) REFERENCES [dbo].[Course] ([CourseID]),
    CONSTRAINT [FK_PrereqId_Prereqs] FOREIGN KEY ([PrereqId]) REFERENCES [dbo].[Course] ([CourseID]));
```
---
5. **Copy and paste the following script and execute (Ctrl+Shift+E):**

```
IF (SELECT OBJECT_ID('tempdb..#tmpErrors')) IS NOT NULL DROP TABLE #tmpErrors
GO
CREATE TABLE #tmpErrors (Error int)
GO
SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL READ COMMITTED
GO
BEGIN TRANSACTION
GO
PRINT N'Dropping [dbo].[FK_StudentMinor_StudentId]...';
GO
ALTER TABLE [dbo].[StudentMinor] DROP CONSTRAINT [FK_StudentMinor_StudentId];
GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END
IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES              (1);
        BEGIN TRANSACTION;
    END
GO
PRINT N'Dropping [dbo].[FK_StudentMinor_MinorId]...';
GO
ALTER TABLE [dbo].[StudentMinor] DROP CONSTRAINT [FK_StudentMinor_MinorId];
GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END
IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES              (1);
        BEGIN TRANSACTION;
    END
GO
PRINT N'Starting rebuilding table [dbo].[StudentMinor]...';
GO
BEGIN TRANSACTION;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET XACT_ABORT ON;
CREATE TABLE [dbo].[tmp_ms_xx_StudentMinor] (
    [StudentMinorId] INT IDENTITY (1, 1) NOT NULL,
    [StudentId]     INT NOT NULL,
    [MinorId]       INT NOT NULL,
    PRIMARY KEY CLUSTERED ([StudentMinorId] ASC)
);
IF EXISTS (SELECT TOP 1 1
        FROM   [dbo].[StudentMinor])
    BEGIN
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_StudentMinor] ON;
        INSERT INTO [dbo].[tmp_ms_xx_StudentMinor] ([StudentMinorId], [StudentId], [MinorId])
        SELECT   [StudentMinorId],
                [StudentId],
                [MinorId]
        FROM    [dbo].[StudentMinor]
        ORDER BY [StudentMinorId] ASC;
        SET IDENTITY_INSERT [dbo].[tmp_ms_xx_StudentMinor] OFF;
    END
DROP TABLE [dbo].[StudentMinor];
EXECUTE sp_rename N'[dbo].[tmp_ms_xx_StudentMinor]', N'StudentMinor';
COMMIT TRANSACTION;
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END
IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES              (1);
        BEGIN TRANSACTION;
    END
GO
PRINT N'Creating [dbo].[FK_StudentMinor_StudentId]...';
GO
ALTER TABLE [dbo].[StudentMinor] WITH NOCHECK
    ADD CONSTRAINT [FK_StudentMinor_StudentId] FOREIGN KEY ([StudentId]) REFERENCES [dbo].[Student] ([StudentId]);
GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END
IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES              (1);
        BEGIN TRANSACTION;
    END
GO
PRINT N'Creating [dbo].[FK_StudentMinor_MinorId]...';
GO
ALTER TABLE [dbo].[StudentMinor] WITH NOCHECK
    ADD CONSTRAINT [FK_StudentMinor_MinorId] FOREIGN KEY ([MinorId]) REFERENCES [dbo].[Major] ([MajorId]);
GO
IF @@ERROR <> 0
   AND @@TRANCOUNT > 0
    BEGIN
        ROLLBACK;
    END
 
IF @@TRANCOUNT = 0
    BEGIN
        INSERT  INTO #tmpErrors (Error)
        VALUES              (1);
        BEGIN TRANSACTION;
    END
GO
IF EXISTS (SELECT * FROM #tmpErrors) ROLLBACK TRANSACTION
GO
IF @@TRANCOUNT>0 BEGIN
PRINT N'The transacted portion of the database update succeeded.'
COMMIT TRANSACTION
END
ELSE PRINT N'The transacted portion of the database update failed.'
GO
DROP TABLE #tmpErrors
GO
PRINT N'Checking existing data against newly created constraints';
GO
USE [$(DatabaseName)];
GO
ALTER TABLE [dbo].[StudentMinor] WITH CHECK CHECK CONSTRAINT [FK_StudentMinor_StudentId];
ALTER TABLE [dbo].[StudentMinor] WITH CHECK CHECK CONSTRAINT [FK_StudentMinor_MinorId];
GO
PRINT N'Update complete.';
GO
```
---

6. **Open the SchoolU_EF.edmx file from the Solution Explorer.**
7. **Delete the Building, Event, Class, and Professor tables (you will be prompted twice to verify that you want to delete the table, click “Yes” for both prompts).**
8. **Right click anywhere in the .edmx file and select “Update Model from Database…”**
9. **Make sure the the “Tables” checkbox is checked on the “Add” tab of the Update Wizard.**
10. **Select “Finish”.**
11. **Copy and paste the following query and execute:**

```
INSERT INTO admin VALUES(1, 'admin', 'secure');
 
INSERT INTO Building VALUES ('Building one', 'B1', 'A');
 
INSERT INTO BuildingRooms VALUES (1, 50, 'A');
 
INSERT INTO studentYear VALUES('Freshman'), ('Sophomore'), ('Junior'), ('Senior');
 
INSERT INTO schoolYear VALUES ('2019-08-15', '2020-5-10', 'A');
 
INSERT INTO semester VALUES (1, 'Fall', '2018-08-15', '2018-12-15'), (1, 'Spring', '2019-01-15', '2019-05-10'), (1, 'Summer', '2019-06-10', '2019-07-20');
 
INSERT INTO department VALUES ('Computer Science', 'CS');
 
INSERT INTO course VALUES ('Introduction to Programming', 1);
 
INSERT INTO professor VALUES ('Professor1', 'Professor1', 'Professor', 'secure', '123-456-1234', 'professor@gmail.com', '1');
 
INSERT INTO Student VALUES ('Bob1', 'Smith1', 'Bob1', 'BobSmith1@gmail.com', '1secure', 1), ('Bob2', 'Smith2', 'Bob2', 'BobSmith2@gmail.com', '2secure', 2), ('Bob3', 'Smith3', 'Bob3', 'BobSmith3@gmail.com', '3secure', 3), ('Bob4', 'Smith4', 'Bob4', 'BobSmith4@gmail.com', '4secure', 4);
 
INSERT INTO Major VALUES ('Aerospace Engineering', 'AE'), ('Physics', 'PHYS'), ('Political Science', 'PoliSci'), ('Mathematics', 'MA');
 
INSERT INTO StudentMajor VALUES (2, 3), (3, 4), (4, 6), (5, 5);
INSERT INTO StudentMinor VALUES (2, 4), (3, 6), (4, 2), (5, 1);
 
INSERT INTO PreRequisites VALUES (1, 1);
```

12. **Right click the solution at the top of the Solution Explorer and select “Rebuild Solution”.**

The program should now work properly

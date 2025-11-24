-- Migration SQL: convert dbo.Students.Id to IDENTITY(1,1)
-- Review and BACKUP your database before running.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- 0) Safety check: ensure there are no foreign keys from other tables referencing dbo.Students
IF EXISTS (
    SELECT 1
    FROM sys.foreign_keys fk
    WHERE fk.referenced_object_id = OBJECT_ID('dbo.Students')
      AND fk.parent_object_id <> OBJECT_ID('dbo.Students')
)
BEGIN
    RAISERROR('There are external foreign keys referencing dbo.Students. Resolve them before running this script.', 16, 1);
    ROLLBACK TRANSACTION;
    RETURN;
END

-- 1) Drop FK from Students -> Majors if present (migration-created name used in your project)
IF OBJECT_ID('FK_Students_Majors_MajorId','F') IS NOT NULL
BEGIN
    ALTER TABLE dbo.Students DROP CONSTRAINT FK_Students_Majors_MajorId;
END

-- 2) Create a temporary table with an IDENTITY Id column
IF OBJECT_ID('dbo.Students_temp', 'U') IS NOT NULL
    DROP TABLE dbo.Students_temp;

CREATE TABLE dbo.Students_temp (
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name nvarchar(max) NULL,
    Major nvarchar(max) NULL,
    Mentorid int NOT NULL,
    Address nvarchar(max) NULL,
    PhoneNumber nvarchar(max) NULL,
    MajorId int NULL
);

-- 3) Copy data from original table to temp table preserving original Id values
-- Use IDENTITY_INSERT on the temp table to allow explicit Id values to be inserted.
SET IDENTITY_INSERT dbo.Students_temp ON;

INSERT INTO dbo.Students_temp (Id, Name, Major, Mentorid, Address, PhoneNumber, MajorId)
SELECT Id, Name, Major, Mentorid, Address, PhoneNumber, MajorId
FROM dbo.Students;

SET IDENTITY_INSERT dbo.Students_temp OFF;

-- 4) Drop original table and replace with temp table
DROP TABLE dbo.Students;

EXEC sp_rename 'dbo.Students_temp', 'Students';

-- 5) Recreate FK from Students -> Majors
ALTER TABLE dbo.Students
ADD CONSTRAINT FK_Students_Majors_MajorId FOREIGN KEY (MajorId) REFERENCES dbo.Majors(MajorId);

-- 6) Ensure identity seed starts after the current max Id
DECLARE @maxId int = (SELECT ISNULL(MAX(Id), 0) FROM dbo.Students);
IF @maxId < 1 SET @maxId = 1;
DBCC CHECKIDENT ('dbo.Students', RESEED, @maxId);

COMMIT TRANSACTION;
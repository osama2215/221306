-- Run in SSMS / Azure Data Studio against the same database in your connection string
SELECT COLUMN_NAME, DATA_TYPE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'Courses';
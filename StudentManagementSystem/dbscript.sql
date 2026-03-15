CREATE DATABASE StudentManagementSystem;
GO

USE StudentManagementSystem;
GO

CREATE TABLE Students
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PhoneNumber VARCHAR(10) NOT NULL UNIQUE,
    EmailId NVARCHAR(100) NOT NULL UNIQUE,
    CreatedAt DATETIME DEFAULT GETDATE()
);


CREATE TABLE Classes
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(100)
);

CREATE TABLE StudentClasses
(
    StudentId INT NOT NULL,
    ClassId INT NOT NULL,
    PRIMARY KEY (StudentId, ClassId),
    FOREIGN KEY (StudentId) REFERENCES Students(Id) ON DELETE CASCADE,
    FOREIGN KEY (ClassId) REFERENCES Classes(Id) ON DELETE CASCADE
);

INSERT INTO Classes (Name, Description)
VALUES 
('Mathematics', 'Basic Mathematics'),
('Physics', 'Physics Fundamentals'),
('Chemistry', 'Chemistry Basics'),
('Computer Science', 'Introduction to Programming');

SELECT * FROM Students
SELECT * FROM Classes
SELECT * FROM StudentClasses
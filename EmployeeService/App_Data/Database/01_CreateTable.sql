USE [Test]
GO

CREATE TABLE dbo.Employee
(
    ID INT NOT NULL IDENTITY(1, 1),
    Name VARCHAR(100) NOT NULL,
    ManagerID INT  NULL,
    Enable BIT NOT NULL
    
    CONSTRAINT DF_Employee_Enable DEFAULT (1),

    CONSTRAINT PK_Employee
        PRIMARY KEY CLUSTERED (ID),

    CONSTRAINT FK_Employee_Manager
        FOREIGN KEY (ManagerID) REFERENCES dbo.Employee (ID)
);
GO

CREATE NONCLUSTERED INDEX IX_Employee_ManagerID
    ON dbo.Employee (ManagerID);
GO

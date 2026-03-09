USE [Test]
GO

SET IDENTITY_INSERT dbo.Employee ON;

INSERT INTO dbo.Employee (ID, Name, ManagerID, Enable)
VALUES
    (1, 'Andrey', NULL, 1),
    (2, 'Alexey', 1,   1),
    (3, 'Nir',    2,   1),
    (4, 'Smadar', 3,   1),
    (5, 'Barak',  3,   1),
    (6, 'Roman',  2,   1);

SET IDENTITY_INSERT dbo.Employee OFF;
GO

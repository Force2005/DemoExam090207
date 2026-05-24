IF DB_ID(N'demo_090207_m1_m2') IS NULL
BEGIN
    CREATE DATABASE demo_090207_m1_m2;
END;
GO

USE demo_090207_m1_m2;
GO

IF OBJECT_ID(N'dbo.Users', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Users
    (
        UserId int IDENTITY(1,1) NOT NULL CONSTRAINT PK_Users PRIMARY KEY,
        FullName nvarchar(200) NOT NULL,
        Login nvarchar(100) NOT NULL,
        PasswordHash nvarchar(500) NOT NULL,
        RoleName nvarchar(50) NOT NULL,
        FailedAttempts int NOT NULL CONSTRAINT DF_Users_FailedAttempts DEFAULT (0),
        IsBlocked bit NOT NULL CONSTRAINT DF_Users_IsBlocked DEFAULT (0),
        CreatedAt datetime2 NOT NULL CONSTRAINT DF_Users_CreatedAt DEFAULT (SYSDATETIME())
    );
END;
GO

IF NOT EXISTS (
    SELECT 1
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'dbo.Users')
      AND name = N'UX_Users_Login'
)
BEGIN
    CREATE UNIQUE INDEX UX_Users_Login ON dbo.Users(Login);
END;
GO

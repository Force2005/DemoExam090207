USE master;
GO

IF DB_ID('ProductionDB') IS NULL
    CREATE DATABASE ProductionDB;
GO

USE ProductionDB;
GO

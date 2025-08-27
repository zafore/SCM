-- =====================================================
-- SCM Microservices Database Setup Script
-- Creates separate databases for each microservice
-- =====================================================

USE [master]
GO

-- =====================================================
-- 1. SUPPLIERS DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'SuppliersDB')
BEGIN
    CREATE DATABASE [SuppliersDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'SuppliersDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SuppliersDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'SuppliersDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\SuppliersDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

-- =====================================================
-- 2. INVENTORY DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'InventoryDB')
BEGIN
    CREATE DATABASE [InventoryDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'InventoryDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\InventoryDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'InventoryDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\InventoryDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

-- =====================================================
-- 3. ORDER DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'OrderDB')
BEGIN
    CREATE DATABASE [OrderDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'OrderDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\OrderDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'OrderDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\OrderDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

-- =====================================================
-- 4. IDENTITY DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'IdentityDB')
BEGIN
    CREATE DATABASE [IdentityDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'IdentityDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\IdentityDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'IdentityDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\IdentityDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

-- =====================================================
-- 5. CUSTOMER DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'CustomerDB')
BEGIN
    CREATE DATABASE [CustomerDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'CustomerDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CustomerDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'CustomerDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\CustomerDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

-- =====================================================
-- 6. ADMIN DATABASE
-- =====================================================
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'AdminDB')
BEGIN
    CREATE DATABASE [AdminDB]
    CONTAINMENT = NONE
    ON PRIMARY 
    ( 
        NAME = N'AdminDB', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AdminDB.mdf' , 
        SIZE = 4096KB , 
        MAXSIZE = UNLIMITED, 
        FILEGROWTH = 1024KB 
    )
    LOG ON 
    ( 
        NAME = N'AdminDB_log', 
        FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.MSSQLSERVER\MSSQL\DATA\AdminDB_log.ldf' , 
        SIZE = 1280KB , 
        MAXSIZE = 2048GB , 
        FILEGROWTH = 10%
    )
END
GO

PRINT 'All SCM Microservices databases created successfully!'
PRINT 'Databases created:'
PRINT '- SuppliersDB (Suppliers.Api)'
PRINT '- InventoryDB (InventoryMicroservice)'
PRINT '- OrderDB (OrderMicroservice)'
PRINT '- IdentityDB (IdentityMicroservice)'
PRINT '- CustomerDB (CustomerMicroservice)'
PRINT '- AdminDB (AdminMicroservice)'
GO

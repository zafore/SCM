-- =====================================================
-- تحديث قاعدة البيانات لإضافة الحسابات المحاسبية
-- =====================================================

USE [SuppliersDB]
GO

-- =====================================================
-- 1. إضافة AccountId لجدول الموردين
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID('Suppliers') AND name = 'AccountId')
BEGIN
    ALTER TABLE [dbo].[Suppliers] 
    ADD [AccountId] [int] NULL;
END
GO

-- =====================================================
-- 2. إنشاء جدول دليل الحسابات
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('ChartOfAccounts') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ChartOfAccounts](
        [AccountId] [int] IDENTITY(1,1) NOT NULL,
        [AccountCode] [nvarchar](20) NOT NULL,
        [AccountName] [nvarchar](100) NOT NULL,
        [AccountType] [nvarchar](50) NOT NULL, -- Asset, Liability, Equity, Revenue, Expense
        [ParentAccountId] [int] NULL,
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [UpdatedDate] [datetime] NULL,
        CONSTRAINT [PK_ChartOfAccounts] PRIMARY KEY CLUSTERED ([AccountId] ASC)
    );
END
GO

-- =====================================================
-- 3. إنشاء جدول قيود اليومية
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('JournalEntries') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[JournalEntries](
        [EntryId] [int] IDENTITY(1,1) NOT NULL,
        [OrderId] [int] NULL,
        [EntryDate] [datetime] NOT NULL,
        [Description] [nvarchar](500) NOT NULL,
        [TotalAmount] [decimal](18,2) NOT NULL,
        [CreatedBy] [int] NOT NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [IsPosted] [bit] NOT NULL DEFAULT 0,
        CONSTRAINT [PK_JournalEntries] PRIMARY KEY CLUSTERED ([EntryId] ASC)
    );
END
GO

-- =====================================================
-- 4. إنشاء جدول تفاصيل قيود اليومية
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('JournalEntryDetails') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[JournalEntryDetails](
        [DetailId] [int] IDENTITY(1,1) NOT NULL,
        [EntryId] [int] NOT NULL,
        [AccountId] [int] NOT NULL,
        [Debit] [decimal](18,2) NOT NULL DEFAULT 0,
        [Credit] [decimal](18,2) NOT NULL DEFAULT 0,
        [Description] [nvarchar](500) NULL,
        CONSTRAINT [PK_JournalEntryDetails] PRIMARY KEY CLUSTERED ([DetailId] ASC)
    );
END
GO

-- =====================================================
-- 5. إنشاء جدول دفعيات العملاء
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('CustomerPayments') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[CustomerPayments](
        [PaymentId] [int] IDENTITY(1,1) NOT NULL,
        [OrderId] [int] NOT NULL,
        [CustomerId] [int] NOT NULL,
        [Amount] [decimal](18,2) NOT NULL,
        [PaymentMethodId] [int] NOT NULL,
        [PaymentDate] [datetime] NOT NULL,
        [PaymentStatesId] [int] NOT NULL,
        [CurrencyId] [int] NOT NULL,
        [Notes] [nvarchar](500) NULL,
        [CreatedBy] [int] NOT NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_CustomerPayments] PRIMARY KEY CLUSTERED ([PaymentId] ASC)
    );
END
GO

-- =====================================================
-- 6. إنشاء جدول دفعيات الموردين
-- =====================================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID('SupplierPayments') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[SupplierPayments](
        [PaymentId] [int] IDENTITY(1,1) NOT NULL,
        [OrderId] [int] NOT NULL,
        [SupplierId] [int] NOT NULL,
        [Amount] [decimal](18,2) NOT NULL,
        [PaymentMethodId] [int] NOT NULL,
        [PaymentDate] [datetime] NOT NULL,
        [PaymentStatesId] [int] NOT NULL,
        [CurrencyId] [int] NOT NULL,
        [Notes] [nvarchar](500) NULL,
        [CreatedBy] [int] NOT NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        CONSTRAINT [PK_SupplierPayments] PRIMARY KEY CLUSTERED ([PaymentId] ASC)
    );
END
GO

-- =====================================================
-- 7. إضافة العلاقات (Foreign Keys)
-- =====================================================

-- ربط الموردين بدليل الحسابات
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_Suppliers_ChartOfAccounts'))
BEGIN
    ALTER TABLE [dbo].[Suppliers] 
    ADD CONSTRAINT [FK_Suppliers_ChartOfAccounts] 
    FOREIGN KEY([AccountId]) REFERENCES [dbo].[ChartOfAccounts] ([AccountId]);
END
GO

-- ربط تفاصيل قيود اليومية بقيود اليومية
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_JournalEntryDetails_JournalEntries'))
BEGIN
    ALTER TABLE [dbo].[JournalEntryDetails] 
    ADD CONSTRAINT [FK_JournalEntryDetails_JournalEntries] 
    FOREIGN KEY([EntryId]) REFERENCES [dbo].[JournalEntries] ([EntryId]) ON DELETE CASCADE;
END
GO

-- ربط تفاصيل قيود اليومية بدليل الحسابات
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_JournalEntryDetails_ChartOfAccounts'))
BEGIN
    ALTER TABLE [dbo].[JournalEntryDetails] 
    ADD CONSTRAINT [FK_JournalEntryDetails_ChartOfAccounts] 
    FOREIGN KEY([AccountId]) REFERENCES [dbo].[ChartOfAccounts] ([AccountId]);
END
GO

-- ربط دفعيات العملاء بطرق الدفع
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_CustomerPayments_PaymentMethod'))
BEGIN
    ALTER TABLE [dbo].[CustomerPayments] 
    ADD CONSTRAINT [FK_CustomerPayments_PaymentMethod] 
    FOREIGN KEY([PaymentMethodId]) REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId]);
END
GO

-- ربط دفعيات العملاء بحالات الدفع
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_CustomerPayments_PaymentStates'))
BEGIN
    ALTER TABLE [dbo].[CustomerPayments] 
    ADD CONSTRAINT [FK_CustomerPayments_PaymentStates] 
    FOREIGN KEY([PaymentStatesId]) REFERENCES [dbo].[PaymentStates] ([PaymentStatesId]);
END
GO

-- ربط دفعيات العملاء بالعملات
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_CustomerPayments_Currency'))
BEGIN
    ALTER TABLE [dbo].[CustomerPayments] 
    ADD CONSTRAINT [FK_CustomerPayments_Currency] 
    FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]);
END
GO

-- ربط دفعيات الموردين بطرق الدفع
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_SupplierPayments_PaymentMethod'))
BEGIN
    ALTER TABLE [dbo].[SupplierPayments] 
    ADD CONSTRAINT [FK_SupplierPayments_PaymentMethod] 
    FOREIGN KEY([PaymentMethodId]) REFERENCES [dbo].[PaymentMethod] ([PaymentMethodId]);
END
GO

-- ربط دفعيات الموردين بحالات الدفع
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_SupplierPayments_PaymentStates'))
BEGIN
    ALTER TABLE [dbo].[SupplierPayments] 
    ADD CONSTRAINT [FK_SupplierPayments_PaymentStates] 
    FOREIGN KEY([PaymentStatesId]) REFERENCES [dbo].[PaymentStates] ([PaymentStatesId]);
END
GO

-- ربط دفعيات الموردين بالعملات
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_SupplierPayments_Currency'))
BEGIN
    ALTER TABLE [dbo].[SupplierPayments] 
    ADD CONSTRAINT [FK_SupplierPayments_Currency] 
    FOREIGN KEY([CurrencyId]) REFERENCES [dbo].[Currency] ([CurrencyId]);
END
GO

-- ربط دفعيات الموردين بالموردين
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID('FK_SupplierPayments_Suppliers'))
BEGIN
    ALTER TABLE [dbo].[SupplierPayments] 
    ADD CONSTRAINT [FK_SupplierPayments_Suppliers] 
    FOREIGN KEY([SupplierId]) REFERENCES [dbo].[Suppliers] ([SupplierId]);
END
GO

-- =====================================================
-- 8. إدراج البيانات الأساسية لدليل الحسابات
-- =====================================================

-- الحسابات الرئيسية
IF NOT EXISTS (SELECT * FROM ChartOfAccounts WHERE AccountCode = '101')
BEGIN
    INSERT INTO ChartOfAccounts (AccountCode, AccountName, AccountType, ParentAccountId)
    VALUES 
    -- الأصول
    ('101', 'البنك', 'Asset', NULL),
    ('102', 'النقدية', 'Asset', NULL),
    ('103', 'العملاء', 'Asset', NULL),
    
    -- الخصوم
    ('301', 'الموردين', 'Liability', NULL),
    
    -- حقوق الملكية
    ('401', 'رأس المال', 'Equity', NULL),
    ('402', 'الأرباح المحتجزة', 'Equity', NULL),
    
    -- الإيرادات والمصروفات
    ('501', 'المبيعات', 'Revenue', NULL),
    ('502', 'المشتريات', 'Expense', NULL),
    ('503', 'تكاليف الشحن', 'Expense', NULL),
    ('504', 'المصروفات الإدارية', 'Expense', NULL);
END
GO

-- =====================================================
-- 9. إنشاء الفهارس لتحسين الأداء
-- =====================================================

-- فهرس على AccountCode
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('ChartOfAccounts') AND name = 'IX_ChartOfAccounts_AccountCode')
BEGIN
    CREATE UNIQUE INDEX [IX_ChartOfAccounts_AccountCode] ON [dbo].[ChartOfAccounts] ([AccountCode]);
END
GO

-- فهرس على EntryDate
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('JournalEntries') AND name = 'IX_JournalEntries_EntryDate')
BEGIN
    CREATE INDEX [IX_JournalEntries_EntryDate] ON [dbo].[JournalEntries] ([EntryDate]);
END
GO

-- فهرس على OrderId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('JournalEntries') AND name = 'IX_JournalEntries_OrderId')
BEGIN
    CREATE INDEX [IX_JournalEntries_OrderId] ON [dbo].[JournalEntries] ([OrderId]);
END
GO

-- فهرس على PaymentDate
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('CustomerPayments') AND name = 'IX_CustomerPayments_PaymentDate')
BEGIN
    CREATE INDEX [IX_CustomerPayments_PaymentDate] ON [dbo].[CustomerPayments] ([PaymentDate]);
END
GO

-- فهرس على PaymentDate
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('SupplierPayments') AND name = 'IX_SupplierPayments_PaymentDate')
BEGIN
    CREATE INDEX [IX_SupplierPayments_PaymentDate] ON [dbo].[SupplierPayments] ([PaymentDate]);
END
GO

PRINT 'تم تحديث قاعدة البيانات بنجاح!'
PRINT 'الجداول المضافة:'
PRINT '- ChartOfAccounts (دليل الحسابات)'
PRINT '- JournalEntries (قيود اليومية)'
PRINT '- JournalEntryDetails (تفاصيل قيود اليومية)'
PRINT '- CustomerPayments (دفعيات العملاء)'
PRINT '- SupplierPayments (دفعيات الموردين)'
PRINT 'تم إضافة AccountId لجدول الموردين'
PRINT 'تم إنشاء العلاقات والفهارس'
GO

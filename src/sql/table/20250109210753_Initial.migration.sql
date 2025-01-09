PRINT 'Before TRY'
BEGIN TRY
	BEGIN TRAN
	PRINT 'First Statement in the TRY block'
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[Subject] (
        [SubjectId] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NULL,
        [GivenName] nvarchar(100) NULL,
        [FamilyName] nvarchar(100) NULL,
        [UserPrincipalName] nvarchar(100) NULL,
        [CreatedDate] datetime2 NOT NULL,
        CONSTRAINT [PK_Subject] PRIMARY KEY ([SubjectId])
    );
    DECLARE @description AS sql_variant;
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'SubjectId';
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'Name';
    SET @description = N'Subject primary key';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'GivenName';
    SET @description = N'Subject Surname ()';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'FamilyName';
    SET @description = N'Username (upn claim)';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'UserPrincipalName';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Subject', 'COLUMN', N'CreatedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[Policy] (
        [PolicyId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NULL,
        [Description] nvarchar(255) NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CreatedSubjectId] uniqueidentifier NOT NULL,
        [LastModifiedDate] datetime2 NOT NULL,
        [LastModifiedSubjectId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Policy] PRIMARY KEY ([PolicyId]),
        CONSTRAINT [FK_Policy_Subject_CreatedSubjectId] FOREIGN KEY ([CreatedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId]),
        CONSTRAINT [FK_Policy_Subject_LastModifiedSubjectId] FOREIGN KEY ([LastModifiedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId])
    );
    -- DECLARE @description AS sql_variant;
    SET @description = N'Policies of the application';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy';
    SET @description = N'Auto incrementing id that is for internal use only';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'PolicyId';
    SET @description = N'Name of the policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'Name';
    SET @description = N'Policy description';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'Description';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'CreatedDate';
    SET @description = N'Date and time entity was last modified';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'LastModifiedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[Permission] (
        [PermissionId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NULL,
        [Description] nvarchar(255) NULL,
        [PolicyId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CreatedSubjectId] uniqueidentifier NOT NULL,
        [LastModifiedDate] datetime2 NOT NULL,
        [LastModifiedSubjectId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Permission] PRIMARY KEY ([PermissionId]),
        CONSTRAINT [FK_Permission_Policy_PolicyId] FOREIGN KEY ([PolicyId]) REFERENCES [dbo].[Policy] ([PolicyId]),
        CONSTRAINT [FK_Permission_Subject_CreatedSubjectId] FOREIGN KEY ([CreatedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId]),
        CONSTRAINT [FK_Permission_Subject_LastModifiedSubjectId] FOREIGN KEY ([LastModifiedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId])
    );
    -- DECLARE @description AS sql_variant;
    SET @description = N'Permissions available within a policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission';
    SET @description = N'Auto incrementing id that is for internal use only';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'PermissionId';
    SET @description = N'Name of the Permission';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'Name';
    SET @description = N'Permission description';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'Description';
    SET @description = N'FK to Policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'PolicyId';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'CreatedDate';
    SET @description = N'Date and time entity was last modified';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'LastModifiedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[Role] (
        [RoleId] int NOT NULL IDENTITY,
        [Name] nvarchar(100) NULL,
        [Description] nvarchar(255) NULL,
        [PolicyId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CreatedSubjectId] uniqueidentifier NOT NULL,
        [LastModifiedDate] datetime2 NOT NULL,
        [LastModifiedSubjectId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([RoleId]),
        CONSTRAINT [FK_Role_Policy_PolicyId] FOREIGN KEY ([PolicyId]) REFERENCES [dbo].[Policy] ([PolicyId]),
        CONSTRAINT [FK_Role_Subject_CreatedSubjectId] FOREIGN KEY ([CreatedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId]),
        CONSTRAINT [FK_Role_Subject_LastModifiedSubjectId] FOREIGN KEY ([LastModifiedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId])
    );
    -- DECLARE @description AS sql_variant;
    SET @description = N'Roles within a policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role';
    SET @description = N'Auto incrementing id that is for internal use only';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'RoleId';
    SET @description = N'Name of the Role';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'Name';
    SET @description = N'Role description';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'Description';
    SET @description = N'FK to Policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'PolicyId';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'CreatedDate';
    SET @description = N'Date and time entity was last modified';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'LastModifiedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[PolicyRoleClaim] (
        [Id] int NOT NULL IDENTITY,
        [ClaimType] nvarchar(100) NULL,
        [Value] nvarchar(500) NULL,
        [Description] nvarchar(255) NULL,
        [RoleId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CreatedSubjectId] uniqueidentifier NOT NULL,
        [LastModifiedDate] datetime2 NOT NULL,
        [LastModifiedSubjectId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_PolicyRoleClaim] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_PolicyRoleClaim_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([RoleId]),
        CONSTRAINT [FK_PolicyRoleClaim_Subject_CreatedSubjectId] FOREIGN KEY ([CreatedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId]),
        CONSTRAINT [FK_PolicyRoleClaim_Subject_LastModifiedSubjectId] FOREIGN KEY ([LastModifiedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId])
    );
    -- DECLARE @description AS sql_variant;
    SET @description = N'PolicyRoleClaims within a role';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim';
    SET @description = N'Auto incrementing id that is for internal use only';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'Id';
    SET @description = N'ClaimType of the PolicyRoleClaim, i.e. sub/role/group etc';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'ClaimType';
    SET @description = N'Value of the claim';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'Value';
    SET @description = N'PolicyRoleClaim description';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'Description';
    SET @description = N'FK to Role';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'RoleId';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'CreatedDate';
    SET @description = N'Date and time entity was last modified';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'LastModifiedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE TABLE [dbo].[RolePermission] (
        [RolePermissionId] int NOT NULL IDENTITY,
        [RoleId] int NOT NULL,
        [PermissionId] int NOT NULL,
        [CreatedDate] datetime2 NOT NULL,
        [CreatedSubjectId] uniqueidentifier NOT NULL,
        [LastModifiedDate] datetime2 NOT NULL,
        [LastModifiedSubjectId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_RolePermission] PRIMARY KEY ([RolePermissionId]),
        CONSTRAINT [FK_RolePermission_Permission_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [dbo].[Permission] ([PermissionId]),
        CONSTRAINT [FK_RolePermission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([RoleId]),
        CONSTRAINT [FK_RolePermission_Subject_CreatedSubjectId] FOREIGN KEY ([CreatedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId]),
        CONSTRAINT [FK_RolePermission_Subject_LastModifiedSubjectId] FOREIGN KEY ([LastModifiedSubjectId]) REFERENCES [dbo].[Subject] ([SubjectId])
    );
    -- DECLARE @description AS sql_variant;
    SET @description = N'RolePermissions are permissions assigned to a role within a policy';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission';
    SET @description = N'Auto incrementing id that is for internal use only';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'RolePermissionId';
    SET @description = N'FK to Role';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'RoleId';
    SET @description = N'FK to Permission';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'PermissionId';
    SET @description = N'Date and time entity was created';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'CreatedDate';
    SET @description = N'Date and time entity was last modified';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'LastModifiedDate';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Permission_CreatedSubjectId] ON [dbo].[Permission] ([CreatedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Permission_LastModifiedSubjectId] ON [dbo].[Permission] ([LastModifiedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Permission_PolicyId] ON [dbo].[Permission] ([PolicyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Policy_CreatedSubjectId] ON [dbo].[Policy] ([CreatedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Policy_LastModifiedSubjectId] ON [dbo].[Policy] ([LastModifiedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_PolicyRoleClaim_CreatedSubjectId] ON [dbo].[PolicyRoleClaim] ([CreatedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_PolicyRoleClaim_LastModifiedSubjectId] ON [dbo].[PolicyRoleClaim] ([LastModifiedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_PolicyRoleClaim_RoleId] ON [dbo].[PolicyRoleClaim] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Role_CreatedSubjectId] ON [dbo].[Role] ([CreatedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Role_LastModifiedSubjectId] ON [dbo].[Role] ([LastModifiedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_Role_PolicyId] ON [dbo].[Role] ([PolicyId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_RolePermission_CreatedSubjectId] ON [dbo].[RolePermission] ([CreatedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_RolePermission_LastModifiedSubjectId] ON [dbo].[RolePermission] ([LastModifiedSubjectId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_RolePermission_PermissionId] ON [dbo].[RolePermission] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    CREATE INDEX [IX_RolePermission_RoleId] ON [dbo].[RolePermission] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250109210753_Initial'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250109210753_Initial', N'8.0.8');
END;

COMMIT;

	PRINT 'Last Statement in the TRY block'
	COMMIT TRAN
END TRY
BEGIN CATCH
    PRINT 'In CATCH Block'
    IF(@@TRANCOUNT > 0)
        ROLLBACK TRAN;

    THROW; -- Raise error to the client.
END CATCH
PRINT 'After END CATCH'
GO

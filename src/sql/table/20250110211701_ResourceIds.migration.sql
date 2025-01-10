PRINT 'Before TRY'
BEGIN TRY
	BEGIN TRAN
	PRINT 'First Statement in the TRY block'
BEGIN TRANSACTION;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    EXEC sp_rename N'[dbo].[PolicyRoleClaim].[Id]', N'PolicyRoleClaimId', N'COLUMN';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    ALTER TABLE [dbo].[RolePermission] ADD [RolePermissionResourceId] uniqueidentifier NOT NULL DEFAULT newid();
    DECLARE @description AS sql_variant;
    SET @description = N'Public unique identifier';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'RolePermission', 'COLUMN', N'RolePermissionResourceId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    ALTER TABLE [dbo].[Role] ADD [RoleResourceId] uniqueidentifier NOT NULL DEFAULT newid();
    -- DECLARE @description AS sql_variant;
    SET @description = N'Public unique identifier';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Role', 'COLUMN', N'RoleResourceId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    ALTER TABLE [dbo].[PolicyRoleClaim] ADD [PolicyRoleClaimResourceId] uniqueidentifier NOT NULL DEFAULT newid();
    -- DECLARE @description AS sql_variant;
    SET @description = N'Public unique identifier';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'PolicyRoleClaim', 'COLUMN', N'PolicyRoleClaimResourceId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    ALTER TABLE [dbo].[Policy] ADD [PolicyResourceId] uniqueidentifier NOT NULL DEFAULT newid();
    -- DECLARE @description AS sql_variant;
    SET @description = N'Public unique identifier';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Policy', 'COLUMN', N'PolicyResourceId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    ALTER TABLE [dbo].[Permission] ADD [PermissionResourceId] uniqueidentifier NOT NULL DEFAULT newid();
    -- DECLARE @description AS sql_variant;
    SET @description = N'Public unique identifier';
    EXEC sp_addextendedproperty 'MS_Description', @description, 'SCHEMA', N'dbo', 'TABLE', N'Permission', 'COLUMN', N'PermissionResourceId';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RolePermission_RolePermissionResourceId] ON [dbo].[RolePermission] ([RolePermissionResourceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Role_RoleResourceId] ON [dbo].[Role] ([RoleResourceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE INDEX [IX_PolicyRoleClaim_ClaimType_Value] ON [dbo].[PolicyRoleClaim] ([ClaimType], [Value]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE UNIQUE INDEX [IX_PolicyRoleClaim_PolicyRoleClaimResourceId] ON [dbo].[PolicyRoleClaim] ([PolicyRoleClaimResourceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_Policy_Name] ON [dbo].[Policy] ([Name]) WHERE [Name] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Policy_PolicyResourceId] ON [dbo].[Policy] ([PolicyResourceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Permission_PermissionResourceId] ON [dbo].[Permission] ([PermissionResourceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20250110211701_ResourceIds'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20250110211701_ResourceIds', N'8.0.8');
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

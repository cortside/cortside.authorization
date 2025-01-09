DROP TRIGGER IF EXISTS dbo.trRolePermission
GO

---
-- Trigger for RolePermission that will handle logging of both update and delete
-- NOTE: inserted data is current value in row if not altered
---
CREATE TRIGGER trRolePermission
	ON [dbo].[RolePermission]
	FOR UPDATE, DELETE
	AS
		BEGIN
	SET NOCOUNT ON

	DECLARE 
		@AuditLogTransactionId	bigint,
		@Inserted	    		int = 0,
 		@ROWS_COUNT				int

	SELECT @ROWS_COUNT=count(*) from inserted

    -- Check if this is an INSERT, UPDATE or DELETE Action.
    DECLARE @action as varchar(10);
    SET @action = 'INSERT';
    IF EXISTS(SELECT 1 FROM DELETED)
    BEGIN
        SET @action = 
            CASE
                WHEN EXISTS(SELECT 1 FROM INSERTED) THEN 'UPDATE'
                ELSE 'DELETE'
            END
        SELECT @ROWS_COUNT=count(*) from deleted
    END

	-- determine username
	DECLARE @UserName nvarchar(200);
	SELECT TOP 1 @UserName=[LastModifiedSubjectId] FROM inserted;

	-- insert parent transaction
	INSERT INTO audit.AuditLogTransaction (TableName, TableSchema, Action, HostName, ApplicationName, AuditLogin, AuditDate, AffectedRows, DatabaseName, UserId, TransactionId)
	values('RolePermission', 'dbo', @action, CASE WHEN LEN(HOST_NAME()) < 1 THEN ' ' ELSE HOST_NAME() END,
		CASE WHEN LEN(APP_NAME()) < 1 THEN ' ' ELSE APP_NAME() END,
		SUSER_SNAME(), GETDATE(), @ROWS_COUNT, db_name(), @UserName, CURRENT_TRANSACTION_ID()
	)
	Set @AuditLogTransactionId = SCOPE_IDENTITY()

	-- [RolePermissionId]
	IF UPDATE([RolePermissionId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[RolePermissionId]',
				CONVERT(nvarchar(4000), OLD.[RolePermissionId], 126),
				CONVERT(nvarchar(4000), NEW.[RolePermissionId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[RolePermissionId] <> OLD.[RolePermissionId]) 
					Or (NEW.[RolePermissionId] Is Null And OLD.[RolePermissionId] Is Not Null)
					Or (NEW.[RolePermissionId] Is Not Null And OLD.[RolePermissionId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [RoleId]
	IF UPDATE([RoleId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[RoleId]',
				CONVERT(nvarchar(4000), OLD.[RoleId], 126),
				CONVERT(nvarchar(4000), NEW.[RoleId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[RoleId] <> OLD.[RoleId]) 
					Or (NEW.[RoleId] Is Null And OLD.[RoleId] Is Not Null)
					Or (NEW.[RoleId] Is Not Null And OLD.[RoleId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [PermissionId]
	IF UPDATE([PermissionId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[PermissionId]',
				CONVERT(nvarchar(4000), OLD.[PermissionId], 126),
				CONVERT(nvarchar(4000), NEW.[PermissionId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[PermissionId] <> OLD.[PermissionId]) 
					Or (NEW.[PermissionId] Is Null And OLD.[PermissionId] Is Not Null)
					Or (NEW.[PermissionId] Is Not Null And OLD.[PermissionId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [CreatedDate]
	IF UPDATE([CreatedDate]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[CreatedDate]',
				CONVERT(nvarchar(4000), OLD.[CreatedDate], 126),
				CONVERT(nvarchar(4000), NEW.[CreatedDate], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[CreatedDate] <> OLD.[CreatedDate]) 
					Or (NEW.[CreatedDate] Is Null And OLD.[CreatedDate] Is Not Null)
					Or (NEW.[CreatedDate] Is Not Null And OLD.[CreatedDate] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [CreatedSubjectId]
	IF UPDATE([CreatedSubjectId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[CreatedSubjectId]',
				CONVERT(nvarchar(4000), OLD.[CreatedSubjectId], 126),
				CONVERT(nvarchar(4000), NEW.[CreatedSubjectId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[CreatedSubjectId] <> OLD.[CreatedSubjectId]) 
					Or (NEW.[CreatedSubjectId] Is Null And OLD.[CreatedSubjectId] Is Not Null)
					Or (NEW.[CreatedSubjectId] Is Not Null And OLD.[CreatedSubjectId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [LastModifiedDate]
	IF UPDATE([LastModifiedDate]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[LastModifiedDate]',
				CONVERT(nvarchar(4000), OLD.[LastModifiedDate], 126),
				CONVERT(nvarchar(4000), NEW.[LastModifiedDate], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[LastModifiedDate] <> OLD.[LastModifiedDate]) 
					Or (NEW.[LastModifiedDate] Is Null And OLD.[LastModifiedDate] Is Not Null)
					Or (NEW.[LastModifiedDate] Is Not Null And OLD.[LastModifiedDate] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

	-- [LastModifiedSubjectId]
	IF UPDATE([LastModifiedSubjectId]) OR @action in ('INSERT', 'DELETE')      
		BEGIN       
			INSERT INTO audit.AuditLog (AuditLogTransactionId, PrimaryKey, ColumnName, OldValue, NewValue, Key1)
			SELECT
				@AuditLogTransactionId,
				convert(nvarchar(1500), IsNull('[[RolePermissionId]]='+CONVERT(nvarchar(4000), IsNull(OLD.[RolePermissionId], NEW.[RolePermissionId]), 0), '[[RolePermissionId]] Is Null')),
				'[LastModifiedSubjectId]',
				CONVERT(nvarchar(4000), OLD.[LastModifiedSubjectId], 126),
				CONVERT(nvarchar(4000), NEW.[LastModifiedSubjectId], 126),
				convert(nvarchar(4000), COALESCE(OLD.[RolePermissionId], NEW.[RolePermissionId], null))
			FROM deleted OLD 
			LEFT JOIN inserted NEW On (NEW.[RolePermissionId] = OLD.[RolePermissionId] or (NEW.[RolePermissionId] Is Null and OLD.[RolePermissionId] Is Null))
			WHERE ((NEW.[LastModifiedSubjectId] <> OLD.[LastModifiedSubjectId]) 
					Or (NEW.[LastModifiedSubjectId] Is Null And OLD.[LastModifiedSubjectId] Is Not Null)
					Or (NEW.[LastModifiedSubjectId] Is Not Null And OLD.[LastModifiedSubjectId] Is Null))
			set @inserted = @inserted + @@ROWCOUNT
		END

END
GO

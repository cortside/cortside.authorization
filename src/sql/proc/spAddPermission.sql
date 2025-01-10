CREATE OR ALTER PROCEDURE spAddPermission
    @PolicyName varchar(100),
    @RoleName varchar(100),
    @PermissionName varchar(100),
    @Description varchar(255)
AS
BEGIN
    -- Error handling
    BEGIN TRY
        -- Check if arguments are valid
        IF @PolicyName IS NULL
        BEGIN
            RAISERROR('Invalid PolicyName', 16, 1);
            RETURN;
        END

        IF @RoleName IS NULL
        BEGIN
            RAISERROR('Invalid RoleName', 16, 1);
            RETURN;
        END

        IF @PermissionName IS NULL
        BEGIN
            RAISERROR('Invalid PermissionName', 16, 1);
            RETURN;
        END

        DECLARE @EmptyGuid UniqueIdentifier = '00000000-0000-0000-0000-000000000000'

        DECLARE @PolicyId int = (SELECT PolicyId FROM dbo.[Policy] WHERE [Name] = @PolicyName);
        DECLARE @RoleId int = (SELECT RoleId FROM dbo.[Role] WHERE [Name] = @RoleName and PolicyId = @PolicyId);
        DECLARE @PermissionId int;

        -- upsert to policy
        IF NOT Exists(Select * from dbo.[Permission] where [Name] = @PermissionName and PolicyId = @PolicyId)
        BEGIN
            INSERT INTO [dbo].[Permission]
                ([Name]
                ,[Description]
                ,[PolicyId]
                ,[CreatedDate]
                ,[CreatedSubjectId]
                ,[LastModifiedDate]
                ,[LastModifiedSubjectId])
            VALUES
                (@PermissionName --<Name, nvarchar(100),>
                ,@Description --<Description, nvarchar(255),>
                ,@PolicyId --<PolicyId, int,>
                ,getutcdate() --<CreatedDate, datetime2(7),>
                ,@EmptyGuid --<CreatedSubjectId, uniqueidentifier,>
                ,getutcdate() --<LastModifiedDate, datetime2(7),>
                ,@EmptyGuid --<LastModifiedSubjectId, uniqueidentifier,>
                )
        END
        ELSE
        BEGIN
            UPDATE [dbo].[Permission]
                SET [Description] = @Description
            WHERE [Name] = @PermissionName and PolicyId = @PolicyId
        END

        SET @PermissionId = (SELECT PermissionId FROM dbo.[Permission] WHERE [Name] = @PermissionName and PolicyId = @PolicyId);

        -- assign to role, if necessary
        IF NOT Exists(Select * from dbo.[RolePermission] where [PermissionId] = @PermissionId and RoleId = @RoleId)
        BEGIN
            INSERT INTO [dbo].[RolePermission]
                ([RoleId]
                ,[PermissionId]
                ,[CreatedDate]
                ,[CreatedSubjectId]
                ,[LastModifiedDate]
                ,[LastModifiedSubjectId])
            VALUES
                (@RoleId --<RoleId, int,>
                ,@PermissionId --<PermissionId, int,>
                ,getutcdate() --<CreatedDate, datetime2(7),>
                ,@EmptyGuid --<CreatedSubjectId, uniqueidentifier,>
                ,getutcdate() --<LastModifiedDate, datetime2(7),>
                ,@EmptyGuid --<LastModifiedSubjectId, uniqueidentifier,>
                )
        END

        -- Retrieve the id
        SELECT @PermissionId;
    END TRY
    BEGIN CATCH
        -- Handle errors
        DECLARE @ErrorMessage NVARCHAR(4000);
        DECLARE @ErrorSeverity INT;
        DECLARE @ErrorState INT;

        SELECT
            @ErrorMessage = ERROR_MESSAGE(),
            @ErrorSeverity = ERROR_SEVERITY(),
            @ErrorState = ERROR_STATE();

        RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END;

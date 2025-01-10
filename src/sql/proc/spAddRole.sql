CREATE OR ALTER PROCEDURE spAddRole
    @PolicyName varchar(100),
    @RoleName varchar(100),
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

        DECLARE @EmptyGuid UniqueIdentifier = '00000000-0000-0000-0000-000000000000'

        DECLARE @PolicyId int = (SELECT PolicyId FROM dbo.[Policy] WHERE [Name] = @PolicyName);

        -- upsert
        IF NOT Exists(Select * from dbo.[Role] where [Name] = @RoleName and PolicyId = @PolicyId)
        BEGIN
            INSERT INTO [dbo].[Role]
                ([Name]
                ,[Description]
                ,[PolicyId]
                ,[CreatedDate]
                ,[CreatedSubjectId]
                ,[LastModifiedDate]
                ,[LastModifiedSubjectId])
            VALUES
                (@RoleName --<Name, nvarchar(100),>
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
            UPDATE [dbo].[Role]
                SET [Description] = @Description
            WHERE [Name] = @RoleName and PolicyId = @PolicyId
        END

        -- Retrieve the id
        SELECT RoleId FROM dbo.[Role] WHERE [Name] = @RoleName and PolicyId = @PolicyId;
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

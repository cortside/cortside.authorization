CREATE OR ALTER PROCEDURE spAddClaimTypeToRole
    @PolicyName varchar(100),
    @RoleName varchar(100),
    @ClaimType varchar(100),
    @ClaimValue varchar(500),
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

        IF @ClaimType IS NULL
        BEGIN
            RAISERROR('Invalid ClaimType', 16, 1);
            RETURN;
        END

        IF @ClaimValue IS NULL
        BEGIN
            RAISERROR('Invalid ClaimValue', 16, 1);
            RETURN;
        END

        DECLARE @EmptyGuid UniqueIdentifier = '00000000-0000-0000-0000-000000000000'

        DECLARE @PolicyId int = (SELECT PolicyId FROM dbo.[Policy] WHERE [Name] = @PolicyName);
        DECLARE @RoleId int = (SELECT RoleId FROM dbo.[Role] WHERE [Name] = @RoleName and PolicyId = @PolicyId);

        -- upsert
        IF NOT Exists(Select * from dbo.[PolicyRoleClaim] where RoleId = @RoleId and ClaimType = @ClaimType and [Value] = @ClaimValue)
        BEGIN
            INSERT INTO [dbo].[PolicyRoleClaim]
                ([ClaimType]
                ,[Value]
                ,[Description]
                ,[RoleId]
                ,[CreatedDate]
                ,[CreatedSubjectId]
                ,[LastModifiedDate]
                ,[LastModifiedSubjectId])
            VALUES
                (@ClaimType --<ClaimType, nvarchar(100),>
                ,@ClaimValue --<Value, nvarchar(500),>
                ,@Description --<Description, nvarchar(255),>
                ,@RoleId --<RoleId, int,>
                ,getutcdate() --<CreatedDate, datetime2(7),>
                ,@EmptyGuid --<CreatedSubjectId, uniqueidentifier,>
                ,getutcdate() --<LastModifiedDate, datetime2(7),>
                ,@EmptyGuid --<LastModifiedSubjectId, uniqueidentifier,>
                )
        END
        ELSE
        BEGIN
            UPDATE [dbo].[PolicyRoleClaim]
                SET [Description] = @Description
            WHERE RoleId = @RoleId and ClaimType = @ClaimType and [Value] = @ClaimValue
        END


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

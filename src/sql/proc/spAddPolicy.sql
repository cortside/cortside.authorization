CREATE OR ALTER PROCEDURE spAddPolicy
    @PolicyName varchar(100),
    @Description varchar(255)
AS
BEGIN
    -- Error handling
    BEGIN TRY
        -- Check if the PolicyName is valid
        IF @PolicyName IS NULL
        BEGIN
            RAISERROR('Invalid PolicyName', 16, 1);
            RETURN;
        END

        DECLARE @EmptyGuid UniqueIdentifier = '00000000-0000-0000-0000-000000000000'

        -- upsert
        IF NOT Exists(Select * from dbo.[Policy] where [Name] = @PolicyName)
        BEGIN
            INSERT INTO [dbo].[Policy]
                ([Name]
                ,[Description]
                ,[CreatedDate]
                ,[CreatedSubjectId]
                ,[LastModifiedDate]
                ,[LastModifiedSubjectId])
            VALUES
                (@PolicyName --<Name, nvarchar(100),>
                ,@Description --<Description, nvarchar(255),>
                ,getutcdate() --<CreatedDate, datetime2(7),>
                ,@EmptyGuid --<CreatedSubjectId, uniqueidentifier,>
                ,getutcdate() --<LastModifiedDate, datetime2(7),>
                ,@EmptyGuid --<LastModifiedSubjectId, uniqueidentifier,>
                )
        END
        ELSE
        BEGIN
            UPDATE [dbo].[Policy]
                SET [Description] = @Description
                    -- LastModifiedDate = getutcdate(),
                    -- LastModifiedSubjectId = @EmptyGuid
            WHERE [Name] = @PolicyName
        END

        -- Retrieve the id
        SELECT PolicyId FROM dbo.[Policy] WHERE [Name] = @PolicyName;
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

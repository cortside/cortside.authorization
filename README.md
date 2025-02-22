# Cortside.Authorization

This project was created from the [cortside-api template](https://github.com/cortside/cortside.templates).

It contains a .Net WebApi and a [RestApiClient](https://github.com/cortside/cortside.restapiclient/tree/develop)

- WebApi [persists](./src/sql/data/001-catalog-policy.data.sql) and serves Roles and Permissions based on configurable ClaimsPrincipal Claims (key and value)
- [Cortside.Authorization.Client](./src/Cortside.Authorization.Client/README.md) can be registered in other WebApis for real-time RBAC (roll-based access control)

## TODO

- Admin policy data
- CRUD endpoints

## Pre-Requisites

- .NET 8.0
- Visual Studio 2022

## Visual Studio Extensions

Here are the Visual Studio Extensions we use for this project:

- EditorConfig Language Service
- Format document on Save
- SonarLint for Visual Studio
- Extension Manager
- Code Cleanup On Save
- MappingGenerator -- https://mappinggenerator.net/

## Database location

You can override the default expected location of the database using environment variables. The easiest way to set these is to set them up in your powershell profile (`notepad $PROFILE`):

```powershell
$env:MSSQL_SERVER="kehlstein"
$env:MSSQL_USER="sa"
$env:MSSQL_PASSWORD="password1@"
```

The default will be to use Sql Express if `$env:MSSQL_SERVER` is not set. The default to be to use logged in user with trusted connection if `$env:MSSQL_USER` is not set. The above example shows how to use a remote host with sql authentication enabled.

## Create database locally

Run powershell script `.\update-database.ps1 -CreateDatabase`

## Update existing local database

Run powershell script `.\update-database.ps1`

## Updating the database schema through migrations

### Add a new migration

- In Cortside.Authorization.Data, make changes to the database models to match the desired schema.
- To generate a new migration, run: `add-migration.ps1` and supply a name for the migration when prompted
  - also runs `Generate-Sql.ps1` and `Generate-SqlTriggers.ps1`
- If needed, edit and add to the generated C# migration file.
- If needed, run the `Generate-Sql.ps1` script to generate the SQL file.
- If needed, run the `Generate-SqlTriggers.ps1` script.
- Run the `update-database.ps1` script to update your local instance.

### Edit an existing migration

The way EntityFramework migrations work, particularly how we use them, is that editing a migration **after** it has already run in a database, will accomplish nothing. If you read the generated sql files that are run by `update-database.ps1`, you will see why. The deployment process does not ever run something like `dotnet ef down --target-migration some_previous_migration`.

That means editing existing migrations can really only be done sensibly in a branch that has not yet been merged **and preferably not yet deployed to any integrated environment like dev or stage** (or there will likely be future issues to deal with there). But know that you will need to manually delete the record in your local database's `__EFMigrationsHistory` table to run the updated script against your local db, one way or another. And undo what it did, one way or another. Deleting your local db and running `update-database.ps1 -CreateDatabase` is a viable option.

In other words, **if your branch has not yet merged AND has not yet deployed to dev/test/stage/uat/etc** then:

- delete your local `Authorization` database
- delete the generated sql migration file
- update the cs migration _or_ delete the existing cs migration and add a fresh one
- repeat the remaining steps for adding a migration

`*`**NOTE:** This is one way and it's the big hammer way. Combining with the details above of migrations work, we can also rollback to a previous version, and then undo the migration changes (files, entities, etc).

## Deployment of Database changes

Database changes are deployed to shared environments when Octopus runs `update-database.ps1` against the database for the given environment being deployed to.

## Running the application locally

- `run.ps1` is a convenience script, if debugging in VisualStudio is not needed
- you may need to run `update-database.ps1` to update your local database (do not run against SqlTestOnlineApp, as deployments should do that)

# Automation

# Major Milestones

|Name|Complete|
|---|---|
| &#9745; | Initial Creation |
| &#9745; | RBAC Authorization |
| &#9744; | Create script |
| &#9744; | Run job |
| &#9744; | Import modules |
| &#9744; | Store variables |
| &#9744; | Store credentials |
| &#9744; | Create automations |
| &#9744; | Create webhooks |
| &#9744; | User interface |


# Major Components

## EphIt.Blazor

This is the user interface project. It is a Blazor Web Assembly project with a server API back end of EphIt.Server. Lowest priority so it just exists.

## EphIt.Server

Web API / Blazor server component. API endpoints should be prefixed with /api (IE, /api/Script). 

## EphIt.Service

Windows service to run the scripts.

## EphIt.SQL

SQL project with DB creation scripts. 

# Deploying code

Open the project in Visual Studio and publish the EphIt.SQL project to a Microsoft SQL server instance. Set EphIt.Server as the startup project and then update appsettings.json with the new connection string for the DB. 

On startup, the application will add the current user as a full admin so then you should have access to everything.
# Class library Standards

## Names:

* BL
    * BL means business logic. All business logic should be run by either the windows service or the API. No business logic should specifically be run in the Blazor project or any future PowerShell module as this could theoretically be modified by the end user.
    * Example project name:
        * EphIt.BL.Script
            * Business logic for creating scripts, script versions, and publishing scripts.
* UI
    * UI means user interaction and isn't specifically a graphical UI. Any projects prefaced with UI will not directly interact with the database or have any business logic in them. These projects will be used in the Blazor app and in a future PowerShell module, so dependencies should be set accordingly (.netstandard 2.0 and no EF code).
    * Example project name:
        * EphIt.UI.Script
            * Logic to interact with the /api/Script endpoint on the web service.


## Special Projects
* EphIt.Db.Models
    * The database models live in this project without any Entity Framework dependencies so the same models can be used in UI and BL projects. 
    * Parameter models also exist here - for instance POST models for the POST body.
        * TO DO: Looking at refactoring the name to EphIt.Models as it has more than just DB models.
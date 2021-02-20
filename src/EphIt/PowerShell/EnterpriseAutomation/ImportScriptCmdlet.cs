using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Management.Automation;
using EphIt.Db.Models;
using System.Linq;
using EphIt.BL.Automation;
using System.IO;

namespace EnterpriseAutomation
{
    [Cmdlet(VerbsData.Import, "EAScript")]
    [OutputType(typeof(VMScript))]
    public class ImportScript : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public ImportScript() { }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Name of the script."
        )]
        public string Name { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Script Description"
        )]
        public string Description { get; set; }
        
        [Parameter(
            Mandatory = true,
            ParameterSetName = "Body",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Script body."
        )]
        public string Body { get; set; }

        [Parameter(
            Mandatory = true,
            ParameterSetName = "File",
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Script File."
        )]
        public string ScriptFile { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 3,
            HelpMessage = "Language of the script."
        )]
        [ValidateSet("PowerShellCore")]
        public string Language { get; set; } = "PowerShellCore";

        public object GetDynamicParameters()
        {
            return DynamicParameters.dynParams(ref _staticStorage);
        }
        protected override void BeginProcessing()
        {
            base.BeginProcessing();
            automationHelper = SetupAutomationHelper.Setup(_staticStorage);
        }
        protected override void ProcessRecord()
        {
            base.ProcessRecord();
            if(ParameterSetName.Equals("File")) {
                if(File.Exists(ScriptFile))
                {
                    Body = File.ReadAllText(ScriptFile);
                }
            }
            Collection<PSParseError> parseErrors = new Collection<PSParseError>();
            PSParser.Tokenize(Body, out parseErrors);
            if(parseErrors.Count > 0) {
                var exception = new Exception("Script parse error");
                ErrorRecord error = new ErrorRecord(exception,"1", ErrorCategory.InvalidData,null);
                WriteError(error);
                return;
            }
            string url = automationHelper.GetUrl();
            url = url + "/api/Script";

            WriteVerbose($"Using URL: {url}");
            //does it already exist?
            List<VMScript> result = automationHelper.GetWebCall<List<VMScript>>(url + $"?Name={Name}");
            bool exists = result.Any(r => r.Name.Equals(Name));
            string scriptID = null;
            if(!exists)
            {
                //create new
                ScriptPostParameters postParams = new ScriptPostParameters();
                postParams.Description = Description;
                postParams.Name = Name;
                scriptID = automationHelper.PostWebCall(url, postParams);
            }
            else {
                scriptID = result.Select(r => r.ScriptId).FirstOrDefault().ToString();
            }

            //create version
            if (!string.IsNullOrEmpty(Body) && scriptID != null)
            {
                url = automationHelper.GetUrl() + $"/api/Script/{scriptID}/Version";
                ScriptVersionPostParameters verPostParms = new ScriptVersionPostParameters();
                verPostParms.ScriptBody = Body;
                short lang = 0;
                switch (Language)
                {
                    case "PowerShellCore": 
                        lang = (short)ScriptLanguageEnum.PowerShellCore;
                        break;
                }
                verPostParms.ScriptLanguageId = lang; 
                string versionID = automationHelper.PostWebCall(url, verPostParms);
            }

            //return script object
            url = automationHelper.GetUrl() + $"/api/Script/{scriptID}";
            WriteObject(automationHelper.GetWebCall<VMScript>(url));
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

using System;
using System.Management.Automation;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using EphIt.Db.Models;
using EphIt.BL.Automation;

namespace Automation
{
    [Cmdlet(VerbsCommon.New, "AutomationScript")]
    [OutputType(typeof(VMScript))]
    public class NewScriptCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public NewScriptCmdlet() 
        {
            automationHelper = new AutomationHelper();        
        }
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Script Name"
        )]
        public string Name { get; set; }
        
        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Script Description"
        )]
        public string Description { get; set; }
        
        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Script Body"
        )]
        public string Body { get; set; }

        public object GetDynamicParameters()
        {
            return DynamicParameters.dynParams(ref _staticStorage);
        }
        protected override void BeginProcessing()
        {
            string server = (string)_staticStorage.Values.Where(v => v.Name.Equals("Server")).Select(s => s.Value).FirstOrDefault();
            int port = (int)_staticStorage.Values.Where(v => v.Name.Equals("Port")).Select(s => s.Value).FirstOrDefault();
            automationHelper.SetPort(port);
            automationHelper.SetServer(server);
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            string url = automationHelper.GetUrl();
            url = url + "/api/Script";

            WriteVerbose($"Using URL :{url}");
            //does it already exist?
            List<VMScript> result = automationHelper.GetWebCall<List<VMScript>>(url + $"?Name={Name}");
            bool exists = result.Any(r => r.Name.Equals(Name));
            if (exists)
            {
                WriteVerbose($"Script already exists.");
                return;
            }

            //create new
            ScriptPostParameters postParams = new ScriptPostParameters();
            postParams.Description = Description;
            postParams.Name = Name;
            string scriptID = automationHelper.PostWebCall(url, postParams);
            
            //create version
            if(!string.IsNullOrEmpty(Body))
            {
                url = automationHelper.GetUrl() + $"/api/Script/{scriptID}/Version";
                ScriptVersionPostParameters verPostParms = new ScriptVersionPostParameters();
                verPostParms.ScriptBody = Body;
                verPostParms.ScriptLanguageId = 2; //this needs to be dynamic someday
                string versionID = automationHelper.PostWebCall(url, verPostParms);
                WriteVerbose($"Version ID: {versionID}");
            }

            //return script object
            url = automationHelper.GetUrl() + $"/api/Script/{scriptID}";
            WriteObject(automationHelper.GetWebCall<VMScript>(url));
            
            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

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

namespace Automation
{
    [Cmdlet(VerbsCommon.New, "AutomationScript")]
    [OutputType(typeof(VMScript))]
    public class NewScriptCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private string scriptName;
        private string scriptBody;
        private string scriptDescription;
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
        public string Name
        {
            get { return scriptName; }
            set { scriptName = value; }
        }
        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Script Description"
        )]
        public string Description
        {
            get { return scriptDescription; }
            set { scriptDescription = value; }
        }
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 2,
            HelpMessage = "Script Body"
        )]
        public string Body
        {
            get { return scriptBody; }
            set { scriptBody = value; }
        }

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
            List<VMScript> result = automationHelper.GetWebCall<List<VMScript>>(url + $"?Name={scriptName}");
            bool exists = result.Any(r => r.Name.Equals(scriptName));
            if (exists)
            {
                WriteVerbose($"Script already exists.");
                return;
            }

            //create new
            ScriptPostParameters postParams = new ScriptPostParameters();
            postParams.Description = scriptDescription;
            postParams.Name = scriptName;
            string scriptID = automationHelper.PostWebCall(url, postParams);
            
            //create version
            url = automationHelper.GetUrl() + $"/api/Script/{scriptID}/Version";
            ScriptVersionPostParameters verPostParms = new ScriptVersionPostParameters();
            verPostParms.ScriptBody = scriptBody;
            verPostParms.ScriptLanguageId = 2; //this needs to be dynamic someday
            string versionID = automationHelper.PostWebCall(url, verPostParms);

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

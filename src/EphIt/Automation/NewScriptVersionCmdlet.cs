using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using EphIt.Db.Models;
using System.Linq;
using EphIt.BL.Automation;

namespace Automation
{
    [Cmdlet(VerbsCommon.New, "AutomationScriptVersion")]
    [OutputType(typeof(VMScriptVersion))]
    public class NewScriptVersionCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public NewScriptVersionCmdlet()
        {
            automationHelper = new AutomationHelper();
        }
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Script ID"
        )]
        public string ID { get; set; }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
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
            url = url + $"/api/Script/{ID}";

            //does the script exist?
            VMScript result = automationHelper.GetWebCall<VMScript>(url);
            if (result == null)
            {
                WriteVerbose($"Script does not exists.");
                return;
            }

            url = automationHelper.GetUrl() + $"/api/Script/{ID}/Version";
            ScriptVersionPostParameters verPostParms = new ScriptVersionPostParameters();
            verPostParms.ScriptBody = Body;
            verPostParms.ScriptLanguageId = 2; //this needs to be dynamic someday
            string versionID = automationHelper.PostWebCall(url, verPostParms);
            WriteVerbose($"Version ID: {versionID}");

            url = automationHelper.GetUrl() + $"/api/Script/{ID}/Version?IncludeAll=True";
            List<VMScriptVersion> newVersion = automationHelper.GetWebCall<List<VMScriptVersion>>(url);
            WriteObject(newVersion.Where(v => v.ScriptVersionId.ToString().Equals(versionID)).FirstOrDefault());
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

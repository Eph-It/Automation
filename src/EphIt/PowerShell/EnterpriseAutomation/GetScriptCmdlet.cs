using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using EphIt.Db.Models;
using System.Linq;
using Newtonsoft.Json;
using EphIt.BL.Automation;
using System.Collections;

namespace EnterpriseAutomation
{

    [Cmdlet(VerbsCommon.Get, "EAScript")]
    [OutputType(typeof(VMScript))]
    public class GetScriptCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = false,
            Position = 0,
            HelpMessage = "Script Version ID"
        )]
        public int ScriptId { get; set; }
        public GetScriptCmdlet()
        {
            automationHelper = new AutomationHelper();
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
            string url = automationHelper.GetUrl() + $"/api/Script/{ScriptId}";
            var response = automationHelper.GetWebCall<VMScript>(url);
            WriteObject(response);

            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

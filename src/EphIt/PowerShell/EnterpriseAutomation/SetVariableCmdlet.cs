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

namespace EnterpriseAutomation
{
    [Cmdlet(VerbsCommon.Set, "EAVariable")]
    [OutputType(typeof(VMVariable))]
    public class SetVariableCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public SetVariableCmdlet()
        {
            automationHelper = new AutomationHelper();
        }
        [Parameter(
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
            HelpMessage = "Variable Name"
        )]
        public string Name { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Variable Value"
        )]
        public object Value { get; set; }

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
            url = url + "/api/Variable";

            WriteVerbose($"Using URL :{url}");
            
            //create new or update
            url = url + $"?name={Name}&value={Value}";
            var result = automationHelper.PostWebCall<VMVariable>(url, null);
            WriteObject(result);
            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

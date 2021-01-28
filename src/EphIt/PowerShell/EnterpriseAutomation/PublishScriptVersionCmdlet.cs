using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using EphIt.Db.Models;
using System.Linq;
using EphIt.BL.Automation;

namespace EnterpriseAutomation
{
    [Cmdlet(VerbsData.Publish, "EAScript")]
    [OutputType(typeof(VMScript))]
    public class PublishScriptVersion : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public PublishScriptVersion()
        {
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
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = true,
            Position = 1,
            HelpMessage = "Script Version ID, if null the latest version is published."
        )]
        public int? VersionID { get; set; } = null;

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
            string url = automationHelper.GetUrl();
            if(VersionID.HasValue) {
                url = url + $"/api/Script/{ID}/Publish/{VersionID}";
            }
            else {
                url = url + $"/api/Script/{ID}/Publish";
            }
            
            WriteObject(automationHelper.PostWebCall<VMScript>(url));
            base.ProcessRecord();
        }

        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

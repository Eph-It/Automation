using System;
using System.Collections.Generic;
using System.Text;
using System.Management.Automation;
using EphIt.Db.Models;
using System.Linq;
using Newtonsoft.Json;
using EphIt.BL.Automation;
using System.Collections;

namespace Automation
{
    [Cmdlet(VerbsCommon.Get, "AutomationVariable")]
    [OutputType(typeof(object))]
    public class GetVariableCmdlet : PSCmdlet
    {
        private IAutomationHelper automationHelper;

        public GetVariableCmdlet()
        {
            automationHelper = new AutomationHelper();
        }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 0,
            HelpMessage = "Variable Name"
        )]
        public string Name{ get; set; }

        [Parameter(
            Mandatory = true,
            DontShow = true,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 1,
            HelpMessage = "Server Name, this parameter is hidden and set automatically at runtime."
        )]
        public string AutomationServerName { get; set; }

        [Parameter(
            Mandatory = true,
            DontShow = true,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 2,
            HelpMessage = "Server Port, this parameter is hidden and set automatically at runtime."
        )]
        public int AutomationServerPort { get; set; }
        protected override void BeginProcessing()
        {
            automationHelper.SetPort(AutomationServerPort);
            automationHelper.SetServer(AutomationServerName);
            base.BeginProcessing();
        }
        protected override void ProcessRecord()
        {
            string url = automationHelper.GetUrl() + $"/api/Variable/{Name}";
            Variable response = automationHelper.GetWebCall<Variable>(url);
            if (response != null)
            {
                WriteObject(response.Value);
            }
            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

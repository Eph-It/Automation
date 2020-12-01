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
    [Cmdlet(VerbsCommon.New, "EAJob")]
    [OutputType(typeof(VMJob))]
    public class NewJobCmdlet : PSCmdlet, IDynamicParameters
    {
        private static RuntimeDefinedParameterDictionary _staticStorage;
        private IAutomationHelper automationHelper;

        public NewJobCmdlet()
        {
            automationHelper = new AutomationHelper();
        }

        [Parameter(
            Mandatory = true,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 0,
            HelpMessage = "Script Version ID"
        )]
        public int ScriptVersionId { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 1,
            HelpMessage = "Automation ID"
        )]
        public int AutomationID { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 2,
            HelpMessage = "Schedule ID"
        )]
        public int ScheduleID { get; set; }

        [Parameter(
            Mandatory = false,
            ValueFromPipeline = false,
            ValueFromPipelineByPropertyName = false,
            Position = 3,
            HelpMessage = "Job script parameters."
        )]
        public Hashtable Parameters { get; set; }
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
            string url = automationHelper.GetUrl() + "/api/Job/";
            JobPostParameters jobPostParameters = new JobPostParameters();
            jobPostParameters.AutomationID = AutomationID;
            jobPostParameters.ScheduleID = ScheduleID;
            jobPostParameters.ScriptVersionID = ScriptVersionId;
            jobPostParameters.Parameters = JsonConvert.SerializeObject(Parameters);
            string response = automationHelper.PostWebCall(url, jobPostParameters);
            if (!string.IsNullOrEmpty(response))
            {
                var guidResponse = JsonConvert.DeserializeObject<Guid>(response);
                WriteObject(guidResponse);
            }
            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

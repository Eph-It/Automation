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
using Simple.OData.Client;
using EphIt.UI;

namespace EnterpriseAutomation
{
    [Cmdlet(VerbsCommon.New, "EAScript")]
    [OutputType(typeof(Script))]
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
        private string _server;
        private int _port;
        protected override void BeginProcessing()
        {
            _server = (string)_staticStorage.Values.Where(v => v.Name.Equals("Server")).Select(s => s.Value).FirstOrDefault();
            _port = (int)_staticStorage.Values.Where(v => v.Name.Equals("Port")).Select(s => s.Value).FirstOrDefault();
            base.BeginProcessing();
        }
        protected async override void ProcessRecord()
        {
            var uri = $"https://{_server}:{_port}";
            var eaOdata = new EAOdata(uri);
            var oData = eaOdata.GetClient();
            var entries = await oData.For<Script>().Filter(p => p.Name == Name).FindEntriesAsync();
            if (entries.Any())
            {
                WriteVerbose("Script already exists");
                return;
            }
            var createdScript = await oData.For<Script>()
                .Set(new { 
                    Name = Name,
                    Description = Description
                }).InsertEntryAsync();


            var createdVersion = await oData.For<ScriptVersion>()
                .Set(new
                {
                    Body = Body,
                    ScriptLanguageId = 2,
                    ScriptId = createdScript.ScriptId
                }).InsertEntryAsync();

            //WriteObject(createdScript);
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
    }
}

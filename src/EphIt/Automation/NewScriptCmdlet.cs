using System;
using System.Management.Automation;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Automation
{
    [Cmdlet(VerbsCommon.New, "AutomationScript")]
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
            Mandatory = true,
            ValueFromPipeline = true,
            ValueFromPipelineByPropertyName = true,
            Position = 0,
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
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            WriteVerbose($"Using URL :{url}");
            ScriptPostParameters postParams = new ScriptPostParameters();
            postParams.Description = scriptDescription;
            postParams.Name = scriptName;
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            var postJson = JsonConvert.SerializeObject(postParams);
            var bytes = Encoding.ASCII.GetBytes(postJson);
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            Console.WriteLine(responseFromServer);
            // Cleanup the streams and the response.
            reader.Close();
            dataStream.Close();
            response.Close();
            base.ProcessRecord();
        }
        protected override void EndProcessing()
        {
            base.EndProcessing();
        }
        public class ScriptPostParameters
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public int? Published_Version { get; set; }
        }
    }
}

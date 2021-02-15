using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using EphIt.Db.Enums;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EphIt.BL.Automation
{
    public class AutomationHelper : IAutomationHelper
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string AutomationModulePath { get; set; }
        public int GetQueuedJobDelay { get; set; }
        public string TempDirectory {get; set;}
        public AutomationHelper() { }
        public AutomationHelper(IConfiguration configuration)
        {
            SetServer(configuration.GetSection("ServerInfo:WebServer").Value);
            SetPort(Int32.Parse(configuration.GetSection("ServerInfo:Port").Value));
            SetAutomationModulePath(configuration.GetSection("ServerInfo:AutomationModulePath").Value);
            SetGetQueuedJobDelay(Int32.Parse(configuration.GetSection("ServerInfo:GetQueuedJobDelay").Value));
            SetTempDirectory(configuration.GetSection("ServerInfo:TempDirectory").Value);
        }
        public string GetUrl()
        {
            if(!Server.StartsWith("https://"))
            {
                Server = "https://" + Server;
            }
            return Server + ":" + Port;
        }
        public string GetServer()
        {
            return Server;
        }
        public string GetAutomationModulePath()
        {
            return AutomationModulePath;
        }
        public int GetGetQueuedJobDelay()
        {
            return GetQueuedJobDelay;
        }
        public string GetTempDirectory() {
            return TempDirectory;
        }
        public void SetServer(string name)
        {
            Server = name;
        }
        public void SetPort(int port)
        {
            Port = port;
        }
        public void SetAutomationModulePath(string path)
        {
            AutomationModulePath = path;
        }
        public void SetGetQueuedJobDelay(int value)
        {
            GetQueuedJobDelay = value;
        }
        public void SetTempDirectory(string value) {
            if(string.IsNullOrEmpty(value)) {
                TempDirectory = System.Environment.GetEnvironmentVariable("TEMP");
            }
            else {
                TempDirectory = value;
            }
        }
        public int GetPort()
        {
            return Port;
        }
        public string PostWebCall(string url, object body = null)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            if(body != null) {
                Stream requestStream = request.GetRequestStream();
                var postJson = JsonConvert.SerializeObject(body);
                var bytes = Encoding.ASCII.GetBytes(postJson);
                requestStream.Write(bytes, 0, bytes.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        public string GetWebCall(string url)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";
            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseFromServer = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            response.Close();
            return responseFromServer;
        }
        public T GetWebCall<T>(string url)
        {
            string response = GetWebCall(url);
            return JsonConvert.DeserializeObject<T>(response);
        }
        public T PostWebCall<T>(string url, object body = null)
        {
            string response = PostWebCall(url, body);
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
    public class ScriptPostParameters
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? Published_Version { get; set; }
    }
    public class ScriptVersionPostParameters
    {
        public string ScriptBody { get; set; }
        public short ScriptLanguageId { get; set; }
    }
    public class JobPostParameters
    {
        public int ScriptVersionID { get; set; }
        public int? ScheduleID { get; set; }
        public int? AutomationID { get; set; } //maybe should be guid?
        //maybe add a runbook server param later?
        public string Parameters { get; set; }
    }
    public class JobOutputPostParameters
    {
        public Guid JobUid { get; set; }
        public string Type { get; set; }
        public string JsonValue { get; set; }
        public byte[] ByteArrayValue { get; set; }
    }
    public class LogPostParameters
    {
        public Guid jobUid { get; set; }
        public string message { get; set; }
        public JobLogLevelEnum level { get; set; }
        public string Exception { get; set; }
    }
}

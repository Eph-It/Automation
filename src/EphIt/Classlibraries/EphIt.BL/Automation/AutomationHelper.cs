using EphIt.Db.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
//using System.Security.Policy;
using System.Text;

namespace EphIt.BL.Automation
{
    public class AutomationHelper : IAutomationHelper
    {
        public string Server { get; set; }
        public int Port { get; set; }
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
        public void SetServer(string name)
        {
            Server = name;
        }
        public void SetPort(int port)
        {
            Port = port;
        }
        public int GetPort()
        {
            return Port;
        }
        public string PostWebCall(string url, object body)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";

            request.Credentials = CredentialCache.DefaultCredentials;
            request.ContentType = "application/json";
            Stream requestStream = request.GetRequestStream();
            var postJson = JsonConvert.SerializeObject(body);
            var bytes = Encoding.ASCII.GetBytes(postJson);
            requestStream.Write(bytes, 0, bytes.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            // Display the status.
            // Console.WriteLine(response.StatusDescription);
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Display the content.
            // Cleanup the streams and the response.
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
            // Display the status.
            // Get the stream containing content returned by the server.
            Stream dataStream = response.GetResponseStream();
            // Open the stream using a StreamReader for easy access.
            StreamReader reader = new StreamReader(dataStream);
            // Read the content.
            string responseFromServer = reader.ReadToEnd();
            // Cleanup the streams and the response.
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
    }
}

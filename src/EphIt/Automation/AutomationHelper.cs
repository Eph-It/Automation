using System;
using System.Collections.Generic;
using System.Text;

namespace Automation
{
    class AutomationHelper : IAutomationHelper
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
    }
}

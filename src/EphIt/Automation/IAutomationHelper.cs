using System;
using System.Collections.Generic;
using System.Text;

namespace Automation
{
    interface IAutomationHelper
    {
        string GetUrl();
        void SetServer(string name);
        string GetServer();
        void SetPort(int port);
        int GetPort();
        string PostWebCall(string url, object body);
        string GetWebCall(string url);
        T GetWebCall<T>(string url);
    }
}

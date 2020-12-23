using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.BL.Automation
{
    public interface IAutomationHelper
    {
        string GetUrl();
        void SetServer(string name);
        string GetServer();
        string GetAutomationModulePath();
        void SetPort(int port);
        int GetPort();
        string PostWebCall(string url, object body);
        string GetWebCall(string url);
        T GetWebCall<T>(string url);
    }
}

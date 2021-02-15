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
        int GetGetQueuedJobDelay();
        string GetTempDirectory();        
        void SetGetQueuedJobDelay(int value);
        void SetTempDirectory(string value);
        void SetPort(int port);
        int GetPort();
        string PostWebCall(string url, object body = null);
        T PostWebCall<T>(string url, object body = null);
        string GetWebCall(string url);
        T GetWebCall<T>(string url);
    }
}

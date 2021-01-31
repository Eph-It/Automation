using System;
using EphIt.BL.Automation;
using System.Management.Automation;
using System.Linq;

namespace EnterpriseAutomation
{
    public static class SetupAutomationHelper {
        public static AutomationHelper Setup(RuntimeDefinedParameterDictionary parameterDictionary) {
            AutomationHelper automationHelper = new AutomationHelper();
            string server = (string)parameterDictionary.Values.Where(v => v.Name.Equals("Server")).Select(s => s.Value).FirstOrDefault();
            int port = (int)parameterDictionary.Values.Where(v => v.Name.Equals("Port")).Select(s => s.Value).FirstOrDefault();
            automationHelper.SetPort(port);
            automationHelper.SetServer(server);
            return automationHelper;
        }
    }
}
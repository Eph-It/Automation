using Microsoft.VisualStudio.TestTools.UnitTesting;
using EphIt.Service.Posh;
using EphIt.Service.Posh.Job;
using Moq;
using EphIt.BL.JobManager;
using EphIt.Db.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Management.Automation;
using EphIt.BL.PowerShellModule;
using EphIt.BL.User;
using System.Threading;
using EphIt.BL.Automation;

namespace EphIt.BL.PowerShellModule.Tests
{
    [TestClass]
    public class ModuleManagerTests {
        private Mock<IAutomationHelper> _automationHelper;
        public void Setup() {
            
        }
        public ModuleManagerTests() {}
        
        [TestMethod]
        public void ShouldUnzipToTemp(){

        }

        [TestMethod]
        public void ShouldIdentityProperPowerShellModule(){}


        [TestMethod]
        public void ShouldIdentityImroperPowerShellModule(){}

    }
}
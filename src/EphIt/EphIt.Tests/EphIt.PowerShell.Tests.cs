using Microsoft.VisualStudio.TestTools.UnitTesting;
using EphIt.BL.Automation;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using EphIt.PowerShell;


namespace EphIt.PowerShell.Tests
{
    [TestClass]
    public class ModuleManagerTests {
        private IAutomationHelper automationHelper;
        
        public ModuleManagerTests() {
            Setup();
        }
        public void Setup()
        {
            automationHelper = new AutomationHelper();
            automationHelper.SetTempDirectory();
        }

        public byte[] CreateModule()
        {
            List<string> psm1BodyStrings = new List<string>();
            psm1BodyStrings.Add("Function Get-Widget{}; Export-ModuleMember 'Get-Widget'");
            string psm1Path = "tmpModule\\tmpModule.psm1";
            Directory.CreateDirectory("tmpModule");
            File.Create(psm1Path).Dispose();
            File.WriteAllLines(psm1Path, psm1BodyStrings);
            if (File.Exists("tmpModule.zip"))
            {
                File.Delete("tmpModule.zip");
            }
            ZipFile.CreateFromDirectory("tmpModule", "tmpModule.zip");
            Directory.Delete("tmpModule", true);
            return File.ReadAllBytes("tmpModule.zip");
        }
        [TestMethod]
        public void ShouldUnzipToTemp(){
            ModuleManager mm = new ModuleManager(automationHelper);
            string tmpDestination = mm.UnzipModule(CreateModule());
            Assert.IsTrue(Directory.Exists(tmpDestination));
            File.Delete("tmpModule.zip");
            Directory.Delete(tmpDestination,true);
        }

        [TestMethod]
        public void ShouldIdentityProperPowerShellModule(){}


        [TestMethod]
        public void ShouldIdentityImroperPowerShellModule(){}

    }
}
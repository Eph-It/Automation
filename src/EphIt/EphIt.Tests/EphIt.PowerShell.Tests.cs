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

        public byte[] CreateModule(string moduleName)
        {
            List<string> psm1BodyStrings = new List<string>();
            psm1BodyStrings.Add("Function Get-Widget{}; Export-ModuleMember 'Get-Widget'");
            string psm1Path = "tmpModule\\" + moduleName + ".psm1";
            Directory.CreateDirectory(moduleName);
            File.Create(psm1Path).Dispose();
            File.WriteAllLines(psm1Path, psm1BodyStrings);
            if (File.Exists(moduleName + ".zip"))
            {
                File.Delete(moduleName + ".zip");
            }
            ZipFile.CreateFromDirectory(moduleName, moduleName + ".zip");
            Directory.Delete(moduleName, true);
            return File.ReadAllBytes(moduleName + ".zip");
        }
        
        [TestMethod]
        public void ShouldUnzipToTemp(){
            ModuleManager mm = new ModuleManager(automationHelper);
            string tmpDestination = mm.UnzipModule(CreateModule("tmpModule"), "tmpModule");
            Assert.IsTrue(Directory.Exists(tmpDestination));
            File.Delete("tmpModule.zip");
            Directory.Delete(tmpDestination,true);
        }

        [TestMethod]
        public void ShouldValidateProperPowerShellModule(){
            ModuleManager mm = new ModuleManager(automationHelper);
            string tmpDestination = mm.UnzipModule(CreateModule("tmpModule"), "tmpModule");
            var success = mm.ValidatePowerShellModule(tmpDestination, "tmpModule");
            Assert.IsTrue(success);
        }

        [TestMethod]
        public void ShouldNotValidateImroperPowerShellModule(){}

    }
}
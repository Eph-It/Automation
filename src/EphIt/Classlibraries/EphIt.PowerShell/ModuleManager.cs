using EphIt.Db.Models;
using System.IO.Compression;
using System.IO;
using EphIt.BL.Automation;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation.Runspaces;

namespace EphIt.PowerShell
{
    public class ModuleManager : IModuleManager
    {
        private string tempDirectory;
        public ModuleManager(IAutomationHelper automationHelper)
        {
            tempDirectory = automationHelper.GetTempDirectory();
        }
        public VMModule NewModule(byte[] compressedModule, string moduleName)
        {
            UnzipModule(compressedModule, moduleName);
            //validate its a valid powershell module TODO
            //get the name of the module TODO
            //save to database / update TODO
            return (new VMModule(new Module()));
        }
        public string UnzipModule(byte[] compressedModule, string moduleName)
        {
            string tmpExtractPath = tempDirectory + "\\" + moduleName;
            if (Directory.Exists(tmpExtractPath))
            {
                Directory.Delete(tmpExtractPath, true);
            }
            var zippedStream = new MemoryStream(compressedModule);
            var archive = new ZipArchive(zippedStream);
            archive.ExtractToDirectory(tmpExtractPath, true);
            if (Directory.Exists(tmpExtractPath))
            {
                return tmpExtractPath;
            }
            return null;
        }

        public PSModuleInfo GetPowerShellModuleInfo(string path, string name)
        {
            System.Management.Automation.PowerShell ps = System.Management.Automation.PowerShell.Create();
            Collection<PSObject> results = new Collection<PSObject>();
            ps.AddScript("Set-ExecutionPolicy Unrestricted");  //manage this better somehow
            ps.AddScript("$VerbosePreference = 'Continue'");
            ps.AddScript($"Import-Module -Name {path} -Verbose");
            ps.AddScript("Get-Module -Name " + name + " -Verbose;");
            results = ps.Invoke();
            ps.Dispose();
            foreach(var result in results) {
                if(result.TypeNames.Contains("System.Management.Automation.PSModuleInfo")) {
                    return (PSModuleInfo)result.BaseObject;
                }
            }
            return null;
        }
        public bool ValidatePowerShellModule(string path, string name) {
            //add version, etc someday if there is ever a need.
            PSModuleInfo moduleInfo = GetPowerShellModuleInfo(path, name);
            if(moduleInfo != null) {
                if(moduleInfo.Name.Equals(name)) {
                    return true;
                }
            }
            return false;
        }
    }
}
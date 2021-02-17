using EphIt.Db.Models;
using System.IO.Compression;
using System.IO;
using EphIt.BL.Automation;
using System.Management.Automation;


namespace EphIt.PowerShell
{
    public class ModuleManager : IModuleManager
    {
        private string tempDirectory;
        public ModuleManager(IAutomationHelper automationHelper)
        {
            tempDirectory = automationHelper.GetTempDirectory();
        }
        public VMModule NewModule(byte[] compressedModule)
        {
            UnzipModule(compressedModule);
            //validate its a valid powershell module TODO
            //get the name of the module TODO
            //save to database / update TODO
            return (new VMModule(new Module()));
        }
        public string UnzipModule(byte[] compressedModule)
        {
            string tmpExtractPath = tempDirectory + "\\" + System.Guid.NewGuid().ToString();
            var zippedStream = new MemoryStream(compressedModule);
            var archive = new ZipArchive(zippedStream);
            archive.ExtractToDirectory(tmpExtractPath, true);
            if (Directory.Exists(tmpExtractPath))
            {
                return tmpExtractPath;
            }
            return null;
        }

        public bool ValidatePowerShellModule(string path)
        {
        //essentially do this.
        //https://github.com/PowerShell/PowerShell/blob/master/src/System.Management.Automation/engine/Modules/TestModuleManifestCommand.cs
            return false;
        }
    }
}
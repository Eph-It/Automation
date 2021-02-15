using EphIt.Db.Models;
using System.IO.Compression;
using System.IO;
using EphIt.BL.Automation;

namespace EphIt.BL.PowerShellModule {
    public class ModuleManager : IModuleManager {
        private string tempDirectory; 
        public ModuleManager(AutomationHelper automationHelper) {
            tempDirectory = automationHelper.GetTempDirectory();
        }
        public Module NewModule (byte[] compressedModule){
            //write the zip somewhere
            var zippedStream = new MemoryStream(compressedModule);
            var archive = new ZipArchive(zippedStream);
            archive.ExtractToDirectory(tempDirectory);
            //validate its a valid powershell module
            //get the name of the module
            //save to database / update
            return new Module();
        }
    }
    
}
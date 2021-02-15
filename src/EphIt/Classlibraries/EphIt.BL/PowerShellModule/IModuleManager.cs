using EphIt.Db.Models;
using System;

namespace EphIt.BL.PowerShellModule
{
    public interface IModuleManager {
        Module NewModule(byte[] compressedModule);
    }
}
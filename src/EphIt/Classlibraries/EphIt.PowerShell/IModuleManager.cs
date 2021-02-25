using EphIt.Db.Models;
using System;

namespace EphIt.PowerShell
{
    public interface IModuleManager
    {
        VMModule NewModule(byte[] compressedModule, string moduleName);
    }
}
using EphIt.Service.Posh.Job;
using EphIt.Service.Posh;
using System.Management.Automation;

namespace EphIt.Service.Posh.Stream
{
    public interface IStreamHelper {
        void RecordStream(PoshJob poshJob, object sender, DataAddedEventArgs e);
    }
}
using EphIt.Service.Posh.Job;
using EphIt.Service.Posh;
using System.Management.Automation;

namespace EphIt.Service.Posh.Stream
{
    public interface IStreamHelper {
        public void RecordStream(PoshJob poshJob, object sender, DataAddedEventArgs e);
        public void RecordOutput(PoshJob poshJob, object sender, DataAddedEventArgs e);
    }
}
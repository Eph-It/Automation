using System.Management.Automation;
using Serilog;

namespace EphIt.Service.Posh.Stream {
    public class StreamHelper : IStreamHelper {
        public void RecordStream(PoshJob poshJob, object sender, DataAddedEventArgs e) {
            //todo implement this.
            var type = sender.GetType();
            
            if (type == typeof(PSDataCollection<VerboseRecord>))
            {
                VerboseRecord record = ((PSDataCollection<VerboseRecord>)sender)[e.Index];
            }
            if (type == typeof(PSDataCollection<DebugRecord>))
            {
                DebugRecord record = ((PSDataCollection<DebugRecord>)sender)[e.Index];
            }
            if (type == typeof(PSDataCollection<ErrorRecord>))
            {
                ErrorRecord record = ((PSDataCollection<ErrorRecord>)sender)[e.Index];
            }
            if (type == typeof(PSDataCollection<WarningRecord>))
            {
                WarningRecord record = ((PSDataCollection<WarningRecord>)sender)[e.Index];
            }
        }
        //I know the Output isnt a stream technically... we can rename the class later.
        public void RecordOutput(PoshJob poshJob, object sender, DataAddedEventArgs e)
        {
            var record = ((PSDataCollection<PSDataCollection<PSObject>>)sender)[e.Index];
            Log.Information($"{poshJob.JobUID} Output: Type - {record[0].TypeNames[0]} Value - {record[0].BaseObject}");
        }
    }
}

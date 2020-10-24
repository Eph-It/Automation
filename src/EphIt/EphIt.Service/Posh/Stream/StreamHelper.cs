using System.Management.Automation;

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
    }
}

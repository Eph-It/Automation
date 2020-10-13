using System;

namespace EphIt.Db.Models
{
    public partial class VMJobLog
    {
        public VMJobLog()
        {
            
        }

        public VMJobLog(JobLog obj)
        {
            JobLogId = obj.JobLogId;
            JobUid = obj.JobUid;
            Message = obj.Message;
            Stream = obj.Stream;
            LogTime = obj.LogTime;
        }

        public long JobLogId { get; set; }
        public Guid JobUid { get; set; }
        public string Message { get; set; }
        public string Stream { get; set; }
        public DateTime LogTime { get; set; }

    }
}

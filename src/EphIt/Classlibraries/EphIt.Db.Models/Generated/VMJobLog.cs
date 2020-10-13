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
            LevelId = obj.LevelId;
            LogTime = obj.LogTime;
        }

        public Guid JobLogId { get; set; }
        public Guid JobUid { get; set; }
        public string Message { get; set; }
        public short LevelId { get; set; }
        public DateTime LogTime { get; set; }

    }
}

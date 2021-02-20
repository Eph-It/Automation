using System.Collections;

namespace EphIt.Db.Models
{
    public class VMScriptJob
    {
        public Job Job { get; set; }
        public string Script { get; set; }
        public Hashtable Parameters { get; set; }
    }
}

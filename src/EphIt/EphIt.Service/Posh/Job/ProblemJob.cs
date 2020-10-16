using System;
using System.Collections.Generic;
using System.Text;

namespace EphIt.Service.Posh.Job
{
    class ProblemJob
    {
        public Exception Exception { get; set; }
        public PoshJob Job {get; set;}
    }
}

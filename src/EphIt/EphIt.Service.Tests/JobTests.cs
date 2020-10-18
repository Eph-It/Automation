using Microsoft.VisualStudio.TestTools.UnitTesting;
using EphIt.Service.Posh;
using EphIt.Service.Posh.Job;
using EphIt.Service.Posh.Stream;
using Moq;
using EphIt.BL.JobManager;
using EphIt.Db.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;

namespace EphIt.Service.Tests
{
    [TestClass]
    public class JobQueueTests
    {
        private Mock<IStreamHelper> streamMoq;
        private IPoshManager poshManager;
        private EphItContext ephItContext;
        private Mock<ILogger<JobManager>> logger;
        private IJobManager jobManager;
        private IPoshJobManager poshJobManager;
        private PoshJob job;
        private void setup()
        {
            streamMoq = new Mock<IStreamHelper>();
            poshManager = new PoshManager(streamMoq.Object);
            ephItContext = new EphItContext();
            logger = new Mock<ILogger<JobManager>>();
            jobManager = new JobManager(ephItContext, logger.Object);
            poshJobManager = new PoshJobManager(poshManager, streamMoq.Object, jobManager);
            job = new PoshJob();
            job.Script = "Get-Module; write-Verbose '123'";
        }
        public JobQueueTests()
        {
            setup();
        }
        [TestMethod]
        public void ShouldQueueJob()
        {
            bool hasJob = poshJobManager.HasPendingJob();
            Assert.IsFalse(hasJob);
            poshJobManager.QueueJob(job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);
        }

        [TestMethod]
        public void ShouldDequeueJob()
        {
            bool hasJob = poshJobManager.HasPendingJob();
            Assert.IsFalse(hasJob);
            poshJobManager.QueueJob(this.job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);

            hasJob = poshJobManager.HasPendingJob();
            PoshJob job = null;  
            if (hasJob)
            {
                job = poshJobManager.DequeueJob();
            }
            Assert.IsNotNull(job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsFalse(hasJob);
        }
        [TestMethod]
        public void ShouldStartJob()
        {
            bool hasJob = poshJobManager.HasPendingJob();
            Assert.IsFalse(hasJob);
            poshJobManager.QueueJob(this.job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);

            //the runspace needs to have commands added.
            //bug must be fixed.
            //poshJobManager.StartPendingJob();

        }
    }
}

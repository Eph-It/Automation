using Microsoft.VisualStudio.TestTools.UnitTesting;
using EphIt.Service.Posh;
using EphIt.Service.Posh.Job;
using Moq;
using EphIt.BL.JobManager;
using EphIt.Db.Models;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using System.Management.Automation;
using EphIt.BL.Script;
using EphIt.BL.User;
using System.Threading;

namespace EphIt.Service.Tests
{
    [TestClass]
    public class JobQueueTests
    {
        /*private Mock<IStreamHelper> streamMoq;
        private IPoshManager poshManager;
        private EphItContext ephItContext;
        private Mock<ILogger<EphIt.BL.JobManager.JobManager>> logger;
        private IJobManager jobManager;
        private IPoshJobManager poshJobManager;
        private PoshJob job;
        private void setup()
        {
            streamMoq = new Mock<IStreamHelper>();
            streamMoq.Setup(s => s.RecordStream(It.IsAny<PoshJob>(), It.IsAny<object>() ,It.IsAny<DataAddedEventArgs>()));
            IStreamHelper realStream = new StreamHelper();
            poshManager = new PoshManager(realStream);
            ephItContext = new EphItContext();
            logger = new Mock<ILogger<EphIt.BL.JobManager.JobManager>>();
            jobManager = new EphIt.BL.JobManager.JobManager(ephItContext, logger.Object);
            poshJobManager = new PoshJobManager(poshManager);
            job = new PoshJob();
            job.Script = "$VerbosePreference = 'Continue'; Get-Module; Write-Verbose '123'; write-debug '123'; write-error '123'; start-sleep 300";
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
            poshJobManager.QueuePendingJob(job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);
        }

        [TestMethod]
        public void ShouldDequeueJob()
        {
            bool hasJob = poshJobManager.HasPendingJob();
            Assert.IsFalse(hasJob);
            poshJobManager.QueuePendingJob(this.job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);

            hasJob = poshJobManager.HasPendingJob();
            PoshJob job = null;  
            if (hasJob)
            {
                job = poshJobManager.DequeuePendingJob();
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
            poshJobManager.QueuePendingJob(this.job);
            hasJob = poshJobManager.HasPendingJob();
            Assert.IsTrue(hasJob);

            poshJobManager.StartPendingJob();
        }
        private void StartJob()
        {
            poshJobManager.QueuePendingJob(this.job);
            poshJobManager.StartPendingJob();
        }
         unsure how to test this
        [TestMethod]
        public void ShouldRecordVerboseLog()
        {
            StartJob();
            Thread.Sleep(2000);
            streamMoq.Verify(m => m.RecordStream(It.IsAny<PoshJob>(), It.IsAny<object>(), It.IsAny<DataAddedEventArgs>()), Times.Once);
        }
        */
    }
}

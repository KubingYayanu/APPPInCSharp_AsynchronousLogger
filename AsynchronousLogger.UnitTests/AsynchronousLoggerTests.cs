using NUnit.Framework;
using System;
using System.Threading;

namespace AsynchronousLogger.UnitTests
{
    [TestFixture]
    public class AsynchronousLoggerTests
    {
        private AsynchronousLogger logger;
        private int messagesLogged;

        [SetUp]
        public void SetUp()
        {
            messagesLogged = 0;
            logger = new AsynchronousLogger(Console.Out);
            Pause();
        }

        [TearDown]
        public void TearDown()
        {
            logger.Stop();
        }

        [Test]
        public void OneMessage()
        {
            logger.LogMessage("one message");
            CheckMessagesFlowToLog(1);
        }

        [Test]
        public void TwoConsecutiveMessages()
        {
            logger.LogMessage("another");
            logger.LogMessage("and another");
            CheckMessagesFlowToLog(2);
        }

        [Test]
        public void ManyMessages()
        {
            for (int i = 0; i < 10; i++)
            {
                logger.LogMessage(string.Format("message:{0}", i));
                CheckMessagesFlowToLog(1);
            }
        }

        private void CheckMessagesFlowToLog(int queued)
        {
            CheckQueuqdAndLogged(queued, messagesLogged);
            Pause();
            messagesLogged += queued;
            CheckQueuqdAndLogged(0, messagesLogged);
        }

        private void CheckQueuqdAndLogged(int queued, int logged)
        {
            Assert.AreEqual(queued, logger.MessagesInQueue(), "queued");
            Assert.AreEqual(logged, logger.MessagesLogged(), "logged");
        }

        private void Pause()
        {
            Thread.Sleep(50);
        }
    }
}
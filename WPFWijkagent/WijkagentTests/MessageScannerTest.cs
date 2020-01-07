using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using WijkagentModels;
using WijkagentWPF;

namespace WijkagentTests
{
    [TestFixture]
    public class MessageScannerTest
    {
        long id = 1206933407655243778;

        MessageScanner scanner;
        ContactWitnessDialogueController controller;

        /// <summary>
        /// Checks, whether the Message has correctly been sent to a observer of the scanners
        /// </summary>
        [Test]
        public void CompareConversation_DifferentListsGIven_ControllerMessagesUpdates()
        {
            //arrange
            scanner = new MessageScanner(id);
            controller = new ContactWitnessDialogueController(id);
            
            //act
            scanner.Attach(controller);
            scanner.StartScanning(2000);
            
            scanner.StopScanning();
            int results = controller.directMessages.Count;

            //assert
            Assert.AreEqual(scanner._messages, controller.directMessages);
            Assert.Greater(results, 0);
            Console.WriteLine(results);
        }

        [Test]
        public void GetConversation_MessageISSent_ListIsDifferent()
        {
            scanner = new MessageScanner(id);
            int result = scanner.GetConversation().Count;
            Assert.Greater(result, 0);
        }

        [Test]
        public void ScanConversation_NewMessageSent_Messageadded()
        {
            //arrange
            scanner = new MessageScanner(id);
            controller = new ContactWitnessDialogueController(id);
            //act
            scanner.Attach(controller);
            scanner.StartScanning(2000);
            scanner.scraper.SentDirectMessage("test message", id);
            System.Threading.Thread.Sleep(6000);
            scanner.StopScanning();
            //assert
            Assert.AreEqual("test message",scanner._messages[0].Content);
            Assert.AreEqual("test message", controller.directMessages[0].Content);


        }
    }
}

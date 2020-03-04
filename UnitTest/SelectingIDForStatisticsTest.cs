using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class SelectingIDForStatisticsTest
    {
        [TestMethod]
        public async Task GetNewFinishedRejectsIfGivenTimeIsInvalidTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            ActionResult<string> result = await controller.GetNewFinished("INVALID TIME");
            Assert.AreNotEqual(((ObjectResult) result.Result).StatusCode, 200);
        }
        [TestMethod]
        public async Task GetNewFinishedReturnsTimeOfReceivingTest()
        {
            DateTime givenTime = DateTime.UtcNow;
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            System.Threading.Thread.Sleep(1000); // Add sleep because converting DateTime to string will drop milliseconds.
            ActionResult<string> result = await controller.GetNewFinished(givenTime.ToString());
            DateTime.TryParse(((ObjectResult) result.Result).Value.ToString(), out DateTime returnedTime);
            Assert.IsTrue(returnedTime >= givenTime);
        }
        [TestMethod]
        public async Task GetNotFoundIfNoNewCandidatesAreFinishedTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string idBeforeTime = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(idBeforeTime);
			await gameSessionController.EndSession(idBeforeTime);
            System.Threading.Thread.Sleep(1000); // Add sleep to see difference between ending time of Test and the time that will be send to the controller.
            ActionResult<string> result = await controller.GetNewFinished(DateTime.UtcNow.ToString());
            Assert.AreEqual(((ObjectResult) result.Result).StatusCode, 404);
        }
        [TestMethod]
        public async Task GetNewFinishedReturnsOnlyNewFinishedIDTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string idBeforeTime = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(idBeforeTime);
			string idAfterTime = await repo.AddCandidate("TestAfter");
			await gameSessionController.StartSession(idAfterTime);
			await gameSessionController.EndSession(idBeforeTime);
            System.Threading.Thread.Sleep(1000); // Add sleep to get a difference in ending time of Test and the variable time below.
            DateTime time = DateTime.UtcNow;
			await gameSessionController.EndSession(idAfterTime);
            ActionResult<string> result = await controller.GetNewFinished(time.ToString());
            var definition = new {Ids = new string[0], time = ""};
            string[] newFinished = JsonConvert.DeserializeAnonymousType(result.Value, definition).Ids;
            Assert.AreEqual(1, newFinished.Length);
            Assert.AreEqual(idAfterTime, newFinished[0]);
        }
        [TestMethod]
        public async Task GetNewFinishedReturnsOnlyIDsOfFinishedCandidatesTest()
        {
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string idBeforeTime = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(idBeforeTime);
			string idAfterTime = await repo.AddCandidate("TestAfter");
			await gameSessionController.StartSession(idAfterTime);
            DateTime time = DateTime.UtcNow;
			await gameSessionController.EndSession(idAfterTime);
            ActionResult<string> result = await controller.GetNewFinished(time.ToString());
            var definition = new {Ids = new string[0], time = ""};
            string[] newFinished = JsonConvert.DeserializeAnonymousType(result.Value, definition).Ids;
            Assert.AreEqual(1, newFinished.Length);
            Assert.AreEqual(idAfterTime, newFinished[0]);
        }
		[TestMethod]
		public async Task GetPreviousIDRejectsWithInvalidIDTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(id);
			await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.GetPreviousFinished(id + "NOT");
			Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
		}
		[TestMethod]
		public async Task GetPreviousIDBeforeFirstCandidateIsNullTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(id);
			await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.GetPreviousFinished(id);
			Assert.AreEqual(((StatusCodeResult) result.Result).StatusCode, 404);
		}
		[TestMethod]
		public async Task GetPreviousIDWhichIsFinishedTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id1 = await repo.AddCandidate("Test kandidaat 1");
			await gameSessionController.StartSession(id1);
			string id2 = await repo.AddCandidate("Test kandidaat 2");
			await gameSessionController.StartSession(id2);
			string id3 = await repo.AddCandidate("Test kandidaat 3");
			await gameSessionController.StartSession(id3);
			string id4 = await repo.AddCandidate("Test kandidaat 4");
			await gameSessionController.StartSession(id4);
			string id5 = await repo.AddCandidate("Test kandidaat 5");
			await gameSessionController.StartSession(id5);
			await gameSessionController.EndSession(id1);
			await gameSessionController.EndSession(id2);
			await gameSessionController.EndSession(id4);
			await gameSessionController.EndSession(id5);
            ActionResult<string> result = await controller.GetPreviousFinished(id4);
			Assert.AreEqual(id2, result.Value);
		}
		[TestMethod]
		public async Task GetNextIDRejectsWithInvalidIDTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(id);
			await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.GetNextFinished(id + "NOT");
			Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
		}
		[TestMethod]
		public async Task GetPreviousIDAfterLastCandidateIsNullTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id = await repo.AddCandidate("Test");
			await gameSessionController.StartSession(id);
			await gameSessionController.EndSession(id);
            ActionResult<string> result = await controller.GetNextFinished(id);
			Assert.AreEqual(((StatusCodeResult) result.Result).StatusCode, 404);
		}
		[TestMethod]
		public async Task GetNextIDWhichIsFinishedTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id1 = await repo.AddCandidate("Test kandidaat 1");
			await gameSessionController.StartSession(id1);
			string id2 = await repo.AddCandidate("Test kandidaat 2");
			await gameSessionController.StartSession(id2);
			string id3 = await repo.AddCandidate("Test kandidaat 3");
			await gameSessionController.StartSession(id3);
			string id4 = await repo.AddCandidate("Test kandidaat 4");
			await gameSessionController.StartSession(id4);
			string id5 = await repo.AddCandidate("Test kandidaat 5");
			await gameSessionController.StartSession(id5);
			await gameSessionController.EndSession(id1);
			await gameSessionController.EndSession(id2);
			await gameSessionController.EndSession(id4);
			await gameSessionController.EndSession(id5);
            ActionResult<string> result = await controller.GetNextFinished(id2);
			Assert.AreEqual(id4, result.Value);
		}
		[TestMethod]
		public async Task GetLastFinishedReturnsTheRightIDTest()
		{
            IRepository repo = new TestDB();
            StatisticsController controller = new StatisticsController(repo);
            GameSessionController gameSessionController = new GameSessionController(repo);
			string id1 = await repo.AddCandidate("Test kandidaat 1");
			await gameSessionController.StartSession(id1);
			string lastFinishing = await repo.AddCandidate("Test kandidaat 2");
			await gameSessionController.StartSession(lastFinishing);
			string id3 = await repo.AddCandidate("Test kandidaat 3");
			await gameSessionController.StartSession(id3);
			await gameSessionController.EndSession(id1);
			await gameSessionController.EndSession(id3);
			await gameSessionController.EndSession(lastFinishing);
            ActionResult<string> result = await controller.GetLastFinished();
			Assert.AreEqual(lastFinishing, result.Value);
		}
    }
}

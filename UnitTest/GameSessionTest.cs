using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Text.Json;
using Newtonsoft.Json;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class GameSessionTest
    {
        [TestMethod]
        public async Task StartSessionAcceptsValidIDTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult result = await controller.StartSession(id);
            Assert.AreEqual(((StatusCodeResult) result).StatusCode, 200);
        }
        [TestMethod]
        public async Task StartSessionSetsStartedTest()
        {
            DateTime timeBeforeStart = DateTime.UtcNow;
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            CandidateEntity candidate = await repo.GetCandidate(id);
            Assert.IsTrue(candidate.started >= timeBeforeStart);
        }
        [TestMethod]
        public async Task StartSessionRejectsWithInvalidIDTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult result = await controller.StartSession(id + "NOT");
            Assert.AreNotEqual(((StatusCodeResult) result).StatusCode, 200);
        }
        [TestMethod]
        public async Task CanNotStartSessionIfSessionIsAlreadyStartedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            CandidateEntity candidate = await repo.GetCandidate(id);
            DateTime timeFirstTimeStart = candidate.started;
            await controller.StartSession(id);
            CandidateEntity candidateAgain = await repo.GetCandidate(id);
            Assert.AreEqual(timeFirstTimeStart, candidate.started);
        }
        [TestMethod]
        public async Task EndSessionAcceptsValidIDTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            ActionResult result = await controller.EndSession(id);
            Assert.AreEqual(((StatusCodeResult) result).StatusCode, 200);
        }
        [TestMethod]
        public async Task EndSessionSetsFinishedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            DateTime timeBeforeEnd = DateTime.UtcNow;
            await controller.EndSession(id);
            CandidateEntity candidate = await repo.GetCandidate(id);
            Assert.IsTrue(candidate.finished >= timeBeforeEnd);
        }
        [TestMethod]
        public async Task EndSessionRejectsWithInvalidIDTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            ActionResult result = await controller.EndSession(id + "NOT");
            Assert.AreNotEqual(((StatusCodeResult) result).StatusCode, 200);
        }
        [TestMethod]
        public async Task CanNotEndSessionIfSessionIsNotStartedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.EndSession(id);
            CandidateEntity candidate = await repo.GetCandidate(id);
            Assert.AreEqual(new DateTime(), candidate.finished);
        }
        [TestMethod]
        public async Task CanNotEndSessionIfSessionIsAlreadyEndedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            await controller.EndSession(id);
            CandidateEntity candidate = await repo.GetCandidate(id);
            DateTime timeFirstEnd = candidate.finished;
            await controller.EndSession(id);
            CandidateEntity candidateAgain = await repo.GetCandidate(id);
            Assert.AreEqual(timeFirstEnd, candidate.finished);
        }
        [TestMethod]
        public async Task GetOverviewRejectsIfCandidateIsNotStartedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.GetOverview(id);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
            Assert.IsNull(result.Value);
        }
        [TestMethod]
        public async Task GetOverviewGivesOverviewForEveryLevelTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            ActionResult<string> result = await controller.GetOverview(id);
            /// The vars are needed to create an anonymous type in order to deserialize the return value.
            var definition = new {levels = new JsonElement[0]};
            var overview = JsonConvert.DeserializeAnonymousType(result.Value, definition);
            Assert.AreEqual(controller.GetTotalAmountLevels(), overview.levels.Length);
        }
        [TestMethod]
        public async Task GetRemainingTimeRejectsWhenSessionIsNotStartedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<long> result = await controller.GetRemainingTime(id);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
            Assert.AreEqual(0,result.Value);
        }
        [TestMethod]
        public async Task GetRemainingTimeRejectsWhenSessionIsEndedTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            await controller.EndSession(id);
            ActionResult<long> result = await controller.GetRemainingTime(id);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
            Assert.AreEqual(0,result.Value);
        }
        [TestMethod]
        public async Task GetRemainingTimeResolvesWhenThereIsTimeLeftTest()
        {
            IRepository repo = new TestDB();
            GameSessionController controller = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await controller.StartSession(id);
            ActionResult<long> result = await controller.GetRemainingTime(id);
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
            Assert.IsTrue(result.Value > 0);
        }
    }
}

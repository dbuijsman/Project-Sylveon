using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class LevelSessionControllerTest
    {
        [TestMethod]
        public async Task StartLevelSessionWithoutStartingGameSessionRejectsTest()
        {
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.StartLevel(id,"1");
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        [TestMethod]
        public async Task StartLevelSessionAfterStartingGameSessionResolvesTest()
        {
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            ActionResult<string> result = await controller.StartLevel(id,"1");
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
        }
        [TestMethod]
        public async Task StartLevelSessionSetsLevelSessionInProgressTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            DateTime timeOfStarting = DateTime.UtcNow;
            await controller.StartLevel(id, levelNumber.ToString());
            CandidateEntity candidate = await repo.GetCandidate(id);
            LevelSession levelSession = candidate.GetLevelSession(levelNumber);
            Assert.IsTrue(levelSession.InProgress);
        }
        [TestMethod]
        public async Task StartLevelSessionOfLevelThatIsNaNTest()
        {
            string levelNumberNaN = "Not a number";
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            DateTime timeOfStarting = DateTime.UtcNow;
            ActionResult<string> result = await controller.StartLevel(id, levelNumberNaN);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        [TestMethod]
        public async Task StartLevelSessionWithInvalidLevelNumberTest()
        {
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string levelNumberNaN = (gameController.GetTotalAmountLevels()+4).ToString();
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            DateTime timeOfStarting = DateTime.UtcNow;
            ActionResult<string> result = await controller.StartLevel(id, levelNumberNaN);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        [TestMethod]
        public async Task StopLevelSessionWithoutStartingGameSessionRejectsTest()
        {
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            ActionResult<string> result = await controller.StopLevel(id,"1");
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        [TestMethod]
        public async Task StopLevelSessionSetsLevelSessionNotInProgressTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            LevelSessionController controller = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            DateTime timeOfStarting = DateTime.UtcNow;
            await controller.StartLevel(id, levelNumber.ToString());
            await controller.StopLevel(id, levelNumber.ToString());
            CandidateEntity candidate = await repo.GetCandidate(id);
            LevelSession levelSession = candidate.GetLevelSession(levelNumber);
            Assert.IsFalse(levelSession.InProgress);
        }
    }
}

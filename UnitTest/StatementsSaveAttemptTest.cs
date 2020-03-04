using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd;
using React_Frontend.Controllers;

namespace UnitTest
{
    [TestClass]
    public class StatementsSaveAttemptTest
    {
        [TestMethod]
        public async Task SendingNoStatementsRejectsTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            ActionResult<string> result = await controller.SaveAttempt(id, levelNumber, null);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task SendingEmptyStatementsRejectsTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            ActionResult<string> result = await controller.SaveAttempt(id, levelNumber, new Statement[0]);
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task SendingEmptyStatementsDoesNotSaveTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            await controller.SaveAttempt(id, levelNumber, new Statement[0]);
            CandidateEntity candidate = await repo.GetCandidate(id);
            List<LevelSolution> solutions = candidate.GetLevelSession(levelNumber).Solutions;
            Assert.AreEqual(0, solutions.Count);
        }
        
        [TestMethod]
        public async Task SaveStatementsWhileLevelSessionIsNotInProgressRejectsTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            ActionResult<string> result = await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            Assert.AreNotEqual(((StatusCodeResult) result.Result).StatusCode, 200);
        }
        
        [TestMethod]
        public async Task SaveStatementsWhileLevelSessionIsNotInProgressDoesNotSaveTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            CandidateEntity candidate = await repo.GetCandidate(id);
            List<LevelSolution> solutions = candidate.GetLevelSession(levelNumber).Solutions;
            Assert.AreEqual(0, solutions.Count);
        }
        
        [TestMethod]
        public async Task SaveStatementsAcceptsStatementsTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            ActionResult<string> result = await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            Assert.IsTrue(result.Result == null || ((StatusCodeResult) result.Result).StatusCode == 200);
        }
        
        [TestMethod]
        public async Task SaveStatementsSavesAttemptTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            CandidateEntity candidate = await repo.GetCandidate(id);
            List<LevelSolution> solutions = candidate.GetLevelSession(levelNumber).Solutions;
            Assert.AreEqual(1, solutions.Count);
        }
        
        [TestMethod]
        public async Task SaveStatementsSavesDuplicateAttemptTest()
        {
            int levelNumber = 1;
            IRepository repo = new TestDB();
            StatementController controller = new StatementController(repo);
            GameSessionController gameController = new GameSessionController(repo);
            LevelSessionController levelController = new LevelSessionController(repo);
            string id = await repo.AddCandidate("Test");
            await gameController.StartSession(id);
            await levelController.StartLevel(id, levelNumber.ToString());
            await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            await controller.SaveAttempt(id, levelNumber, new Statement[]{new SingleCommand(Command.RotateLeft)});
            CandidateEntity candidate = await repo.GetCandidate(id);
            List<LevelSolution> solutions = candidate.GetLevelSession(levelNumber).Solutions;
            Assert.AreEqual(2, solutions.Count);
        }
    }
}

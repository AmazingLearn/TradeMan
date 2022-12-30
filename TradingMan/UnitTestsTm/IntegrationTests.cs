using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.BusinessLayer.Logic.Messaging;
using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;

namespace UnitTestsTm
{
    [TestClass]
    public class IntegrationTests
    {
        //Mock Db context
        private static DatabaseContext _context;

        // Logic services
        private static StockDataLogic _stockDataLogic;
        private static PositionLogic _positionLogic;
        private static NotificationsLogic _notificationsLogic;
        private static NotificationEvaluation _notificationEvaluation;

        // Test data
        private const string _testProductSymbol = "AAPL";

        // Sets up the class before each test
        [ClassInitialize]
        public static void Init(TestContext c)
        {
            // Create mock instance of DB
            var options = new DbContextOptionsBuilder<DatabaseContext>()
                .UseInMemoryDatabase(databaseName: "testDb").Options;

            _context = new DatabaseContext(options);

            // Prepare loggers
            var uMocklLogger = new Mock<ILogger<UsersLogic>>();
            var sMockLogger = new Mock<ILogger<StockDataLogic>>();
            var pMockLogger = new Mock<ILogger<PositionLogic>>();
            var nMockLogger = new Mock<ILogger<NotificationsLogic>>();
            var nEMockLogger = new Mock<ILogger<NotificationEvaluation>>();

            // Mock this service, messaging requires valid channels to message to.
            var mLogic = new Mock<MessagingLogic>();
            mLogic.Setup(ml => ml.SendMessageToAllRegisteredChannels(It.IsAny<Message>(), It.IsAny<Guid>()));

            // Initialize services
            _stockDataLogic = new StockDataLogic(sMockLogger.Object);
            _positionLogic = new PositionLogic(_context, pMockLogger.Object);
            _notificationsLogic = new NotificationsLogic(_context, nMockLogger.Object);
            _notificationEvaluation = new NotificationEvaluation(_context, nEMockLogger.Object, _stockDataLogic, mLogic.Object);

            
        }
        
        [TestMethod]
        public async Task TestAbsoluteIncreaseSuccess()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should get fullfilled - absolute change condition increase
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not1",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.AbsoluteChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = double.MinValue,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsTrue(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsTrue(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));

        }

        [TestMethod]
        public async Task TestAbsoluteDecreaseSuccess()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should get fullfilled - absolute change condition decrease
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not2",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.AbsoluteChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = double.MaxValue,
                Direction = Direction.Decrease,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsTrue(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsTrue(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }


        [TestMethod]
        public async Task TestPercentageIncreaseSuccess()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should get fullfilled - percentual change condition increase
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not3",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 0.01,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsTrue(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsTrue(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestPercentageDecreaseSuccess()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should get fullfilled - percentual change condition decrease
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not4",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 10,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 100000000,
                Direction = Direction.Decrease,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsTrue(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsTrue(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestAbsoluteIncreaseFail()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - absolute change condition increase
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not7",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.AbsoluteChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = double.MaxValue,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsFalse(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestAbsoluteDecreaseFail()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - absolute change condition decrease
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not8",
                Symbol = "AAPL",
                NotificationBasicType = NotificationBasicType.AbsoluteChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = double.MinValue,
                Direction = Direction.Decrease,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsFalse(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestPercentageIncreaseFail()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - percentual change condition increase
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not9",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 10000,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsFalse(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestPercentageDecreaseFail()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - percentual change condition decrease
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not10",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 10,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 100,
                Direction = Direction.Decrease,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsFalse(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestExpired()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - expired
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not5",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(-1),
                BaseValue = 0.001,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            _notificationsLogic.CreateNotification(testNotification);
            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsFalse(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestAlreadyFullfilled()
        {
            var evaluationTestUserId = Guid.NewGuid();

            // Should not get fullfilled - already fullfilled
            var testNotification = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not6",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 0.001,
                Direction = Direction.Increase,
                Fullfilled = true
            };

            _notificationsLogic.CreateNotification(testNotification);

            _notificationEvaluation.EvaluateNotifications();
            var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, testNotification.NotificationId);
            Assert.IsTrue(not.Fullfilled);

            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);
            Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == testNotification.Name));
        }

        [TestMethod]
        public async Task TestMultipleEvaluation()
        {
            var evaluationTestUserId = Guid.NewGuid();

            var testNotification1 = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not3",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 0.01,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            // Should get fullfilled - percentual change condition decrease
            var testNotification2 = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not4",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 10,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 100000000,
                Direction = Direction.Decrease,
                Fullfilled = false
            };

            // Should not get fullfilled - percentual change condition increase
            var testNotification3 = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not9",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 10000,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            // Should not get fullfilled - expired
            var testNotification4 = new NotificationBasic
            {
                UserId = evaluationTestUserId,
                Name = "Not5",
                Symbol = _testProductSymbol,
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow.AddDays(-1),
                BaseValue = 0.001,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            var fList = new List<INotification>
            {
                testNotification1,
                testNotification2
            };

            var nfList = new List<INotification>
            {
                testNotification3,
                testNotification4
            };

            var nList = new List<INotification>();
            nList.AddRange(fList);
            nList.AddRange(nfList);

            foreach (var n in nList)
            {
                _notificationsLogic.CreateNotification(n);
            }

            _notificationEvaluation.EvaluateNotifications();
            var positionList = _positionLogic.GetAllPositions(evaluationTestUserId);

            foreach (var n in fList)
            {
                var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, n.NotificationId);
                Assert.IsTrue(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == n.Name));
            }

            foreach (var n in nfList)
            {
                var not = _notificationsLogic.GetSingleNotification(evaluationTestUserId, n.NotificationId);
                Assert.IsFalse(positionList.Any(p => p.UserId == evaluationTestUserId && p.NotificationName == n.Name));
            }
        }
    }
}

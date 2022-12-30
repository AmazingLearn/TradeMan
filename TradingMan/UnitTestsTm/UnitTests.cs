using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;

namespace UnitTestsTm
{
    [TestClass]
    public class UnitTests
    {
        //Mock Db context
        private static DatabaseContext _context;

        // Real logic services
        private static UsersLogic _usersLogic;
        private static StockDataLogic _stockDataLogic;
        private static PositionLogic _positionLogic;
        private static NotificationsLogic _notificationsLogic;

        // Test data
        private const string _testProductSymbol = "AAPL";
        
        private Guid _invalidGuid = Guid.NewGuid();

        private User testUser = new User
        {
            Email = "test@test.test",
            Password = "test"
        };

        private AccountSettings testAss = new AccountSettings
        {
            AlpacaApiKey = "testApiKey",
            AlpacaSecretKey = "testSecretKey",
            TelegramUsername = "testUserName",
            UseTelegram = true
        };

        // Sets up the class - is ran before each test
        [TestInitialize]
        public void Init()
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

            // Initialize services
            _usersLogic = new UsersLogic(_context, uMocklLogger.Object);
            _stockDataLogic = new StockDataLogic(sMockLogger.Object);
            _positionLogic = new PositionLogic(_context, pMockLogger.Object);
            _notificationsLogic = new NotificationsLogic(_context, nMockLogger.Object);
        }

        [TestMethod]
        public async Task TestUsersLogic()
        {
            // Test user creation
            _usersLogic.CreateUser(testUser);
            var users = await _context.Users.ToListAsync();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual(testUser.Email, users[0].Email);
            Assert.AreEqual(testUser.Password, users[0].Password);

            var testInvalidUser1 = new User
            {
                Email = "invalid@test.test",
                Password = "test"
            };

            var testInvalidUser2 = new User
            {
                Email = "test@test.test",
                Password = "invalid"
            };

            // Test user validation
            Assert.AreEqual(testUser.UserId, _usersLogic.VerifyUser(testUser));
            Assert.AreEqual(Guid.Empty, _usersLogic.VerifyUser(testInvalidUser1));
            Assert.AreEqual(Guid.Empty, _usersLogic.VerifyUser(testInvalidUser2));

            // UserId In account settings has to be updatet
            testAss.UserId = testUser.UserId;

            // Test adding account setting
            _usersLogic.SetAccountSettings(testAss);
            var accountSettingsList = await _context.AccountSettings.ToListAsync();
            Assert.AreEqual(1, accountSettingsList.Count);
            Assert.AreEqual(testAss.AlpacaSecretKey, accountSettingsList[0].AlpacaSecretKey);
            Assert.AreEqual(testAss.AlpacaApiKey, accountSettingsList[0].AlpacaApiKey);
            Assert.AreEqual(testAss.UseTelegram, accountSettingsList[0].UseTelegram);

            // Test geting account settigns
            var accountSettings = _usersLogic.GetAccountSettings(testUser.UserId);
            Assert.AreEqual(testAss.AlpacaSecretKey, accountSettings.AlpacaSecretKey);
            Assert.AreEqual(testAss.AlpacaApiKey, accountSettings.AlpacaApiKey);
            Assert.AreEqual(testAss.TelegramUsername, accountSettings.TelegramUsername);
            Assert.AreEqual(testAss.UseTelegram, accountSettings.UseTelegram);

            var invalidGuidaction = new Action(() =>
            {
                _usersLogic.GetAccountSettings(_invalidGuid);
            });

            Assert.ThrowsException<Exception>(invalidGuidaction);
        }

        [TestMethod]
        public void TestPositionsLogic()
        {
            // Setup a position in DB
            var positionTestUserId = Guid.NewGuid();

            var testPosition1 = new Position
            {
                UserId = positionTestUserId,
                ProductSymbol = _testProductSymbol,
                PositionType = PositionType.Buy,
                BaseValue = 100,
                NotificationName = "testNotification"
            };

            var testPosition2 = new Position
            {
                UserId = positionTestUserId,
                ProductSymbol = _testProductSymbol,
                PositionType = PositionType.Buy,
                BaseValue = 100,
                NotificationName = "testNotification"
            };

            var testPosition3 = new Position
            {
                UserId = positionTestUserId,
                ProductSymbol = _testProductSymbol,
                PositionType = PositionType.Buy,
                BaseValue = 100,
                NotificationName = "testNotification"
            };

            var testPosition4 = new Position
            {
                UserId = positionTestUserId,
                ProductSymbol = _testProductSymbol,
                PositionType = PositionType.Buy,
                BaseValue = 100,
                NotificationName = "testNotification"
            };
           

            _context.Add(testPosition1);
            _context.Add(testPosition2);
            _context.Add(testPosition3);
            _context.Add(testPosition4);
            _context.SaveChanges();

            // Test getting all positions for userId
            var allPositions = _positionLogic.GetAllPositions(positionTestUserId);
            Assert.AreEqual(4, allPositions.Count);
            var noPositions = _positionLogic.GetAllPositions(_invalidGuid);
            Assert.IsFalse(noPositions.Any());

            // Test Get single posiiton
            var pos = _positionLogic.GetPosition(positionTestUserId, testPosition1.PositionId);
            Assert.IsNotNull(pos);
            Assert.AreEqual(pos.PositionId, testPosition1.PositionId);

            pos = _positionLogic.GetPosition(positionTestUserId, testPosition2.PositionId);
            Assert.IsNotNull(pos);
            Assert.AreEqual(pos.PositionId, testPosition2.PositionId);

            pos = _positionLogic.GetPosition(positionTestUserId, testPosition3.PositionId);
            Assert.IsNotNull(pos);
            Assert.AreEqual(pos.PositionId, testPosition3.PositionId);

            pos = _positionLogic.GetPosition(positionTestUserId, testPosition3.PositionId);
            Assert.IsNotNull(pos);
            Assert.AreEqual(pos.PositionId, testPosition3.PositionId);

            pos = _positionLogic.GetPosition(positionTestUserId, _invalidGuid);
            Assert.IsNull(pos);

            pos = _positionLogic.GetPosition(_invalidGuid, testPosition1.PositionId);
            Assert.IsNull(pos);

            pos = _positionLogic.GetPosition(_invalidGuid, _invalidGuid);
            Assert.IsNull(pos);

            // Test Remove position
            // Throws multiple exceptions
            var exceptionThrown = false;
            try
            {
                _positionLogic.RemovePosition(_invalidGuid);
            }
            catch
            {
                exceptionThrown = true;
            }

            Assert.IsTrue(exceptionThrown);

            _positionLogic.RemovePosition(testPosition1.PositionId);
            allPositions = _positionLogic.GetAllPositions(positionTestUserId);
            Assert.AreEqual(3, allPositions.Count);
        }

        [TestMethod]
        [Ignore("Requires valid alpaca api and secret key.")]
        public void TestOrdersLogic()
        {
            // Not implemented
        }
        
        [TestMethod]
        public void TestNotificationsLogic()
        {
            var notificationTestUserId = Guid.NewGuid();
            var testNotification1 = new NotificationBasic
            {
                UserId = notificationTestUserId,
                Name = "Not1",
                Symbol = "Sym1",
                NotificationBasicType = NotificationBasicType.AbsoluteChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow,
                BaseValue = 10,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            var testNotification2 = new NotificationBasic
            {
                UserId = notificationTestUserId,
                Name = "Not2",
                Symbol = "Sym2",
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 30,
                ExpiryDate = DateTime.UtcNow.AddDays(1),
                BaseValue = 3,
                Direction = Direction.Decrease,
                Fullfilled = true
            };

            var testNotification3 = new NotificationBasic
            {
                UserId = notificationTestUserId,
                Name = "Not3",
                Symbol = "Sym3",
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow,
                BaseValue = 10,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            var testNotification4 = new NotificationTrend
            {
                UserId = notificationTestUserId,
                Name = "Not4",
                Symbol = "Sym4",
                ExpiryDate = DateTime.UtcNow,
                BaseValue = 10,
                Boundary = Boundary.Resistance,
                Fullfilled = false
            };

            var notList = new List<INotification>()
            {
                testNotification1,
                testNotification2,
                testNotification3,
                testNotification4,
            };

            // Test CreateNotification - just make sure it doesnt throw when inserting valid notifications
            foreach(var n in notList)
            {
                _notificationsLogic.CreateNotification(n);
            }

            // Test GetNotifications
            var allNotifications = _notificationsLogic.GetNotifications(notificationTestUserId).ToList();
            Assert.AreEqual(notList.Count, allNotifications.Count);

            var invalid = _notificationsLogic.GetNotifications(_invalidGuid);
            Assert.IsFalse(invalid.Any());

            var testNotification5 = new NotificationBasic
            {
                UserId = notificationTestUserId,
                Name = "Not5",
                Symbol = "Sym5",
                NotificationBasicType = NotificationBasicType.PercentualChange,
                ExpectedChange = 100,
                ExpiryDate = DateTime.UtcNow,
                BaseValue = 10,
                Direction = Direction.Increase,
                Fullfilled = false
            };

            // Test RemoveNotification
            _notificationsLogic.RemoveNotification(testNotification3);
            allNotifications = _notificationsLogic.GetNotifications(notificationTestUserId).ToList();
            Assert.AreEqual(notList.Count - 1, allNotifications.Count);

            var action = new Action(() =>
            {
                _notificationsLogic.RemoveNotification(testNotification3);
            });

            Assert.ThrowsException<DbUpdateConcurrencyException> (action);

            // Test GetSingleNotification
            var notificationBasic = _notificationsLogic.GetSingleNotification(notificationTestUserId, testNotification1.NotificationId) as NotificationBasic;
            Assert.AreEqual(testNotification1.Name, notificationBasic.Name);
            Assert.AreEqual(testNotification1.Symbol, notificationBasic.Symbol);
            Assert.AreEqual(testNotification1.NotificationBasicType, notificationBasic.NotificationBasicType);
            Assert.AreEqual(testNotification1.NotificationId, notificationBasic.NotificationId);
            Assert.AreEqual(testNotification1.ExpectedChange, notificationBasic.ExpectedChange);
            Assert.AreEqual(testNotification1.ExpiryDate, notificationBasic.ExpiryDate);
            Assert.AreEqual(testNotification1.BaseValue, notificationBasic.BaseValue);
            Assert.AreEqual(testNotification1.Direction, notificationBasic.Direction);
            Assert.AreEqual(testNotification1.Fullfilled, notificationBasic.Fullfilled);

            notificationBasic = _notificationsLogic.GetSingleNotification(notificationTestUserId, testNotification2.NotificationId) as NotificationBasic;
            Assert.AreEqual(testNotification2.Name, notificationBasic.Name);
            Assert.AreEqual(testNotification2.Symbol, notificationBasic.Symbol);
            Assert.AreEqual(testNotification2.NotificationBasicType, notificationBasic.NotificationBasicType);
            Assert.AreEqual(testNotification2.NotificationId, notificationBasic.NotificationId);
            Assert.AreEqual(testNotification2.ExpectedChange, notificationBasic.ExpectedChange);
            Assert.AreEqual(testNotification2.ExpiryDate, notificationBasic.ExpiryDate);
            Assert.AreEqual(testNotification2.BaseValue, notificationBasic.BaseValue);
            Assert.AreEqual(testNotification2.Direction, notificationBasic.Direction);
            Assert.AreEqual(testNotification2.Fullfilled, notificationBasic.Fullfilled);

            var notificationTrend = _notificationsLogic.GetSingleNotification(notificationTestUserId, testNotification4.NotificationId) as NotificationTrend;
            Assert.AreEqual(testNotification4.Name, notificationTrend.Name);
            Assert.AreEqual(testNotification4.Symbol, notificationTrend.Symbol);
            Assert.AreEqual(testNotification4.NotificationId, notificationTrend.NotificationId);
            Assert.AreEqual(testNotification4.ExpiryDate, notificationTrend.ExpiryDate);
            Assert.AreEqual(testNotification4.BaseValue, notificationTrend.BaseValue);
            Assert.AreEqual(testNotification4.Boundary, notificationTrend.Boundary);
            Assert.AreEqual(testNotification4.Fullfilled, notificationTrend.Fullfilled);

            var notification = _notificationsLogic.GetSingleNotification(notificationTestUserId, testNotification3.NotificationId);
            Assert.IsNull(notification);

            notification = _notificationsLogic.GetSingleNotification(_invalidGuid, testNotification1.NotificationId);
            Assert.IsNull(notification);

            notification = _notificationsLogic.GetSingleNotification(_invalidGuid, int.MinValue);
            Assert.IsNull(notification);
        }
    }
}

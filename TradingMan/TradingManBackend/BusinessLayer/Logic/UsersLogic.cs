using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic
{
    public class UsersLogic
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<UsersLogic> _logger;

        public UsersLogic(DatabaseContext context, ILogger<UsersLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Method to create a new user in app database, returns true on success, return false email is already registerd
        /// Throws othewise.
        /// </summary>
        /// <param name="createUserDto"></param>
        /// <returns></returns>
        public bool CreateUser(User createUser)
        {
            _logger.LogInformation($"Creating user with email: {createUser.Email}");
            if (_context.Users.Any(x => x.Email == createUser.Email))
            {
                _logger.LogInformation($"User email already registered.");
                return false;
            }

            _logger.LogInformation($"User created.");
            _context.Add(createUser);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// Verifies users login details. Returns guid used for verification in other parts of application.
        /// </summary>
        /// <param name="verifyUser"></param>
        /// <returns></returns>
        public Guid VerifyUser(User verifyUser)
        {
            _logger.LogInformation($"Verifying user with email: {verifyUser.Email}");
            var userList = _context.Users.Where(x => x.Email == verifyUser.Email && x.Password == verifyUser.Password).ToList();
            if (userList.Count != 1)
            {
                _logger.LogInformation($"User not registered.");
                return Guid.Empty;
            }

            _logger.LogInformation($"User verfied.");
            return userList.First().UserId;
        }


        /// <summary>
        /// Save new accoutn settings, saves new ones removes previous.
        /// </summary>
        /// <param name="accountSettings"></param>
        public void SetAccountSettings(AccountSettings accountSettings)
        {
            _logger.LogInformation($"Saving account settings for userId: {accountSettings.UserId}");
            var existingAS = _context.AccountSettings.Where(x => x.UserId == accountSettings.UserId);

            if (existingAS.Any())
            {
                _logger.LogInformation($"Removing already exising account settings.");
                _context.Remove(existingAS.First());
            }

            _context.Add(accountSettings);
            _context.SaveChanges();
        }

        /// <summary>
        /// Returns account setting object for given userIds
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AccountSettings GetAccountSettings(Guid userId)
        {
            _logger.LogInformation($"Getting account settings from DB for userId: [{userId}]");
            var accountSettingList = _context.AccountSettings.Where(x => x.UserId == userId).ToList();
            if (accountSettingList.Count == 0)
            {
                var exceptionMessage = "Account settings not present for given user.";
                _logger.LogError(exceptionMessage);
                throw new Exception(exceptionMessage);
            }

            if (accountSettingList.Count != 1)
            {
                var exceptionMessage = "More than one entry found for account settings for given user.";
                _logger.LogError($"{exceptionMessage} userId: [{userId}]");
                throw new Exception(exceptionMessage);
            }

            return accountSettingList.First();
        }
    }
}

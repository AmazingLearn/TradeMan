using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.Logic.Messaging;

namespace TradingManBackend.BusinessLayer.Logic.Messaging
{
    /// <summary>
    /// Class responsible for handling messaging
    /// </summary>
    public class MessagingLogic
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<MessagingLogic> _logger;
        private readonly TelegramMessengerLogic _telegramMessengerLogic;

        /// <summary>
        /// Constructor only for tests!
        /// </summary>
        public MessagingLogic()
        {

        }

        public MessagingLogic(DatabaseContext context, ILogger<MessagingLogic> logger, TelegramMessengerLogic telegramMessengerLogic)
        {
            _context = context;
            _logger = logger;
            _telegramMessengerLogic = telegramMessengerLogic;
        }

        /// <summary>
        /// Sends a message to all channels, for now channels are just email and telegram 
        /// so this is done statically
        /// Virtual so that it can be overriden in tests
        /// </summary>
        /// <param name="message"></param>
        /// <param name="userId"></param>
        virtual public void SendMessageToAllRegisteredChannels(Message message, Guid userId)
        {
            var accountSettings = _context.AccountSettings.Where(x => x.UserId == userId).First();
            var telegramUserName = accountSettings.TelegramUsername;
            
            // Send an email - this will always be sent
            try
            {
                var email = _context.Users.Where(x => x.UserId == userId).First().Email;
                EmailMessenger.SendEmail(email, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            // Send telegram message - if opted for
            try
            {
                if (accountSettings.UseTelegram)
                {
                    var channelId = _context.TelegramChannels.Where(x => x.UserId == userId).First().ChannelId;
                    _telegramMessengerLogic.SendTelegramMessage(channelId, message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}

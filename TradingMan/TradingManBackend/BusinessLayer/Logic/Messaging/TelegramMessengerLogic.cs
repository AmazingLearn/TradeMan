using Telegram.Bot;
using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;
using TradingManBackend.Models.Messaging;
using TradingManBackend.Util;

namespace TradingManBackend.BusinessLayer.Logic.Messaging
{
    public class TelegramMessengerLogic
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<TelegramMessengerLogic> _logger;

        public TelegramMessengerLogic(DatabaseContext context, ILogger<TelegramMessengerLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async void SendTelegramMessage(long channelId, Message message)
        {
            _logger.LogInformation($"Sending telegram message.");
            // Register client based on the api token of a telegram bot.
            var client = GetTelegramBotClient();

            // Send the message to the chat with the client
            await client.SendTextMessageAsync(channelId, message.Subject + ":" + message.Body);
        }

        /// <summary>
        /// Sets up the telegram chat with user
        /// </summary>
        /// <param name="telegramUserName"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task SetupTelegram(string telegramUserName, Guid userId)
        {
            // Register client based on the api token of a telegram bot.
            var client = GetTelegramBotClient();

            // Check that the bot has been successfully initialized
            var clientOk = await client.GetMeAsync();
            if (clientOk == null)
            {
                // Should not happen in production.
                throw new Exception("Telegram client not initialized. Most likely issue with telegram api token.");
            }

            // This returns all bot updates, from where channelId can be gained. Usually all comunication for past few days
            var updates = await client.GetUpdatesAsync();
            var telegramChannel = new TelegramChannel()
            {
                UserId = userId
            };

            foreach (var item in updates)
            {
                if (item.Message?.From?.Username == telegramUserName)
                {
                    telegramChannel.ChannelId = item.Message.Chat.Id;
                    break;
                }
            }

            if (telegramChannel.ChannelId == 0)
            {
                throw new Exception($"Cannot find user with username: {telegramUserName} in contacts.");
            }

            var existingChannels = _context.TelegramChannels.Where(x => x.UserId == telegramChannel.UserId);
            if (existingChannels.Any())
            {
                _context.TelegramChannels.Remove(existingChannels.First());
            }

            _context.Add(telegramChannel);
            _context.SaveChanges();

            var message = new Message
            {
                Subject = "WELCOME",
                Body = "Welcome, this bot will be sending you notifications with proposed positions."
            };

            SendTelegramMessage(telegramChannel.ChannelId, message);
        }

        private static TelegramBotClient GetTelegramBotClient()
        {
            return new TelegramBotClient(Constants.TelegramApiToken);
        }
    }
}

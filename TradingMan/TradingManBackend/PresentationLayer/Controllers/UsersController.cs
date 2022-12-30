using Microsoft.AspNetCore.Mvc;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.BusinessLayer.Logic.Broker;
using TradingManBackend.BusinessLayer.Logic.Messaging;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.Controllers
{
    /// <summary>
    /// Controller class responsible for handling REST API calls from frontend related to
    /// user accounts and user account settings
    /// </summary>
    [ApiController]
    [Route("Users")]
    public class UsersController : Controller
    {
        private readonly UsersLogic _usersLogic;
        private readonly ILogger<UsersController> _logger;
        private readonly TelegramMessengerLogic _telegramMessenger;
        private readonly IBrokerLogic _brokerLogic;

        public UsersController(
            UsersLogic usersLogic,
            TelegramMessengerLogic telegramMessenger,
            ILogger<UsersController> logger,
            AlpacaLogic alpacaLogic)
        {
            _usersLogic = usersLogic;
            _telegramMessenger = telegramMessenger;
            _logger = logger;
            _brokerLogic = alpacaLogic;
        }

        /// <summary>
        /// Endpoint for registering new user
        /// </summary>
        /// <param name="dto">newUserDto object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateUser")]
        public IActionResult CreateUser([FromBody] NewUserDto newUserDto)
        {
            _logger.LogInformation($"Creating new user [{newUserDto}].");
            try
            {
                if (!_usersLogic.CreateUser(UserDtoHelper.FromNewUserDto(newUserDto)))
                {
                    return BadRequest("Email already registerd.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok("New user created.");
        }

        // TODO This is not a smart way to handle authorization

        /// <summary>
        /// Endpoint for verifying user login details
        /// </summary>
        /// <param name="dto">loginUserDto object</param>
        /// <returns></returns>
        [HttpPost]
        [Route("LoginUser")]
        public IActionResult LoginUser([FromBody] LoginUserDto loginUserDto)
        {
            _logger.LogInformation($"Verifying login infor for email: [{loginUserDto.Email}].");
            try
            {
                var userId = _usersLogic.VerifyUser(UserDtoHelper.FromLoginUserDto(loginUserDto));
                if (userId != Guid.Empty)
                {
                    _logger.LogInformation("User verified successfully.");
                    return Ok(userId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation("User login info incorrect.");
            return BadRequest("User login info incorrect.");
        }

        /// <summary>
        /// Endpoint that returns account settingsDto for provided userId
        /// </summary>
        /// <param name="userId">guid of user in question</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAccountSettings/{userId}")]
        public IActionResult GetAcountSettings([FromRoute] Guid userId)
        {
            _logger.LogInformation($"Recieved request to retrieve user account settings for userId: [{userId}].");
            AccountSettingsDto accountSettingsDto;
            try
            {
                accountSettingsDto = AccountSettingsDtoHelper.ToAccountSettingsDto(_usersLogic.GetAccountSettings(userId));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(accountSettingsDto);
        }

        /// <summary>
        /// Endpoint to set up new account settings. Overrwrites previously set useraccount settings.
        /// </summary>
        /// <param name="dto">NewAccountSettings dto to be added</param>
        /// <returns></returns>
        [HttpPost]
        [Route("SetAccountSettings")]
        public async Task<IActionResult> SetAccountSettings([FromBody] NewAccountSettingsDto newAccountSettingsDto)
        {
            _logger.LogInformation($"Setting new account settings for userId: [{newAccountSettingsDto.UserId}]");
            try
            {
                var accountSettings = AccountSettingsDtoHelper.FromNewAccountSettingsDto(newAccountSettingsDto);
                var brokerValidation = await _brokerLogic.Validate(accountSettings);
                if (!brokerValidation)
                {
                    _logger.LogError("Unable to validate broker details.");
                    return BadRequest("Unable to validate broker details.");
                }
                
                if (newAccountSettingsDto.UseTelegram)
                {
                    await _telegramMessenger.SetupTelegram(newAccountSettingsDto.TelegramUsername, newAccountSettingsDto.UserId);
                }

                _usersLogic.SetAccountSettings(accountSettings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
            
            return Ok();
        }
    }
}

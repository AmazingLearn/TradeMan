using TradingManBackend.DataLayer.Models;

/// <summary>
/// Contains classes for transfering user related objects between frontend and backend
/// </summary>
namespace TradingManBackend.PresentationLayer.Dtos
{
    // TODO 
    // Could use a base class here, but this should not
    // stay the same. Proper authentication should not be done in the way
    // this aplication is handling it. I therefore presume that all of
    // these objects will eventually diverge and work in a completely different way
    public class NewUserDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    public class LoginUserDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }

    /// <summary>
    /// Contains methods to help transform between user related data transfer objects and models 
    /// </summary>
    public class UserDtoHelper
    {
        public static User FromNewUserDto(NewUserDto newUserDto)
        {
            return new User
            {
                Email = newUserDto.Email,
                Password = newUserDto.Password,
            };
        }

        public static User FromLoginUserDto(LoginUserDto newUserDto)
        {
            return new User
            {
                Email = newUserDto.Email,
                Password = newUserDto.Password,
            };
        }
    }

    public class NewAccountSettingsDto
    {
        public Guid UserId { get; set; }
        public string AlpacaApiKey { get; set; } = "";
        public string AlpacaSecretKey { get; set; } = "";
        public string TelegramUsername { get; set; } = "";
        public bool UseTelegram { get; set; } = false;
    }

    public class AccountSettingsDto
    {
        public string AlpacaApiKey { get; set; } = "";
        public string TelegramUsername { get; set; } = "";

        public bool UseTelegram { get; set; } = false;
    }

    /// <summary>
    /// Contains methods to help transform between account settings related data transfer objects and models 
    /// </summary>
    public class AccountSettingsDtoHelper
    {
        public static AccountSettingsDto ToAccountSettingsDto(AccountSettings accountSettings)
        {
            return new AccountSettingsDto
            {
                AlpacaApiKey = accountSettings.AlpacaApiKey,
                TelegramUsername = accountSettings.TelegramUsername,
                UseTelegram = accountSettings.UseTelegram,
            };
        }

        public static AccountSettings FromNewAccountSettingsDto(NewAccountSettingsDto newAccountSettingsDto)
        {
            return new AccountSettings
            {
                UserId = newAccountSettingsDto.UserId,
                AlpacaApiKey = newAccountSettingsDto.AlpacaApiKey,
                AlpacaSecretKey = newAccountSettingsDto.AlpacaSecretKey,
                TelegramUsername = newAccountSettingsDto.TelegramUsername,
                UseTelegram = newAccountSettingsDto.UseTelegram,
            };
        }
    }
}

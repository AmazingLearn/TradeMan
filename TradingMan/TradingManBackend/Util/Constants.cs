namespace TradingManBackend.Util
{
    // TODO remove unused


    /// <summary>
    /// Contains aplication vide constants.
    /// </summary>
    public static class Constants
    {
        // Token for telegram channel, this should not be publicly available
        public static string TelegramApiToken = "5110840026:AAFSdbNgiQ18wkQtpFFW5nYu_WjL7wUdtC4";

        // These are my personal an need to change based on user - for developement only
        //public static string AlpacaPaperEndpoint = "https://paper-api.alpaca.markets"; //  This further has to be modifiable when creating account - paper vs live
        
        //public static string AlpacaApiKey = "PKBLETCG0OP9QT8FJU9E";
        //public static string AlpacasecretKey = "FT7niCKmXrSvAsi2850tKwTPIFqCWvMXVfai4F1k";

        // Alpha vantage API key, this should not be publicly available
        public static string AlphaVantageApiKey = "0OGYWABXRQK81LWC";

        // App Email info
        public static string GmailEmail = "trademan536@gmail.com";
        public static string GmailPassword = "vqgipxaydgsmjbbs";

        // TODO this should be properly done when the app is hosted somewhere
        public static string FrontendUrl = @"https://localhost:3000";

        // Just for testing 
        private static string telegramUserName = "@IvanTasler536";
    }    
}

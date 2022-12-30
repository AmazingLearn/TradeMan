namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Class representing message to be sent to user
    /// </summary>
    public class Message
    {
        public string Subject { get; set; } = "";
        public string Body { get; set; } = "";
    }
}

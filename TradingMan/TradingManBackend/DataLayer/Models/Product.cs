namespace TradingManBackend.DataLayer.Models
{
    /// <summary>
    /// Calss representing market product, e.g. stock
    /// </summary>
    public class Product
    {
        public string Symbol { get; set; }
        public string Name { get; set; }

        public Product (string symbol, string name)
        {
            Symbol = symbol;
            Name = name;
        }
    }
}

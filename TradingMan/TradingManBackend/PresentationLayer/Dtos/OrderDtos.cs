using TradingManBackend.DataLayer.Models;

/// <summary>
/// Contains classes for transfering order related objects between frontend and backend.
/// </summary>
namespace TradingManBackend.PresentationLayer.Dtos
{
    public class OrderDto
    {
        public Guid UserId { get; set; }
        public string ProductSymbol { get; set; }
        public OrderVolumeType OrderVolumeType { get; set; }
        public double OrderVolume { get; set; }
        public bool UseStopLoss { get; set; }
        public double StopLoss { get; set; }
        public bool UseTakeProfits { get; set; }
        public double TakeProfits { get; set; }
        public bool UsePaperAccount { get; set; }
    }

    /// <summary>
    /// Contains methods to help transform between order related data transfer objects and models 
    /// </summary>
    public class OrderDtoHelper
    {
        public static Order FromOrderDto(OrderDto orderDto)
        {
            return new Order
            {
                UserId = orderDto.UserId,
                ProductSymbol = orderDto.ProductSymbol,
                OrderVolumeType = orderDto.OrderVolumeType,
                OrderVolume = orderDto.OrderVolume,
                UseStopLoss = orderDto.UseStopLoss,
                StopLoss = orderDto.StopLoss,
                UseTakeProfits = orderDto.UseTakeProfits,
                TakeProfits = orderDto.TakeProfits,
                UsePaperAccount = orderDto.UsePaperAccount
            };
        }
    }
}

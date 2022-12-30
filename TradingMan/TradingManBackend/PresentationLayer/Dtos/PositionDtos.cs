using TradingManBackend.DataLayer.Models;

/// <summary>
/// Contains classes for transfering user related objects between frontend and backend
/// </summary>
namespace TradingManBackend.PresentationLayer.Dtos
{
    public class PositionDto
    {
        public Guid PositionId { get; set; }
        public Guid UserId { get; set; }
        public string ProductSymbol { get; set; }
        public PositionType PositionType { get; set; }
        public double BaseValue { get; set; }
        public double CurrentPrice { get; set; }
        public string NotificationName { get; set; }
    }

    /// <summary>
    /// Contains methods to help transform between position related data transfer objects and models 
    /// </summary>
    public class PositionsDtoHelper
    {
        public static PositionDto ToPositionDto(Position position, double currentPrice)
        {
            return new PositionDto
            {
                PositionId = position.PositionId,
                UserId = position.UserId,
                ProductSymbol = position.ProductSymbol,
                PositionType = position.PositionType,
                BaseValue = position.BaseValue,
                CurrentPrice = currentPrice,
                NotificationName = position.NotificationName
            };
        }

        public static Position FromPositionDto(PositionDto positionDto)
        {
            return new Position
            {
                PositionId = positionDto.PositionId,
                UserId = positionDto.UserId,
                ProductSymbol = positionDto.ProductSymbol,
                PositionType = positionDto.PositionType,
                BaseValue = positionDto.BaseValue,
                NotificationName = positionDto.NotificationName,
            };
        }
    }
}

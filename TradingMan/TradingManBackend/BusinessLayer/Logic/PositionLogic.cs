using TradingManBackend.DataLayer;
using TradingManBackend.DataLayer.Models;

namespace TradingManBackend.BusinessLayer.Logic
{
    /// <summary>
    /// Service for handling position logic from Position controller
    /// </summary>
    public class PositionLogic
    {
        private readonly DatabaseContext _context;
        private readonly ILogger<PositionLogic> _logger;

        public PositionLogic(
            DatabaseContext context,
            ILogger<PositionLogic> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// Returns all positions for userId
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<Position> GetAllPositions(Guid userId)
        {
            _logger.LogInformation($"Gettign all positions for userId: {userId}");
            return _context.Positions.Where(x => x.UserId == userId).ToList();
        }

        /// <summary>
        /// Returns position based on userId and positionId
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="positionId"></param>
        /// <returns></returns>
        public Position GetPosition(Guid userId, Guid positionId)
        {
            _logger.LogInformation($"Getting position for userId: [{userId}] and positionId: [{positionId}]");
            return _context.Positions.Where(x => x.UserId == userId && x.PositionId == positionId).FirstOrDefault();
        }

        /// <summary>
        /// Removes a position from DB
        /// </summary>
        /// <param name="position"></param>
        public void RemovePosition(Guid positionId)
        {
            _logger.LogInformation($"Removing position with positionId: {positionId}");
            var positionToRemove = _context.Positions.First(x => x.PositionId == positionId);
            _context.Remove(positionToRemove);
            _context.SaveChanges();
        }
    }
}

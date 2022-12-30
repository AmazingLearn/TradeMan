using Microsoft.AspNetCore.Mvc;
using TradingManBackend.BusinessLayer.Logic;
using TradingManBackend.PresentationLayer.Dtos;

namespace TradingManBackend.PresentationLayer.Controllers
{
    /// <summary>
    /// Controller class responsible for handling REST API calls from frontend related to
    /// noptifications and notification settings.
    /// </summary>
    [ApiController]
    [Route("Notifications")]
    public class NotificationsController : Controller
    {
        private readonly NotificationsLogic _notificationsLogic;
        private readonly ILogger<NotificationsController> _logger;

        public NotificationsController(NotificationsLogic notificationsLogic, ILogger<NotificationsController> logger)
        {
            _notificationsLogic = notificationsLogic;
            _logger = logger;
        }

        /// <summary>
        /// Endpoint returns all active notifications for user
        /// </summary>
        /// <param name="userId">user guid</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetNotifications/{userId}")]
        public IActionResult GetNotifications([FromRoute] Guid userId)
        {
            _logger.LogInformation($"Getting notifications for userId: [{userId}].");
            var notifications = new List<INotificationDto>();
            try
            {
                notifications = _notificationsLogic.GetNotifications(userId).Select(x => NotificationDtoHelper.ToNotificationDto(x)).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            _logger.LogInformation($"Returning grand total of: {notifications.Count} notifications.");
            return Ok(notifications);
        }

        /// <summary>
        /// Endpoint for getting single notification based on userId and notificationId
        /// </summary>
        /// <param name="userId">user guid</param>
        /// <param name="notificationId">notification id</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetSingleNotification/{userId}/{notificationId}")]
        public IActionResult GetSingleNotification([FromRoute] Guid userId, [FromRoute] int notificationId)
        {
            _logger.LogInformation($"Getting notification for userId: [{userId}] and notificationId: [{notificationId}].");
            INotificationDto notificationDto;
            try
            {
                notificationDto = NotificationDtoHelper.ToNotificationDto(_notificationsLogic.GetSingleNotification(userId, notificationId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok(notificationDto);
        }

        // TODO
        // To make this compact a rework of API controller is most liekly needed.
        // from here: https://stackoverflow.com/questions/23343328/webapi-controller-inheritance-and-attribute-routing

        /// <summary>
        /// Endpoint that creates a notificationBasic.
        /// </summary>
        /// <param name="newNotificationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateNotificationBasic")]
        public IActionResult CreateNotificationBasic([FromBody] NewNotificationBasicDto newNotificationDto)
        {
            _logger.LogInformation($"Creating new notification basic for userId: [{newNotificationDto.UserId}].");
            try
            {
                _notificationsLogic.CreateNotification(NotificationDtoHelper.FromNewNotificationDto(newNotificationDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Endpoint that creates a notificationTrend.
        /// </summary>
        /// <param name="newNotificationDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CreateNotificationTrend")]
        public IActionResult CreateNotificationTrend([FromBody] NewNotificationTrendDto newNotificationDto)
        {
            _logger.LogInformation($"Creating new notification trend for userId: [{newNotificationDto.UserId}].");
            try
            {
                _notificationsLogic.CreateNotification(NotificationDtoHelper.FromNewNotificationDto(newNotificationDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Endpoint to remove a notificationBasic from DB
        /// </summary>
        /// <param name="notificationDto">notificationDto object in JSON format</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveNotificationBasic")]
        public IActionResult RemoveNotification([FromBody] NotificationBasicDto notificationDto)
        {
            _logger.LogInformation($"Removing notification with notificationID: [{notificationDto.NotificationId}].");
            try
            {
                _notificationsLogic.RemoveNotification(NotificationDtoHelper.FromNotificationDto(notificationDto));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }

            return Ok();
        }

        /// <summary>
        /// Endpoint to remove a notificationTrend from DB
        /// </summary>
        /// <param name="notificationDto">notificationDto object in JSON format</param>
        /// <returns></returns>
        [HttpPost]
        [Route("RemoveNotificationTrend")]
        public IActionResult RemoveNotification([FromBody] NotificationTrendDto notificationDto)
        {
            _logger.LogInformation($"Removing notification with notificationID: [{notificationDto.NotificationId}].");
            try
            {
                _notificationsLogic.RemoveNotification(NotificationDtoHelper.FromNotificationDto(notificationDto));
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

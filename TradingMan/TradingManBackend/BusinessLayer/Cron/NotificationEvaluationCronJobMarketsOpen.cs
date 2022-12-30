using TradingManBackend.BusinessLayer.Cron;
using TradingManBackend.BusinessLayer.Logic;

// Inspired by https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06

namespace TradingManBackend.Cron
{
    /// <summary>
    /// Class responsible for running notification evaluation cron job. Vist link above to learn more.
    /// </summary>
    public class NotificationEvaluationCronJobMarketsOpen : CronJobService
    {
        private readonly ILogger<NotificationEvaluationCronJobMarketsOpen> _logger;
        private readonly IServiceScopeFactory _scopeFactory;


        public NotificationEvaluationCronJobMarketsOpen(IScheduleConfig<NotificationEvaluationCronJobMarketsOpen> config,
                                             ILogger<NotificationEvaluationCronJobMarketsOpen> logger,
                                             IServiceScopeFactory scopeFactory)
            : base(config.CronExpression, config.TimeZoneInfo)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NotificationEvaluation cron job for markets opening starts.");
            return base.StartAsync(cancellationToken);
        }

        public override Task DoWork(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{DateTime.Now:hh:mm:ss} NotificationEvaluationCronJobMarketsOpen is working.");
            
            using (var scope = _scopeFactory.CreateScope())
            {
                var notEval = scope.ServiceProvider.GetRequiredService<NotificationEvaluation>();

                notEval.EvaluateNotifications();
            }

            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("NotificationEvaluationCronJobMarketsOpen is stopping.");
            return base.StopAsync(cancellationToken);
        }
    }
}

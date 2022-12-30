
// Inspired by https://codeburst.io/schedule-cron-jobs-using-hostedservice-in-asp-net-core-e17c47ba06

namespace TradingManBackend.BusinessLayer.Cron
{
    public interface IScheduleConfig<T>
    {
        string CronExpression { get; set; }
        TimeZoneInfo TimeZoneInfo { get; set; }
    }

    public class ScheduleConfig<T> : IScheduleConfig<T>
    {
        public string CronExpression { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }
    }
}

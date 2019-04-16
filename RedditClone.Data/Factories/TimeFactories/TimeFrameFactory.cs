using RedditClone.Common.Enums.TimeFrameTypes;
using System;
using System.ComponentModel;

namespace RedditClone.Data.Factories.TimeFactories
{
    public static class TimeFrameFactory
    {
        public static TimeSpan GetTimeFrame(TimeFrameType timeFrameType)
        {
            switch (timeFrameType)
            {
                case TimeFrameType.PastHour:
                    return TimeSpan.FromHours(1);
                case TimeFrameType.PastDay:
                    return TimeSpan.FromDays(1);
                case TimeFrameType.PastWeek:
                    return TimeSpan.FromDays(7);
                case TimeFrameType.PastMonth:
                    return TimeSpan.FromDays(31);
                case TimeFrameType.PastYear:
                    return TimeSpan.FromDays(365);
                case TimeFrameType.AllTime:
                    return TimeSpan.FromDays(365 * 100);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}

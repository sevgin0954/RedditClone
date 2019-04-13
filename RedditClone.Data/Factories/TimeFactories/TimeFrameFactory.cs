using RedditClone.Common.Enums;
using System;
using System.ComponentModel;

namespace RedditClone.Data.Factories.TimeFactories
{
    public static class TimeFrameFactory
    {
        public static TimeSpan GetTimeFrame(PostShowTimeFrame postShowTimeFrame)
        {
            switch (postShowTimeFrame)
            {
                case PostShowTimeFrame.PastHour:
                    return TimeSpan.FromHours(1);
                case PostShowTimeFrame.PastDay:
                    return TimeSpan.FromDays(1);
                case PostShowTimeFrame.PastWeek:
                    return TimeSpan.FromDays(7);
                case PostShowTimeFrame.PastMonth:
                    return TimeSpan.FromDays(31);
                case PostShowTimeFrame.PastYear:
                    return TimeSpan.FromDays(365);
                case PostShowTimeFrame.AllTime:
                    return TimeSpan.FromDays(365 * 100);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}

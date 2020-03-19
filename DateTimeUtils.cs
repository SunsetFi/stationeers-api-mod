
using System;

namespace WebAPI
{
    public static class DateTimeUtils
    {
        private static readonly long EPOCH_TICKS = 621355968000000000;
        private static readonly long TICKS_PER_MS = 10000;
        public static long ToJavascriptTimestamp(this DateTime dateTime)
        {
            return DateTimeUtils.TicksToJavascriptTimestamp(dateTime.Ticks);
        }

        public static long TicksToJavascriptTimestamp(long ticks)
        {
            return (ticks - DateTimeUtils.EPOCH_TICKS) / DateTimeUtils.TICKS_PER_MS;
        }
    }
}
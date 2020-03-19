
namespace WebAPI.Payloads
{
    public class BanPayload
    {
        public string steamId { get; set; }
        public long endTimestamp { get; set; }

        public static BanPayload FromBanManagerData(ulong steamId, string length)
        {
            // BanManager stores strings, and stores the raw string of the value if the numberized version is <= 0
            // We just return 0 in that case, as it is essentially a forever ban.
            long ticks = 0;
            long.TryParse(length, out ticks);

            // From .Net Docs:
            //  A single tick represents one hundred nanoseconds or one ten-millionth of a second.
            //  There are 10,000 ticks in a millisecond, or 10 million ticks in a second.
            return new BanPayload()
            {
                steamId = steamId.ToString(),
                endTimestamp = DateTimeUtils.TicksToJavascriptTimestamp(ticks)
            };
        }
    }
}
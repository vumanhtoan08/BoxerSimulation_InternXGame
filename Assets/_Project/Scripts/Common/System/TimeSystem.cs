using System;
using System.Collections;
using System.Collections.Generic;
// using mygame.sdk;
using UnityEngine;

public class TimeSystem : Singleton<TimeSystem>
{
    public static DateTime Now()
    {
        return TimestampToDateTime(GetUtcTime());
    }
    public static DateTime TimestampToDateTime(long timstamp)
    {
        return TicksToDateTime(TimestampToTicks(timstamp));
    }
    public static long GetUtcTime()
    {
#if ENABLE_CHEAT
        DateTime unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        return (long)(DateTime.UtcNow - unixEpoch).TotalMilliseconds;
#endif
        // return GameHelper.CurrentTimeMilisReal();
        return 0;
    }
    public static DateTime TicksToDateTime(long ticks)
    {
        return new DateTime(ticks);
    }
    public static long TimestampToTicks(long timestamp)
    {
        return timestamp * 10000 + 621355968000000000;
    }
    public static long TicksToTimestamp(long ticks)
    {
        return (ticks - 621355968000000000) / 10000;
    }
}

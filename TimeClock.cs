using Microsoft.AspNetCore.Authentication;
using System;
using System.Diagnostics;
using robot_controller_api;
public class TimeClock : TimeProvider, ISystemClock

{
    public override long GetTimestamp()
    {
        // Optional: Use high-resolution timestamp for performance metrics
        return Stopwatch.GetTimestamp();
    }

    public override DateTimeOffset GetUtcNow()
    {
        return DateTimeOffset.UtcNow;
    }

    // For ISystemClock (legacy ASP.NET auth)
    public DateTimeOffset UtcNow => GetUtcNow();
}

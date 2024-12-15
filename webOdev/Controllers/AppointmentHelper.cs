public static class AppointmentHelper
{
    public static List<TimeSpan> GetAvailableTimes()
    {
        return new List<TimeSpan>
        {
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            new TimeSpan(12, 0, 0),
            new TimeSpan(13, 0, 0),
            new TimeSpan(14, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(16, 0, 0),
            new TimeSpan(17, 0, 0)
        };
    }
}

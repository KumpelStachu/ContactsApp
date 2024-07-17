namespace api.Utils
{
    /// <summary>
    /// Extension methods for DateTime objects.
    /// </summary>
    /// <remarks>
    /// This class contains extension methods for DateTime objects.
    /// </remarks>
    public static class DateTimeExtensions
    {
        /// <summary>
        /// Sets the kind of a DateTime object to UTC.
        /// </summary>
        /// <param name="dateTime">The DateTime object to set the kind of.</param>
        /// <returns>The DateTime object with the kind set to UTC.</returns>
        public static DateTime SetKindUtc(this DateTime dateTime)
        {
            if (dateTime.Kind == DateTimeKind.Utc) { return dateTime; }
            return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }
    }
}
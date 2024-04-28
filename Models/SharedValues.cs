namespace ThreeFriends.Models
{
    public static class SharedValues
    {
        public static User CurUser = new User();
        public static DateTime DataRangeFrom = default(DateTime);
        public static DateTime DataRangeTo = default(DateTime);
        public static string Currency= null;
        public static DateTime DeleteOldTo = default(DateTime);
    }
}

namespace ThreeFriends.Models
{
    public static class SharedValues
    {
        public static User CurUser = new User();
        public static DateTime DataRangeFrom = default(DateTime);
        public static DateTime DataRangeTo = default(DateTime);
        public static string Currency = null;
        public static DateTime DeleteOldTo = default(DateTime);

        public static void SetAllNull()
        {
            SharedValues.CurUser = new User();
            SharedValues.DataRangeFrom = default(DateTime);
            SharedValues.DataRangeTo = default(DateTime);
            SharedValues.Currency = null;
            SharedValues.DeleteOldTo = default(DateTime);
        }
    }
}

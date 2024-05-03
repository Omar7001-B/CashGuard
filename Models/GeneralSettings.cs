namespace ThreeFriends.Models
{
    public static class GeneralSettings
    {
        public static DateTime DataRangeFrom = default(DateTime);
        public static DateTime DataRangeTo = default(DateTime);
        public static string Currency = null;
        public static DateTime DeleteOldTo = default(DateTime);

        public static void SetAllNull()
        {
            GeneralSettings.DataRangeFrom = default(DateTime);
            GeneralSettings.DataRangeTo = default(DateTime);
            GeneralSettings.Currency = null;
            GeneralSettings.DeleteOldTo = default(DateTime);
        }

        //public static void SetValues(DateTime DataRangeFrom, DateTime DateRangeTo, string Currency, DateTime DeleteOldTo)
        //{

        //    GeneralSettings.DataRangeFrom = DataRangeFrom;
        //    GeneralSettings.DataRangeTo = DateRangeTo;
        //    GeneralSettings.Currency = Currency;
        //    GeneralSettings.DeleteOldTo = DeleteOldTo;
        //}
    }
}

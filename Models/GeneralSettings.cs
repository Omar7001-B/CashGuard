namespace ThreeFriends.Models
{
    public static class GeneralSettings
    {
        public static DateTime DataRangeFrom = default(DateTime);
        public static DateTime DataRangeTo = default(DateTime);
        public static string Currency = null;
        public static DateTime DeleteOldTo = default(DateTime);
        public static List<Transaction> Transactions = new List<Transaction>();
        public static List<HistoryItem> history = new List<HistoryItem>();
        // we will not delete anything from the history
        public static void SetAllNull()
        {
            GeneralSettings.DataRangeFrom = DateTime.MinValue;
            GeneralSettings.DataRangeTo = DateTime.MaxValue;
            GeneralSettings.Currency = null;
            GeneralSettings.DeleteOldTo = default(DateTime);
        }

        public static void SetTransactions()
        {
            Appdbcontxt _context = new Appdbcontxt();
            Transactions = _context.Transactions.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();
        }

        public static void SetHistoryItems()
        {
            Appdbcontxt _context = new Appdbcontxt();
            history = _context.History.Where(t => t.UserId == SharedValues.CurUser.Id).ToList();
        }

        public static void UpdateTransactions()
        {
            List<Transaction> NewTransactions = new List<Transaction>();
            foreach (var transaction in GeneralSettings.Transactions)
            {
                if (transaction.Timestamp < GeneralSettings.DeleteOldTo)
                {
                    NewTransactions.Add(transaction);
                }
            }
            using (var _context = new Appdbcontxt())
            {
                _context.Transactions.RemoveRange(GeneralSettings.Transactions);
                _context.SaveChanges();

                _context.Transactions.AddRange(NewTransactions);
                _context.SaveChanges();

                GeneralSettings.Transactions = NewTransactions;
            }
        }

        public static void UpdateHistory()
        {
            List<HistoryItem> NewHistory = new List<HistoryItem>();
            foreach (var hstory in GeneralSettings.history)
            {
                if (hstory.Timestamp < GeneralSettings.DeleteOldTo)
                {
                    NewHistory.Add(hstory);
                }
            }
            using (var _context = new Appdbcontxt())
            {
                _context.History.RemoveRange(GeneralSettings.history);
                _context.SaveChanges();

                _context.History.AddRange(NewHistory);
                _context.SaveChanges();
                GeneralSettings.history = NewHistory;
            }
        }



    }
}

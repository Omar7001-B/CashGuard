namespace ThreeFriends.Models
{
    public static class SharedValues
    {
        public static User CurUser = null;
        public static string Dashboard  = "";
        public static string Tranaction = "";
        public static string AddCat     = "";
        public static string Analytics  = "";
        public static string History    = "";
        public static string Account    = "";
        public static string Settings   = "";

        public static void setHover(string hover)
        {
            Dashboard     = "";
            Tranaction    = "";
            AddCat        = "";
            Analytics     = "";
            History       = "";
            Account       = "";
            Settings      = "";
            switch (hover)
            {
                case "Dashboard":
                    Dashboard = "hovered";
                    break;
                case "Tranaction":
                    Tranaction = "hovered";
                    break;
                case "AddCat":
                    AddCat = "hovered";
                    break;
                case "Analytics":
                    Analytics = "hovered";
                    break;
                case "History":
                    History = "hovered";
                    break;
                case "Account":
                    Account = "hovered";
                    break;
                case "Settings":
                    Settings = "hovered";
                    break;
            }
        }
 
    }
}

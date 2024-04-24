namespace ThreeFriends.Models
{
    public static class SharedValues
    {
        public static User CurUser;
        private static Appdbcontxt entity;

        public static void ini()
        {
            CurUser = null;
        }

        public static bool IsUser(string UserName, string Password)
        {
            entity = new Appdbcontxt();
            CurUser = entity.Users.FirstOrDefault(u => u.User_Name == UserName && u.Password == Password);
            return CurUser != null;
        }
        public static void SetCurUser(string UserName, string Password)
        {
            if (!IsUser(UserName, Password))
            {
                CurUser = null;
            }
        }
    }
}

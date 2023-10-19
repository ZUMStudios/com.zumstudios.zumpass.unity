using System;

namespace com.zumstudios.zumpass
{
    [Serializable]
    public class ZUMPassUser
    {
        public string email;
        public string password;
        public string token;

        public ZUMPassUser() { }

        public ZUMPassUser(string email, string password, string token)
        {
            this.email = email;
            this.password = password;
            this.token = token;
        }

        public void Save() => DataHelper.Save<ZUMPassUser>(this, ZUMPassConstants.ZUMPASS_USER_FILENAME);

        public static ZUMPassUser Load() => DataHelper.Load<ZUMPassUser>(ZUMPassConstants.ZUMPASS_USER_FILENAME);

        public static void Delete() => DataHelper.Delete(ZUMPassConstants.ZUMPASS_USER_FILENAME);
    }
}


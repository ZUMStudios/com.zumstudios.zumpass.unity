using System;
using Newtonsoft.Json;

namespace com.zumstudios.zumpass
{
    [Serializable]
    public class ZUMPassLogin
    {
        public string email;
        public string password;

        public ZUMPassLogin() {  }

        public ZUMPassLogin(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}


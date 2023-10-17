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

    [Serializable]
    public class ZUMPassLoginResponse
    {
        public bool success;
        public string msg;
        public string token;

        public ZUMPassLoginResponse() { }
    }

    [Serializable]
    public class ZUMPassData
    {
        public string token;
        public string application_key;

        public ZUMPassData() { }

        public ZUMPassData(string token, string application_key)
        {
            this.token = token;
            this.application_key = application_key;
        }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}


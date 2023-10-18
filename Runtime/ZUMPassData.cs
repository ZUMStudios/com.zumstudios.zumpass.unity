using System;
using Newtonsoft.Json;

namespace com.zumstudios.zumpass
{
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

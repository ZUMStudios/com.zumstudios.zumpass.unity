using System;

namespace com.zumstudios.zumpass
{
    [Serializable]
    public class ZUMPassLoginResponse
    {
        public bool success;
        public string msg;
        public string token;

        public ZUMPassLoginResponse() { }
    }
}
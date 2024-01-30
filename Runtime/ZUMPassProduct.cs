using System;

namespace com.zumstudios.zumpass
{
    [Serializable]
    public class ZUMPassProduct
    {
        public string title;
        public string code;
        public string product_id;
        public long expire_timestamp;

        public ZUMPassProduct() {  }

        public bool HasExpired()
        {
            if (GetExpireDate() < DateTime.UtcNow)
                return true;

            return false;
        }

        public DateTime GetExpireDate() => DataHelper.GetDateTimeFromTimestamp(expire_timestamp);

        public string GetHumanReadableExpireDate()
        {
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(GetExpireDate(), TimeZoneInfo.Local);
            return localTime.ToString("dd/MM/yyyy HH:mm");
        }
    }
}


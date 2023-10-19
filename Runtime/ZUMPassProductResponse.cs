using System;
using System.Collections.Generic;

namespace com.zumstudios.zumpass
{
    [Serializable]
    public class ZUMPassProductResponse
    {
        public bool success;
        public string msg;
        public List<ZUMPassProduct> products;

        public ZUMPassProductResponse() { }

        public void Save() => DataHelper.Save<ZUMPassProductResponse>(this, ZUMPassConstants.ZUMPASS_PRODUCTS_FILENAME);

        public static ZUMPassProductResponse Load() => DataHelper.Load<ZUMPassProductResponse>(ZUMPassConstants.ZUMPASS_PRODUCTS_FILENAME);

        public static void Delete() => DataHelper.Delete(ZUMPassConstants.ZUMPASS_PRODUCTS_FILENAME);
    }
}


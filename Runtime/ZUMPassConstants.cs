namespace com.zumstudios.zumpass {
    public static class ZUMPassConstants {
        public const string BASE_ENDPOINT = "https://www.zumpass.com.br/app/";
        public const string LOGIN_ENDPOINT = BASE_ENDPOINT + "login/";
        public const string REDEEM_ENDPOINT = BASE_ENDPOINT + "promocode/redeem/";
        public const string PRODUCTS_ENDPOINT = BASE_ENDPOINT + "promocode/retrieve/";

        public const string ZUMPASS_USER_FILENAME = "zumpass_user.bin";
        public const string ZUMPASS_PRODUCTS_FILENAME = "zumpass_products.bin";

        public const string ZUMPASS_URL = "https://www.zumpass.com.br";
        public const string ZUMPASS_DESCRIPTION_URL = "https://www.zumpass.com.br/oqueeozumpass/";
        public const string ZUMPASS_RESET_PASSWORD_URL = "https://www.zumpass.com.br/reset_password/";
    }
}
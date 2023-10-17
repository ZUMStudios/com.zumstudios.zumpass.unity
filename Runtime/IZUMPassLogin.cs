namespace com.zumstudios.zumpass
{
    public interface IZUMPassLogin
    {
        public void OnLoginSuccess(ZUMPassUser user);
        public void OnLoginFail(string message);
    }
}

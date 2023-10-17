using System.Collections.Generic;

namespace com.zumstudios.zumpass
{
    public interface IZUMPassProductLoad
    {
        public void OnProductLoadSuccess(List<ZUMPassProduct> products);
        public void OnProductLoadFail(string message);
    }
}


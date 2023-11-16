using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.zumstudios.zumpass 
{
    public static class ZUMPassShortcuts 
    {
        public static void OpenZUMPassSite() 
        {
            Application.OpenURL(ZUMPassConstants.ZUMPASS_URL);
        }

        public static void OpenZUMPassDescription() 
        {
            Application.OpenURL(ZUMPassConstants.ZUMPASS_DESCRIPTION_URL);
        }

        public static void OpenResetPasswordURL()
        {
            Application.OpenURL(ZUMPassConstants.ZUMPASS_RESET_PASSWORD_URL);
        }
    }
}
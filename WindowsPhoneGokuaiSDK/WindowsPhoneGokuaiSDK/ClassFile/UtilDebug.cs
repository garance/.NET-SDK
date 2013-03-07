using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 输出日志
    /// </summary>
    class UtilDebug
    {
        #if DEBUG
        private static bool _isDebug=true;
        #endif

        public static void ShowDebug(string value,string method,string where)
        {
            if (_isDebug)
            {
                Debug.WriteLine("-----------Debug------------");
                Debug.WriteLine("-----------" + where + "------------");
                Debug.WriteLine("-----------" + method + "------------");
                Debug.WriteLine("===>"+value);
            }
            
        }

        public static void ShowException(Exception e,string method,string where) 
        {
            if (_isDebug)
            {
                Debug.WriteLine("-----------Exception------------");
                Debug.WriteLine("-----------" + where + "------------");
                Debug.WriteLine("-----------" + method + "------------");
                Debug.WriteLine("===>" + e.Message);
            }
        }

    }
}

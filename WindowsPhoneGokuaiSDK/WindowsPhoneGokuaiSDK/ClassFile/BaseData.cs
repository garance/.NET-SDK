using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
   /// <summary>
   /// 网络返回数据
   /// </summary>
    public class BaseData
    {
        private const string LOG_TAG = "BaseData";
        protected const string KEY_ERROR_CODE = "error_code";
        protected const string KEY_ERROR_MSG = "error_msg";

        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode
        {
             get;
            protected set;
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMessage
        {
             get;
            protected set;
        }

        /// <summary>
        /// 返回数据读取
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="isError">是否错误</param>
        /// <returns></returns>
        public static BaseData Create(string jsonString,bool isError){
            if (jsonString == null)
            {
                return null;
            }

            BaseData data = new BaseData();
            try
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                if (isError)
                {
                    data.ErrorCode = Convert.ToInt32(json[KEY_ERROR_CODE]);
                    data.ErrorMessage = (string)json[KEY_ERROR_MSG];
                }
                return data;

            }
            catch (Exception e)
            {
                UtilDebug.ShowException(e, "SimpleJson.DeserializeObject", LOG_TAG);
                return null;

            }

        }
    }
}

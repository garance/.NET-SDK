using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WindowsPhoneGokuaiSDK.ClassFile;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 认证信息
    /// </summary>
    public class OauthData:BaseData
    {
        private const string LOG_TAG = "OauthData";
        private const string KEY_TOKEN = "token";

        /// <summary>
        /// 登陆token
        /// </summary>
        public string Token
        {
            get;
            private set;
        }



        /// <summary>
        /// 从json获取oauth数据
        /// </summary>
        /// <param name="jsonString">json字符串</param>
        /// /// <param name="isError">是否错误</param>
        /// <returns></returns>
       new  public static OauthData Create(string jsonString, bool isError)
        {
            if (jsonString == null)
            {
                return null;
            }

            OauthData data = new OauthData();
            try
            {
                var json = (IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                if (isError)
                {
                    var code = json[KEY_ERROR_CODE];
                    data.ErrorCode = int.Parse(code.ToString());
                    data.ErrorMessage = (string)json[KEY_ERROR_MSG];
                }
                else
                {
                    data.Token = (string)json[KEY_TOKEN];
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
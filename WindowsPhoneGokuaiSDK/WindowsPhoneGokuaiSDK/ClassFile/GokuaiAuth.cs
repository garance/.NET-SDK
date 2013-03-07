using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// Oauth认证
    /// </summary>
    public class GokuaiAuth
    {
        private const string URL_OAUTH = "http://a.gokuai.com/app_token";

        private string _clientId;
        private string _clientSecret;

        /// <summary>
        /// 未申请clientId
        /// 使用的认证构造函数
        /// </summary>
        public GokuaiAuth()
        {

        }
        /// <summary>
        /// 已申请clientid
        /// 使用的认证构造函数
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public GokuaiAuth(string clientId,string clientSecret)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
        }
        /// <summary>
        /// 获取token
        /// </summary>
        /// <param name="email"></param>
        /// <param name="handler">回调handler</param>
        public void token(string email, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestUrl = URL_OAUTH;
            request.AppendParameter("email",email);
            request.AppendParameter("client_id", _clientId);
            request.AppendParameter("client_secret", _clientSecret);
            request.RequestMethod = RequestType.POST;
            request.Request();
        }
    }
}

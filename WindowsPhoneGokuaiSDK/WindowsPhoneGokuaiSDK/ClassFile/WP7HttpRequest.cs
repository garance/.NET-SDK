using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using WindowsPhoneGokuaiSDK.ClassFile;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// winphone7的HTTP封装类
    /// </summary>
    public class WP7HttpRequest
    {
        #region 私有成员

        private const string USER_AGENT = "GoKuai_SAClient V_0.1";
        private string _request_url = null;
        private RequestType _request_type;
        private string _request_content=null;
        IDictionary<string, string> _parameter;
        IDictionary<string, string> _headParameter;

        /// <summary>
        /// 提交的内容 string
        /// </summary>
        public string RequetContent
        {
            get { return _request_content; }
            set { _request_content = value; }
        }

        /// <summary>
        /// 上传流数据
        /// </summary>
        public Stream UploadStream
        {
            set;
            private get;

        }


        /// <summary>
        /// 下载路径
        /// </summary>
        public string DownloadPath
        {
            set;
            private get;

        }

        #endregion

        /// <summary>
        /// Http请求指代
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">发送所带的参数</param>
        public delegate void HttpResquestEventHandler(object sender, WP7HttpEventArgs e);

        /// <summary>
        /// Http请求完成事件
        /// </summary>
        public event HttpResquestEventHandler _httpCompleted;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <remarks>
        /// 默认的请求方式的GET
        /// </remarks>
        public WP7HttpRequest()
        {
            _request_url = "";
            _parameter = new Dictionary<string, string>();
            _headParameter=new Dictionary<string,string>();
            _request_type = RequestType.GET; //默认请求方式为GET方式
        }

        /// <summary>
        /// 追加参数
        /// </summary>
        /// <param name="key">进行追加的键</param>
        /// <param name="value">键对应的值</param>
        public void AppendParameter(string key, string value)
        {
            _parameter.Add(key, value);
        }

        /// <summary>
        /// 追加头参数
        /// </summary>
        /// <param name="key">追加键</param>
        /// <param name="value">键对应的值</param>
        public void AppendHeaderParameter(string key,string value) {
            _headParameter.Add(key, value);
        }



        /// <summary>
        /// 触发HTTP请求完成方法
        /// </summary>
        /// <param name="e">事件参数</param>
        public void OnHttpCompleted(WP7HttpEventArgs e)
        {
            if (this._httpCompleted != null)
            {
                this._httpCompleted(this, e);
            }
        }

        /// <summary>
        /// 请求URL地址
        /// </summary>
        public string RequestUrl
        {
            get { return _request_url; }
            set { _request_url = value; }
        }

        /// <summary>
        /// 请求方式
        /// </summary>
        public RequestType RequestMethod
        {
            get { return _request_type; }
            set { _request_type = value; }
        }

        /// <summary>
        /// 进行HTTP请求
        /// </summary>
        public void Request()
        {
            if (this.RequestMethod == RequestType.GET)
            {
                this.GetRequest();
            }
            else if (this.RequestMethod == RequestType.POST)
            {
                this.PostRequest();
            }
            else
            {
                this.PutRequest();
            }
        }

        /// <summary>
        /// HTTP方式的GET请求
        /// </summary>
        /// <returns></returns>
        private void GetRequest()
        {
            string strrequesturl = this.RequestUrl;
            string parastring = this.getParemeterString();
            if (parastring.Length > 0)
            {
                strrequesturl += "?" + parastring;
            }
            Uri myurl = new Uri(strrequesturl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.Method = "GET";
            webRequest.UserAgent = USER_AGENT;
            webRequest.BeginGetResponse(new AsyncCallback(handleResponse), webRequest); //直接获取响应
            _parameter.Clear(); //清空参数列表
        }

        /// <summary>
        /// HTTP的POST请求
        /// </summary>
        /// <returns></returns>
        private void PostRequest()
        {
            Uri myurl = new Uri(this.RequestUrl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            webRequest.UserAgent = USER_AGENT;
            webRequest.BeginGetRequestStream(new AsyncCallback(handlePostReady), webRequest);
        }

        /// <summary>
        /// HTTP的PUT请求
        /// </summary>
        private void PutRequest() {
            Uri myurl = new Uri(this.RequestUrl);
            HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(myurl);
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "PUT";
            webRequest.UserAgent = USER_AGENT;
            webRequest.BeginGetRequestStream(new AsyncCallback(handlePutReady), webRequest);
        }

        /// <summary>
        /// 获取传递参数的字符串
        /// </summary>
        /// <returns>字符串</returns>
        private string getParemeterString()
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            bool hasParameter = false;
            string value = "";
            foreach (var item in _parameter)
            {
                if (!hasParameter)
                    hasParameter = true;
                value = UrlEncoder.Encode(item.Value); //对传递的字符串进行编码操作
                sb.Append(string.Format("{0}={1}&", item.Key, value));
            }
            if (hasParameter)
            {
                result = sb.ToString();
                int len = result.Length;
                result = result.Substring(0, --len); //将字符串尾的‘&’去掉
            }
            return result;

        }


        /// <summary>
        /// 异步请求回调函数
        /// </summary>
        /// <param name="asyncResult">异步请求参数</param>
        private void handlePostReady(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
            using (Stream stream = webRequest.EndGetRequestStream(asyncResult))
            {

                using (StreamWriter writer = new StreamWriter(stream))
                {
                    string parameterstring = this.getParemeterString();
                    if (parameterstring.Length > 0)
                    {
                        writer.Write(this.getParemeterString());
                        writer.Flush();
                    }

                    if (_request_content != null)
                    {
                        writer.Write(_request_content);
                        writer.Flush();
                    }
                }


            }

            webRequest.BeginGetResponse(new AsyncCallback(handleResponse), webRequest);
            _parameter.Clear();//清空参数列表
        }

        /// <summary>
        /// 异步请求回调函数
        /// </summary>
        /// <param name="asyncResult">异步请求参数</param>
        private void handlePutReady(IAsyncResult asyncResult)
        {
            HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
            using (Stream stream = webRequest.EndGetRequestStream(asyncResult))
            {
                if (UploadStream != null)
                {
                    byte[] bytes = Util.ReadToEnd(UploadStream);
                    stream.Write(bytes, 0, bytes.Length);
                    stream.Flush();
                }
            }

            webRequest.BeginGetResponse(new AsyncCallback(handleResponse), webRequest);
            _parameter.Clear();//清空参数列表
        }

        /// <summary>
        /// 异步响应回调函数
        /// </summary>
        /// <param name="asyncResult">异步请求参数</param>
        private void handleResponse(IAsyncResult asyncResult)
        {
            string result = "";
            bool iserror = false;
            try
            {
                HttpWebRequest webRequest = asyncResult.AsyncState as HttpWebRequest;
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.EndGetResponse(asyncResult);
                Stream streamResult = webResponse.GetResponseStream(); //获取响应流
                StreamReader reader = new StreamReader(streamResult);
                result = reader.ReadToEnd();

                //if (RequestMethod == RequestType.GET && DownloadPath != null)
                //{
                //    IsolatedStorageFile myStore = IsolatedStorageFile.GetUserStoreForApplication();
                //    myStore.CreateDirectory(Util.getParentPath(Util.ROOT_PATH+DownloadPath.Replace("/","\\"),"\\").Trim());
                    
                //    byte[] bytes = Util.ReadToEnd(streamResult);
                //    using (IsolatedStorageFileStream stream = myStore.CreateFile(Util.ROOT_PATH+DownloadPath.Replace("/","\\")))
                //    {
                //        stream.Write(bytes, 0, bytes.Length);
                //    }
 
                //}
            }
            catch (WebException ex)
            {
                HttpWebResponse response = ((HttpWebResponse)ex.Response);
                int code = (int)response.StatusCode;
                    try
                    {
                        using (Stream stream = response.GetResponseStream())
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                result = reader.ReadToEnd();
                            }
                        }
                    }
                    catch (WebException e)
                    {
                        UtilDebug.ShowException(e,null,null);
                    }

                iserror = true;
            }
            finally
            {
                WP7HttpEventArgs e = new WP7HttpEventArgs();
                e.IsError = iserror;
                e.Result = result;
                //进行异步回调操作
                System.Windows.Deployment.Current.Dispatcher.BeginInvoke(delegate()
                {

                    OnHttpCompleted(e);

                });
            }
        }
    }

    /// <summary>
    /// 枚举请求类型
    /// </summary>
    public enum RequestType
    {
        /// <summary>
        /// GET请求
        /// </summary>
        GET,

        /// <summary>
        /// POST请求
        /// </summary>
        POST,
        /// <summary>
        /// PUT请求
        /// </summary>
        PUT
    }
}

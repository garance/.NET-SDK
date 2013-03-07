using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    public class GokuaiClient
    {
        private const string LOG_TAG = "GokuaiClient";
        private const string URL_API = "http://sa.gokuai.com";
        private const string SEPERATOR = "/";

        private const string HEAD_TYPE_FILE = "file";
        private const string HEAD_TYPE_PRE = "pre";
        private const string HEAD_TYPE_THUMB = "thumb";

        public const int OVERWRITE_TYPE_OVERWRITE = 0;
        public const int OVERWRITE_TYPE_CREATE_NEW_FILE = 1;

        private string _clientSecret;
        private string _token;

        public GokuaiClient(string token,string clientSecret)
        {
            _token = token;
            _clientSecret = clientSecret;

        }

        /// <summary>
        /// 新建文件夹
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="filehash">文件hash</param>
        /// <param name="overwrite">上传有同名文件是否覆盖,不传默认0， 0覆盖，1新建</param>
        /// <param name="handler"></param>
        public void CreateFolder(string path, string mount, string filehash, int overwrite, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            Add(path, mount, filehash, overwrite, null, handler);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="filehash">文件hash</param>
        /// <param name="overwrite">上传有同名文件是否覆盖,不传默认0 0覆盖，1新建</param>
        /// <param name="stream"></param>
        /// <param name="handler"></param>
        public void UploadFile(string path, string mount, string filehash, int overwrite, Stream stream, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            Add(path, mount, filehash, overwrite, stream, handler);
        }

        /// <summary>
        /// 新增文件、文件夹
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="filehash">文件hash</param>
        /// <param name="overwrite">上传有同名文件是否覆盖,不传默认0 0覆盖，1新建</param>
        /// <param name="stream"></param>
        /// <param name="handler"></param>
        private void Add(string path, string mount, string filehash, int overwrite, Stream stream, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            long filesize = 0;
            if (stream != null)
            {
                filesize = stream.Length;
                if (filesize > 20 * 1024 * 1024)
                {
                    WP7HttpEventArgs e = new WP7HttpEventArgs();
                    e.Result = "上传文件超出20MB";
                    e.IsError = true;
                    handler.Invoke(this, e);
                    return;
 
                }
            }

            string signMethod = "add";
            string sign = GenerateSign(signMethod,path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList=new List<KeyValuePair<string,string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath));
            paramList.Add(new KeyValuePair<string, string>("filehash", filehash));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("filesize", filesize.ToString()));
            paramList.Add(new KeyValuePair<string, string>("overwrite", overwrite.ToString()));
            paramList.Add(new KeyValuePair<string, string>("sign", UrlEncoder.Encode(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.PUT;
            request.UploadStream = stream;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();

        }

        /// <summary>
        /// 获取文件信息
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="handler"></param>
        public void GetFileInfo(string path,string mount, WP7HttpRequest.HttpResquestEventHandler handler) 
        {
            string signMethod = "get";
            string sign = GenerateSign(signMethod, path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("mount", mount));
            paramList.Add(new KeyValuePair<string, string>("sign", UrlEncoder.Encode(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.GET;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();

        }

        /// <summary>
        /// 获取文件列表
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="start">默认为0，返回100条数据</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="handler"></param>
        public void GetFileList(string path, int start,string mount, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            string signMethod = "list";
            string sign = GenerateSign(signMethod, path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("mount", mount));
            paramList.Add(new KeyValuePair<string, string>("start", start.ToString()));
            paramList.Add(new KeyValuePair<string, string>("sign", UrlEncoder.Encode(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.GET;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();
        }

        /// <summary>
        /// 应用复制
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="frompath">源路径 文件夹后跟“/”</param>
        /// <param name="overwrite">上传有同名文件是否覆盖,不传默认0， 0覆盖，1新建</param>
        /// <param name="handler"></param>
        public void Copy(string path, string frompath, string mount,int overwrite, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            string signMethod = "copy";
            string sign = GenerateSign(signMethod, path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("mount", mount));
            paramList.Add(new KeyValuePair<string, string>("frompath", frompath));
            paramList.Add(new KeyValuePair<string, string>("overwrite", overwrite.ToString()));
            paramList.Add(new KeyValuePair<string, string>("sign", UrlEncoder.Encode(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.POST;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();
        }

        /// <summary>
        /// 缩略图302地址
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="handler"></param>
        public void GetThumbUrl(string path, string mount, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            Head(path,mount,HEAD_TYPE_THUMB,handler);
        }

        /// <summary>
        /// 文件302地址
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount"></param>
        /// <param name="handler"></param>
        public void GetFileUrl(string path, string mount, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            Head(path, mount, HEAD_TYPE_FILE, handler);
        }

        /// <summary>
        /// 预览302地址
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="handler"></param>
        public void GetPreviewUrl(string path, string mount, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            Head(path, mount, HEAD_TYPE_PRE, handler);
        }

        /// <summary>
        /// 获取路径
        /// </summary>
        /// <param name="path">文件路径，文件夹后跟“/”</param>
        /// <param name="mount">mout=gokuai 删除该用户用的存储文件</param>
        /// <param name="type"></param>
        /// <param name="handler"></param>
        private void Head(string path, string mount, string type, WP7HttpRequest.HttpResquestEventHandler handler)
        {
            string signMethod = "head";
            string sign = GenerateSign(signMethod, path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath.GetHashCode().ToString()));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("mount", mount));
            paramList.Add(new KeyValuePair<string, string>("type", type));
            paramList.Add(new KeyValuePair<string, string>("sign", Uri.EscapeUriString(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.GET;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();
        }


        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path">删除路径</param>
        public void Del(string path, string mount,WP7HttpRequest.HttpResquestEventHandler handler) 
        {
            string signMethod = "del";
            string sign = GenerateSign(signMethod, path);
            string encodePath = UrlEncoder.Encode(path);

            List<KeyValuePair<string, string>> paramList = new List<KeyValuePair<string, string>>();
            paramList.Add(new KeyValuePair<string, string>("path", encodePath));
            paramList.Add(new KeyValuePair<string, string>("token", _token));
            paramList.Add(new KeyValuePair<string, string>("mount", mount));
            paramList.Add(new KeyValuePair<string, string>("sign", Uri.EscapeUriString(sign)));

            WP7HttpRequest request = new WP7HttpRequest();
            request._httpCompleted += new WP7HttpRequest.HttpResquestEventHandler(handler);
            request.RequestMethod = RequestType.POST;
            request.RequestUrl = URL_API + GenerateUrl(signMethod, paramList);
            request.Request();
 
        }

        /// <summary>
        /// 生成URL
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="paramList">参数</param>
        /// <returns></returns>
        private string GenerateUrl(string method, List<KeyValuePair<string ,string >> paramList)
        {
            string url;
            if (paramList != null)
            {
                url = "/" + method + "?";
                foreach (KeyValuePair<string, string> k in paramList)
                {
                    url += k.Key + "=" + k.Value + "&";
                }
                int len = url.Length;
                return url.Substring(0, --len);

            }
            else
            {
                url = "/" + method;
            }

            return url;
        }

        /// <summary>
        /// 生成签名
        /// </summary>
        /// <param name="method">方法</param>
        /// <param name="path">路径</param>
        /// <returns></returns>
        private string GenerateSign(string method,string path)
        {
            string string_sign=method.ToLower()+"\n";
            string_sign += _token+"\n";
            string_sign += path;
            return Util.EncodeToHMACSHA1(string_sign, _clientSecret);
        }



    }
}

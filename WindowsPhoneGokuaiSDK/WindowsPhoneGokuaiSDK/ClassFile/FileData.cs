using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 文件列表单项数据
    /// </summary>
    public class FileData:BaseData
    {
        private const string LOG_TAG = "FileData";
        private const string KEY_FILENAME = "filename";
        private const string KEY_HASH = "hash";
        private const string KEY_DIR = "dir";
        private const string KEY_FULLPATH = "fullpath";
        private const string KEY_LAST_MEMBER_NAME = "last_member_name";
        private const string KEY_LAST_DATELINE = "last_dateline";
        private const string KEY_FILEHASH = "filehash";
        private const string KEY_FILESIZE = "filesize";
        private const string KEY_URI = "uri";
        private const string KEY_PREVIEW = "preview";
        private const string KEY_THUMBNAIL = "thumbnail";

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get;
            private set;
        }

        /// <summary>
        /// 全路径
        /// </summary>
        public string FullPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 最后更新的时间戳
        /// </summary>
        public long LastDateline
        {
            get;
            private set;
        }

        /// <summary>
        /// hash
        /// </summary>
        public string Hash
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否为文件夹
        /// </summary>
        public int Dir
        {
            get;
            private set;
        }

        /// <summary>
        /// 最后更新人员名
        /// </summary>
        public string LastMemberName
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件hash
        /// </summary>
        public string FileHash
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long FileSize
        {
            get;
            private set;
        }

        /// <summary>
        /// 下载地址
        /// </summary>
        public string  Uri
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件预览地址
        /// </summary>
        public string Preview
        {
            get;
            private set;
        }
        /// <summary>
        /// 文件缩略图
        /// </summary>
        public string Thumbnail
        {
            get;
            private set;
        }

        /// <summary>
        /// 文件列表单项数据读取
        /// </summary>
        /// <param name="jsonObject"></param>
        /// <returns></returns>
        public static FileData Create(JsonObject jsonObject)
        {
            if (jsonObject == null)
            {
                return null;
            }

            FileData data=new FileData();
            data.FileHash = Util.jsonOptString(KEY_FILEHASH,jsonObject);
            data.FileName = Util.jsonOptString(KEY_FILENAME, jsonObject);
            data.FileSize = Util.jsonOptlong(KEY_FILESIZE, jsonObject,0);
            data.FullPath = Util.jsonOptString(KEY_FULLPATH, jsonObject);
            data.Hash = Util.jsonOptString(KEY_HASH, jsonObject);
            data.LastDateline = Util.jsonOptlong(KEY_LAST_DATELINE, jsonObject,0);
            data.LastMemberName = Util.jsonOptString(KEY_LAST_MEMBER_NAME, jsonObject);
            data.Preview = Util.jsonOptString(KEY_PREVIEW, jsonObject);
            data.Thumbnail = Util.jsonOptString(KEY_THUMBNAIL, jsonObject);
            data.Uri = Util.jsonOptString(KEY_URI, jsonObject);
            return data;
        }

        /// <summary>
        /// 单文件数据
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="isError"></param>
        /// <returns></returns>
        new public static FileData Create(string jsonString, bool isError)
        {
            if (jsonString == null)
            {
                return null;
            }

            FileData data = new FileData();
            try
            {
                JsonObject json = (JsonObject)(IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                if (isError)
                {
                    data.ErrorCode = Convert.ToInt32(json[KEY_ERROR_CODE]);
                    data.ErrorMessage = (string)json[KEY_ERROR_MSG];
                }
                else
                {
                    data.Dir = Util.jsonOptInt(KEY_DIR, json,0);
                    data.FileHash = Util.jsonOptString(KEY_FILEHASH, json);
                    data.FileName = Util.jsonOptString(KEY_FILENAME, json);
                    data.FileSize = Util.jsonOptlong(KEY_FILESIZE, json, 0);
                    data.FullPath = Util.jsonOptString(KEY_FULLPATH, json);
                    data.Hash = Util.jsonOptString(KEY_HASH, json);
                    data.LastDateline = Util.jsonOptlong(KEY_LAST_DATELINE, json, 0);
                    data.LastMemberName = Util.jsonOptString(KEY_LAST_MEMBER_NAME, json);
                    data.Preview = Util.jsonOptString(KEY_PREVIEW, json);
                    data.Thumbnail = Util.jsonOptString(KEY_THUMBNAIL, json);
                    data.Uri = Util.jsonOptString(KEY_URI, json);
 
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

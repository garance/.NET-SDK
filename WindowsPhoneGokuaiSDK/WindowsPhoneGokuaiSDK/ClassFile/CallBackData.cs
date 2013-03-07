using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 返回参数数据
    /// </summary>
    public class CallBackData:BaseData
    {
        private const string LOG_TAG = "CallBackData";
        private const string KEY_HASH = "hash";
        private const string KEY_FULLPATH = "fullpath";
        private const string KEY_FILEHASH = "filehash";
        private const string KEY_FILESIZE = "filesize";

        public string Hash
        {
            get;
            private set;
        }

        public string FullPath
        {
            get;
            private set;
        }

        public string FileHash
        {
            get;
            private set;
        }

        public long FileSize
        {
            get;
            private set;
        }

        new public static CallBackData Create(string jsonString, bool isError)
        {

            if (jsonString == null)
            {
                return null;
            }

            CallBackData data = new CallBackData();
            try
            {
                JsonObject json = (JsonObject)(IDictionary<string, object>)SimpleJson.DeserializeObject(jsonString);
                if (isError)
                {
                    var code = json[KEY_ERROR_CODE];
                    data.ErrorCode = int.Parse(code.ToString());
                    data.ErrorMessage =Util.jsonOptString(KEY_ERROR_MSG, json);
                }
                else
                {
                    data.FileHash = Util.jsonOptString(KEY_FILEHASH, json);
                    data.FileSize = Convert.ToInt64(json[KEY_FILEHASH]);
                    data.FullPath = Util.jsonOptString(KEY_FULLPATH, json);
                    data.Hash = Util.jsonOptString(KEY_HASH, json);
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

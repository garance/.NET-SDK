using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 文件列表数据
    /// </summary>
    public class FileListData:BaseData
    {
        private const string LOG_TAG="FileListData";
        private const string KEY_COUNT = "count";
        private const string KEY_LIST = "list";

        /// <summary>
        /// 文件和文件总数
        /// </summary>
        public int Count
        {
            get;
            private set;
        }
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<FileData> DataList
        {
            get;
            set;
        }

        /// <summary>
        /// 文件列表读取
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="isError"></param>
        /// <returns></returns>
        new public static FileListData Create(string jsonString,bool isError)
        {
            if (jsonString == null)
            {
                return null;
            }

            FileListData data = new FileListData();
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
                    var count = json[KEY_COUNT];
                    data.Count = int.Parse(json[KEY_COUNT].ToString());
                    List<FileData> list=new List<FileData>();
                    JsonArray jsonArray=(JsonArray)json[KEY_LIST];
                    foreach(JsonObject jsonobject in jsonArray )
                    {
                         list.Add(FileData.Create(jsonobject));
                    }
                    data.DataList = list;
           
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Media.Imaging;

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// 转化集
    /// </summary>
    public class Util
    {
        /// <summary>
        /// app根目录
        /// </summary>
         public const string ROOT_PATH = "Appkey\\";

        /// <summary>
        /// HMacsha1加密，最后做base64加密
        /// </summary>
        /// <param name="toEncodeString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string EncodeToHMACSHA1(string toEncodeString,string key)
        {
            HMACSHA1 hmacsha1 = new HMACSHA1();
            hmacsha1.Key = System.Text.Encoding.UTF8.GetBytes(key);

            byte[] dataBuffer = System.Text.Encoding.UTF8.GetBytes(toEncodeString);
            byte[] hashBytes = hmacsha1.ComputeHash(dataBuffer);
            return Convert.ToBase64String(hashBytes);
        }

        /// <summary>
        /// 流文件转byte
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = stream.Position;
            stream.Position = 0;

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                stream.Position = originalPosition;
            }
        }

        /// <summary>
        /// 获取文件父级路径
        /// </summary>
        /// <param name="path"></param>
        /// <param name="seperatorString"></param>
        /// <returns></returns>
        public static string getParentPath(string path,string seperatorString)
        {
                int index = path.LastIndexOf(seperatorString);
                return path.Substring(0, index);
        }

        /// <summary>
        /// 获取json long值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long jsonOptlong(string key,JsonObject json, long defaultValue)
        {
            long ret=0;
            object obj;
            ret=json.TryGetValue(key,out obj)?Convert.ToInt64(obj):defaultValue;
            return ret;
        }

        /// <summary>
        /// 获取json int值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int jsonOptInt(string key,JsonObject json, int defaultValue)
        {
            int ret=0;
            object obj;
            ret=json.TryGetValue(key,out obj)?Convert.ToInt32(obj):defaultValue;
            return ret;
        }

        /// <summary>
        /// 获取json string值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string jsonOptString(string key, JsonObject json)
        {
            string ret=string.Empty;
            object obj;
            ret=json.TryGetValue(key,out obj)?(string)obj:string.Empty;
            return ret;
        }

        /// <summary>
        /// 获取json bool类型
        /// </summary>
        /// <param name="key"></param>
        /// <param name="json"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool jsonOptBool(string key,JsonObject json, bool defaultValue)
        {
            bool ret = false;
            object obj;
            ret = json.TryGetValue(key, out obj) ? Convert.ToBoolean(obj) : false;
            return ret;
        }

    }
}

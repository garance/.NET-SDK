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

namespace WindowsPhoneGokuaiSDK.ClassFile
{
    /// <summary>
    /// Http请求参数类
    /// </summary>
    public class WP7HttpEventArgs : EventArgs
    {
        #region 私有成员

        private string _result;
        private bool _is_error;

        #endregion

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public WP7HttpEventArgs()
        {
            this.Result = "";
            this.IsError = false;
        }
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="result">返回结果</param>
        public WP7HttpEventArgs(string result)
        {
            this.Result = result;
            this.IsError = false;
        }

        /// <summary>
        /// 结果字符串
        /// </summary>
        public string Result
        {
            get { return _result; }
            set { _result = value; }
        }

        /// <summary>
        /// 是否错误
        /// </summary>
        public bool IsError
        {
            get { return _is_error; }
            set { _is_error = value; }
        }
    }
}

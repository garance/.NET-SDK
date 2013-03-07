using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using WindowsPhoneGokuaiSDK.ClassFile;
using Microsoft.Phone.Tasks;
using System.Windows.Media.Imaging;
using Microsoft.Phone;
using System.IO;
using System.IO.IsolatedStorage;

namespace WindowsPhoneGokuaiSDK
{
    /// <summary>
    /// 主界面
    /// </summary>
    public partial class MainPage : PhoneApplicationPage
    {
        private const string email="heybozai@hotmail.com";
        private const string CLIENT_ID = "65a47ac7b27925b72bd58a7fcb0c91a6";
        private const string CLIENT_SECRET = "cce4b07a5e7cc45cf2efdbb6417dbb48";
        private string _token = "c39dc22c444e539d6ee3df20e4a74442";
        private Stream _selectedPhotoStream;

        /// <summary>
        /// 构造函数
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 获取token（有clientID）按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_GetToken_HaveClientID_Click(object sender, RoutedEventArgs e)
        {
            //已申请ClientID
            GokuaiAuth gokuaiAuth = new GokuaiAuth(CLIENT_ID,CLIENT_SECRET);
            gokuaiAuth.token(email, Request_Token_HttpCompleted);
        }

        /// <summary>
        /// 获取token  网络访问数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Request_Token_HttpCompleted(object sender, WP7HttpEventArgs e)
        {
            //认证数据
            OauthData data = OauthData.Create(e.Result,e.IsError);

            if (e.IsError)
            {
                //显示错误信息
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                //获得服务器返回的token
                _token = data.Token;

                //把获得token显示出来
                MessageBox.Show(data.Token);
            }
        }

        /// <summary>
        /// 创建文件夹 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_CreateFolder_Click(object sender, RoutedEventArgs e)
        {
            GokuaiClient gokuaiClient = new GokuaiClient(_token,CLIENT_SECRET);
            gokuaiClient.CreateFolder(TBox_CreateFolderPath.Text, "", "", GokuaiClient.OVERWRITE_TYPE_CREATE_NEW_FILE, Request_CreateFolder_HttpCompleted);

        }

        /// <summary>
        /// 创建文件夹 网络访问数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Request_CreateFolder_HttpCompleted(object sender, WP7HttpEventArgs e)
        {
            CallBackData data = CallBackData.Create(e.Result, e.IsError);
            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                MessageBox.Show("创建成功！");
            }
        }

        /// <summary>
        /// 上传文件 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_UploadFile_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedPhotoStream != null)
            {
                GokuaiClient gokuaiClient = new GokuaiClient(_token, CLIENT_SECRET);
                gokuaiClient.UploadFile(TBox_UploadPath.Text, "", "", GokuaiClient.OVERWRITE_TYPE_CREATE_NEW_FILE, _selectedPhotoStream, Request_UploadFile_HttpCompleted);
            }
            else
            {
                MessageBox.Show("请先选择图片！");
            }
            
        }

        /// <summary>
        /// 上传文件 网络访问数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Request_UploadFile_HttpCompleted(object sender, WP7HttpEventArgs e)
        {
            CallBackData data = CallBackData.Create(e.Result, e.IsError);

            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                MessageBox.Show("上传成功！");
            }
        }

        /// <summary>
        /// 获得文件列表 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_GetFileList_Click(object sender, RoutedEventArgs e)
        {
            GokuaiClient gokuaiClient = new GokuaiClient(_token, CLIENT_SECRET);
            gokuaiClient.GetFileList(TBox_GetFileListPath.Text, 0, "", Request_FileList_HttpCompleted);
        }

        /// <summary>
        /// 获得文件列表 网络访问数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Request_FileList_HttpCompleted(object sender, WP7HttpEventArgs e)
        {
            FileListData data = FileListData.Create(e.Result, e.IsError);

            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                string content = "文件总数为"+data.Count.ToString()+"\n";
                foreach(FileData fileData in data.DataList)
                {
                    content += fileData.FileName + "\n";
                }
                MessageBox.Show(content);
            }
        }



        /// <summary>
        /// 删除文件 按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            GokuaiClient gokuaiClient = new GokuaiClient(_token, CLIENT_SECRET);
            gokuaiClient.Del(TBox_DeletePath.Text,"", Request_Delete_HttpCompleted);
        }

        /// <summary>
        /// 删除文件 网络访问数据回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Request_Delete_HttpCompleted(object sender, WP7HttpEventArgs e)
        {
            BaseData data = BaseData.Create(e.Result, e.IsError);

            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                MessageBox.Show("删除成功！");
            }
        }

        /// <summary>
        /// 选择图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_Picture_Select_Click(object sender, RoutedEventArgs e)
        {
            PhotoChooserTask taskToChoosePhoto = new PhotoChooserTask();
            taskToChoosePhoto.Show();
            taskToChoosePhoto.Completed += new EventHandler<PhotoResult>(TaskToChoosePhoto_Completed);
        }

        void TaskToChoosePhoto_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                string fileName = e.OriginalFileName;
                System.Windows.Media.Imaging.BitmapImage bmp = new System.Windows.Media.Imaging.BitmapImage();
                bmp.SetSource(e.ChosenPhoto);
                Image_Selected.Source = bmp;
                _selectedPhotoStream = e.ChosenPhoto;
            }
        }

        private void Btn_GetFileInfo_Click(object sender, RoutedEventArgs e)
        {
            GokuaiClient gokuaiClient = new GokuaiClient(_token, CLIENT_SECRET);
            gokuaiClient.GetFileInfo(TBox_GetFileInfo_Path.Text, "", Request_GetFileInfo_Compelted);
        }

        /// <summary>
        /// 获取文件信息回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Request_GetFileInfo_Compelted(object sender, WP7HttpEventArgs e)
        {
            FileData data = FileData.Create(e.Result, e.IsError);

            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                MessageBox.Show("文件名："+data.FileName+"\n"+"文件缩略图："+data.Thumbnail+"\n"+"文件预览地址："+data.Preview+"\n"+"文件下载地址："+data.Uri+"\n");
            }

        }

        private void Btn_Copy_Click(object sender, RoutedEventArgs e)
        {
            GokuaiClient gokuaiClient = new GokuaiClient(_token, CLIENT_SECRET);
            gokuaiClient.Copy(TBox_Copy_Path.Text,TBox_Copy_FromPath.Text,"",GokuaiClient.OVERWRITE_TYPE_CREATE_NEW_FILE,Request_Copy_Compeleted);

        }

        /// <summary>
        /// 复制请求回调
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Request_Copy_Compeleted(object sender, WP7HttpEventArgs e)
        {
            CallBackData data = CallBackData.Create(e.Result, e.IsError);

            if (e.IsError)
            {
                MessageBox.Show(data.ErrorMessage);
            }
            else
            {
                MessageBox.Show("复制成功！");
            }

        }
    }
}
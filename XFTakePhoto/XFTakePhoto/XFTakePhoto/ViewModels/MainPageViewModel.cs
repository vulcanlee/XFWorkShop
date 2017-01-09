using Plugin.Media;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Xamarin.Forms;

namespace XFTakePhoto.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region Title
        private string _Title= "多奇數位創意有限公司";
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #region MyImageSource
        private ImageSource _ImageSource;
        /// <summary>
        /// ImageSource
        /// </summary>
        public ImageSource MyImageSource
        {
            get { return this._ImageSource; }
            set { this.SetProperty(ref this._ImageSource, value); }
        }
        #endregion

        #region RemoteImageURL
        private string _RemoteImageURL = "";
        /// <summary>
        /// RemoteImageURL
        /// </summary>
        public string RemoteImageURL
        {
            get { return this._RemoteImageURL; }
            set { this.SetProperty(ref this._RemoteImageURL, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        public DelegateCommand 拍照Command { get; set; }

        public readonly IPageDialogService _dialogService;
        #endregion

        #region Constructor 建構式
        public MainPageViewModel(IPageDialogService dialogService)
        {

            _dialogService = dialogService;
            拍照Command = new DelegateCommand(async () =>
            {
                // https://github.com/jamesmontemagno/MediaPlugin
                // https://github.com/dsplaisted/PCLStorage
                // 進行 Plugin.Media 套件的初始化動作
                await CrossMedia.Current.Initialize();

                // 確認這個裝置是否具有拍照的功能
                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await _dialogService.DisplayAlertAsync("No Camera", ":( No camera available.", "OK");
                    return;
                }

                // 啟動拍照功能，並且儲存到指定的路徑與檔案中
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    Directory = "Sample",
                    Name = "Sample.jpg", 
                });

                if (file == null)
                    return;

                // 讀取剛剛拍照的檔案內容，轉換成為 ImageSource，如此，就可以顯示到螢幕上了
                // 要這麼做的話，是因為圖片檔案是儲存在手機端的永久儲存體中，不是隨著專案安裝時候，就部署上去的
                // 因此，需要透過 ImageSource.FromStream 來讀取圖片檔案內容，產生出 ImageSource 物件，
                // 再透過資料繫節綁訂到 View 上的 Image 控制項
                MyImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });

                #region 將剛剛拍照的檔案，上傳到網路伺服器上
                using (var client = new HttpClient())
                {
                    using (var content = new MultipartFormDataContent())
                    {
                        // 取得這個圖片檔案的完整路徑
                        var path = file.Path;
                        // 取得這個檔案的最終檔案名稱
                        var filename = Path.GetFileName(path);

                        // 開啟這個圖片檔案，並且讀取其內容
                        using (var fs = file.GetStream())
                        {
                            var streamContent = new StreamContent(fs);
                            streamContent.Headers.Add("Content-Type", "application/octet-stream");
                            streamContent.Headers.Add("Content-Disposition", "form-data; name=\"file\"; filename=\"" + Path.GetFileName(path) + "\"");
                            content.Add(streamContent, "file", filename);

                            // 上傳到遠端伺服器上
                            HttpResponseMessage message = await client.PostAsync("http://xamarindoggy.azurewebsites.net/api/UploadImage", content);
                            var input = message.Content.ReadAsStringAsync();
                            // 更新頁面上的 Image 控制項，顯示出剛剛上傳的圖片內容
                            // 這個 RemoteImageURL 屬性綁訂到 View 上的 Image 物件
                            RemoteImageURL = $"http://xamarindoggy.azurewebsites.net/uploads/{filename}";
                        }
                    }
                }
                #endregion

            });
        }

        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
        }
        #endregion

        #region 設計時期或者執行時期的ViewModel初始化
        #endregion

        #region 相關事件
        #endregion

        #region 相關的Command定義
        #endregion

        #region 其他方法
        #endregion

    }
}

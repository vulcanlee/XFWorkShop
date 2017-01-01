using PCLStorage;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using XFFiles.Models;
using Newtonsoft.Json;
using System.Reflection;
using System.IO;
using XFFiles.Services;

namespace XFFiles.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region Title
        private string _Title = "多奇數位創意有限公司";
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #region 使用這登入資訊紀錄
        private 使用這登入資訊 _使用這登入資訊紀錄 = new 使用這登入資訊();
        /// <summary>
        /// 使用這登入資訊紀錄
        /// </summary>
        public 使用這登入資訊 使用這登入資訊紀錄
        {
            get { return this._使用這登入資訊紀錄; }
            set { this.SetProperty(ref this._使用這登入資訊紀錄, value); }
        }
        #endregion

        #region 原生專案檔案內容
        private string _原生專案檔案內容;
        /// <summary>
        /// 原生專案檔案內容
        /// </summary>
        public string 原生專案檔案內容
        {
            get { return this._原生專案檔案內容; }
            set { this.SetProperty(ref this._原生專案檔案內容, value); }
        }
        #endregion

        #region 核心PCL專案檔案內容
        private string _核心PCL專案檔案內容;
        /// <summary>
        /// 核心PCL專案檔案內容
        /// </summary>
        public string 核心PCL專案檔案內容
        {
            get { return this._核心PCL專案檔案內容; }
            set { this.SetProperty(ref this._核心PCL專案檔案內容, value); }
        }
        #endregion

        #region 原生專案檔案內容Content
        private string _原生專案檔案內容Content;
        /// <summary>
        /// 原生專案檔案內容Content
        /// </summary>
        public string 原生專案檔案內容Content
        {
            get { return this._原生專案檔案內容Content; }
            set { this.SetProperty(ref this._原生專案檔案內容Content, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        public DelegateCommand 登入Command { get; set; }

        private readonly INavigationService _navigationService;
        private readonly INatvieAssyFile _natvieAssyFile;
        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, INatvieAssyFile natvieAssyFile)
        {

            _navigationService = navigationService;
            _natvieAssyFile = natvieAssyFile;

            登入Command = new DelegateCommand(async () =>
            {
                // 取得這個應用程式的所在目錄
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                // 產生要儲存資料的資料夾
                IFolder sourceFolder = await FileSystem.Current.LocalStorage.CreateFolderAsync("MyDatas", CreationCollisionOption.ReplaceExisting);
                // 建立要儲存資料的檔案
                IFile sourceFile = await sourceFolder.CreateFileAsync("使用這登入資訊紀錄.dat", CreationCollisionOption.ReplaceExisting);

                // 深層複製該物件的所有值
                var bar使用這登入資訊紀錄 = 使用這登入資訊紀錄.ShallowCopy();
                // 若不需要記憶密碼，則不需要將密碼儲存到手機內
                if (bar使用這登入資訊紀錄.記憶密碼 == false)
                {
                    bar使用這登入資訊紀錄.密碼 = "";
                }
                // 將使用者輸入的登入資訊，序列化成為 Json 文字
                var foo使用這登入資訊紀錄 = JsonConvert.SerializeObject(bar使用這登入資訊紀錄);
                // 寫入到檔案中
                await sourceFile.WriteAllTextAsync(foo使用這登入資訊紀錄);
            });
        }

        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            try
            {
                // 取得這個應用程式的所在目錄
                IFolder rootFolder = FileSystem.Current.LocalStorage;
                // 取得要讀取資料的資料夾目錄
                IFolder sourceFolder = await FileSystem.Current.LocalStorage.GetFolderAsync("MyDatas");
                // 判斷這個資料夾內是否有這個檔案存在
                if (await sourceFolder.CheckExistsAsync("使用這登入資訊紀錄.dat") == ExistenceCheckResult.FileExists)
                {
                    // 開啟這個檔案
                    IFile sourceFile = await sourceFolder.GetFileAsync("使用這登入資訊紀錄.dat");

                    // 將檔案內的文字都讀出來
                    var foo使用這登入資訊紀錄 = await sourceFile.ReadAllTextAsync();
                    // 將 Json 文字反序列會成為 .NET 物件
                    var bar使用這登入資訊紀錄 = JsonConvert.DeserializeObject<使用這登入資訊>(foo使用這登入資訊紀錄);

                    // 將讀出的物件，設定到檢視模型內的屬性上
                    使用這登入資訊紀錄.姓名 = bar使用這登入資訊紀錄.姓名;
                    使用這登入資訊紀錄.密碼 = bar使用這登入資訊紀錄.密碼;
                    使用這登入資訊紀錄.帳號 = bar使用這登入資訊紀錄.帳號;
                    使用這登入資訊紀錄.記憶密碼 = bar使用這登入資訊紀錄.記憶密碼;
                }
            }
            catch { }

            #region 讀取核心PCL 專案檔案內容(內嵌資源)
            string resource = "XFFiles.Assets.XFFiles PCL.txt";
            Assembly assembly = GetType().GetTypeInfo().Assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    核心PCL專案檔案內容 = reader.ReadToEnd();
                }
            }
            #endregion

            #region 讀取原生專案檔案內容(內嵌資源)
            var fooOS = _natvieAssyFile.GetPlatformName();

            string assemblyName = "";
            if (fooOS == "Android")
            {
                assemblyName = "XFFiles.Droid";
                resource = "XFFiles.Droid.Assets.XFFiles Android.txt";
            }
            else if (fooOS == "UWP")
            {
                assemblyName = "XFFiles.UWP";
                resource = "XFFiles.UWP.Assets.XFFiles UWP.txt";
            }
            else 
            {
                assemblyName = "XFFiles.iOS";
                resource = "XFFiles.iOS.Resources.XFFiles iOS.txt";
            }
            assembly = Assembly.Load(new AssemblyName(assemblyName));
            var fooa = assemblyName;
            var foob = assembly;
            using (Stream stream = assembly.GetManifestResourceStream(resource))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    原生專案檔案內容 = reader.ReadToEnd();
                }
            }
            #endregion

            #region 讀取原生專案檔案內容(iOS:BundleResource / Android:AndroidAsset / UWP:Content)
            原生專案檔案內容Content = _natvieAssyFile.GetFileContent($"XFFiles {fooOS} Content.txt");
            #endregion
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

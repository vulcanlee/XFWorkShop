using PCLStorage;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XFRNotiiOS.Models;
using XFRNotiiOS.Services;

namespace XFRNotiiOS.ViewModels
{

    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region 基本型別與類別的 Property

        #region UpdateInformation
        private string _UpdateInformation = "";
        /// <summary>
        /// UpdateInformation
        /// </summary>
        public string UpdateInformation
        {
            get { return this._UpdateInformation; }
            set { this.SetProperty(ref this._UpdateInformation, value); }
        }
        #endregion

        #region NotificationLogInformation
        private string _NotificationLogInformation;
        /// <summary>
        /// NotificationLogInformation
        /// </summary>
        public string NotificationLogInformation
        {
            get { return this._NotificationLogInformation; }
            set { this.SetProperty(ref this._NotificationLogInformation, value); }
        }
        #endregion

        #endregion

        #region 集合類別的 Property

        #endregion

        #endregion

        #region Field 欄位

        #region ViewModel 內使用到的欄位
        #endregion

        #region 命令物件欄位

        public DelegateCommand ReadAgainCommand { get; set; }
        public DelegateCommand ResetCommand { get; set; }

        #endregion

        #region 注入物件欄位

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        private readonly ILogService _logService;
        #endregion

        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, 
            ILogService logService)
        {

            #region 相依性服務注入的物件
            _logService = logService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
            #endregion

            #region 頁面中綁定的命令
            ResetCommand = new DelegateCommand(() =>
            {
                _logService.Write("");
            });
            ReadAgainCommand = new DelegateCommand( () =>
            {
                NotificationLogInformation = _logService.Read();
            });
            #endregion

            #region 事件聚合器訂閱
            _eventAggregator.GetEvent<LocalNotificationToPCLEvent>().Subscribe(async x =>
            {
                if (x.NavigationPage == "DetailPage")
                {
                    #region 需要繼續顯示到明細頁面
                    var fooPara = new NavigationParameters();
                    fooPara.Add("ShowIt", x);
                    await _navigationService.NavigateAsync("DetailPage", fooPara);
                    #endregion
                }
            });

            _eventAggregator.GetEvent<UpdateInfoEvent>().Subscribe(x =>
            {
                UpdateInformation += $"{x.time.ToString()} {x.Name}\r\n";
            });
            #endregion
        }

        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {

        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("LocalNotification"))
            {
                #region 需要繼續顯示到明細頁面
                LocalNotificationPayload fooLocalNotificationPayload = parameters["LocalNotification"] as LocalNotificationPayload;
                var fooPara = new NavigationParameters();
                fooPara.Add("ShowIt", fooLocalNotificationPayload);
                await _navigationService.NavigateAsync("DetailPage", fooPara);
                #endregion
            }

            await ViewModelInit();
        }
        #endregion

        #region 設計時期或者執行時期的ViewModel初始化
        #endregion

        #region 相關事件
        #endregion

        #region 相關的Command定義
        #endregion

        #region 其他方法

        /// <summary>
        /// ViewModel 資料初始化
        /// </summary>
        /// <returns></returns>
        private async Task ViewModelInit()
        {
            NotificationLogInformation = _logService.Read();
            await Task.Delay(100);
        }

        #region Notification Log 的檔案讀寫
        public async Task<string> ReadNotificationLog()
        {
            string fooFilename = "NotificationLog.txt";
            string fooContent = "";
            IFile file;

            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);
            var fooResult = await folder.CheckExistsAsync(fooFilename);
            if (fooResult == ExistenceCheckResult.NotFound)
            {
                file = await folder.CreateFileAsync(fooFilename, CreationCollisionOption.ReplaceExisting);
            }
            else
            {
                file = await folder.GetFileAsync(fooFilename);
                fooContent = await file.ReadAllTextAsync();
            }
            return fooContent;
        }

        public async Task WriteNotificationLog(string fooContent)
        {
            string fooFilename = "NotificationLog.txt";
            IFile file;

            IFolder rootFolder = FileSystem.Current.LocalStorage;
            IFolder folder = await rootFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists);
            file = await folder.CreateFileAsync(fooFilename, CreationCollisionOption.ReplaceExisting);
            await file.WriteAllTextAsync(fooContent);
            return;
        }
        #endregion
        #endregion

    }
}

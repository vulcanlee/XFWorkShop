using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace XFLocalNotificationiOS.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region Title
        private string _Title;
        /// <summary>
        /// 主題
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #region Message
        private string _Message;
        /// <summary>
        /// 訊息內文
        /// </summary>
        public string Message
        {
            get { return this._Message; }
            set { this.SetProperty(ref this._Message, value); }
        }
        #endregion
      
        #region SummaryText
        private string _SummaryText;
        /// <summary>
        /// SummaryText
        /// </summary>
        public string SummaryText
        {
            get { return this._SummaryText; }
            set { this.SetProperty(ref this._SummaryText, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        #region ViewModel 內使用到的欄位
        #endregion

        #region 命令物件欄位

        public DelegateCommand LaunchNotificationCommand { get; set; }
        #endregion

        #region 注入物件欄位

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        #endregion

        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {

            #region 相依性服務注入的物件

            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
            #endregion

            #region 頁面中綁定的命令
            LaunchNotificationCommand = new DelegateCommand(() =>
            {
                var fooLocalNotificationPayload = new LocalNotificationPayload
                {
                    ContentTitle = Title,
                    ContentText = Message,
                    NavigationPage = "DetailPage",
                    OtherInformation = "這是額外要傳遞的資訊",
                    SummaryText = SummaryText,
                };
                _eventAggregator.GetEvent<LocalNotificationEvent>().Publish(fooLocalNotificationPayload);
            });
            #endregion

            #region 事件聚合器訂閱
            _eventAggregator.GetEvent<LocalNotificationToPCLEvent>().Subscribe(async x =>
            {
                if(x.NavigationPage == "DetailPage")
                {
                    #region 需要繼續顯示到明細頁面
                    var fooPara = new NavigationParameters();
                    fooPara.Add("ShowIt", x);
                    await _navigationService.NavigateAsync("DetailPage", fooPara);
                    #endregion
                }
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
            Title = "你有一則新留言";
            Message = "這是來自於 iOS 本地端的訊息通知";
            SummaryText = "這來自於您的留言";

            await Task.Delay(100);
        }

        #endregion

    }
}

using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XFRNotiiOS.Models;

namespace XFRNotiiOS.ViewModels
{

    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region 基本型別與類別的 Property

        #endregion

        #region 集合類別的 Property

        #endregion

        #endregion

        #region Field 欄位

        #region ViewModel 內使用到的欄位
        #endregion

        #region 命令物件欄位
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
            await Task.Delay(100);
        }
        #endregion

    }
}

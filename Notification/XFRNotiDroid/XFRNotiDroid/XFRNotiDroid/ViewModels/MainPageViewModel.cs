using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XFRNotiDroid.Models;

namespace XFRNotiDroid.ViewModels
{

    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #endregion

        #region Field 欄位

        #region ViewModel 內使用到的欄位
        #endregion

        #region 命令物件欄位
        #endregion

        #region 注入物件欄位
        private readonly INavigationService _navigationService;
        #endregion

        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService)
        {

            #region 相依性服務注入的物件

            _navigationService = navigationService;
            #endregion

            #region 頁面中綁定的命令

            #endregion

            #region 事件聚合器訂閱

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
            await ViewModelInit();

            #region 需要自己處理的自動換頁機制
            // 使用者是否有啟動要自動換頁
            if (parameters.ContainsKey("AutoNext"))
            {
                // 取出要換到哪個頁面
                var fooAuto = (string)parameters["AutoNext"];
                // 取出推播通知的傳遞內容
                var fooNotificationPayload = parameters["NotificationPayload"] as NotificationPayload;
                if (fooAuto == "NextPage")
                {
                    // 切換到指定頁面，並且將推播通知內容送出去
                    var fooPara = new NavigationParameters();
                    fooPara.Add("NotificationPayload", fooNotificationPayload);
                    await _navigationService.NavigateAsync(fooAuto, fooPara);
                }
            }
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

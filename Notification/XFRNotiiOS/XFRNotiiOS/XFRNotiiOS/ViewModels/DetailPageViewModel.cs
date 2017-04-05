using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XFRNotiiOS.Models;

namespace XFRNotiiOS.ViewModels
{
    public class DetailPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region Title
        private string _Title;
        /// <summary>
        /// Title
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
        /// Message
        /// </summary>
        public string Message
        {
            get { return this._Message; }
            set { this.SetProperty(ref this._Message, value); }
        }
        #endregion

        #region OtherInformation
        private string _OtherInformation;
        /// <summary>
        /// OtherInformation
        /// </summary>
        public string OtherInformation
        {
            get { return this._OtherInformation; }
            set { this.SetProperty(ref this._OtherInformation, value); }
        }
        #endregion

        #region NotificationType
        private string _NotificationType;
        /// <summary>
        /// NotificationType
        /// </summary>
        public string NotificationType
        {
            get { return this._NotificationType; }
            set { this.SetProperty(ref this._NotificationType, value); }
        }
        #endregion

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
        public DetailPageViewModel(INavigationService navigationService)
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
            if (parameters.ContainsKey("ShowIt"))
            {
                LocalNotificationPayload fooLocalNotificationPayload = parameters["ShowIt"] as LocalNotificationPayload;
                Title = fooLocalNotificationPayload.ContentTitle;
                Message = fooLocalNotificationPayload.ContentText;
                OtherInformation = fooLocalNotificationPayload.OtherInformation;
                NotificationType = fooLocalNotificationPayload.Style.ToString();
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

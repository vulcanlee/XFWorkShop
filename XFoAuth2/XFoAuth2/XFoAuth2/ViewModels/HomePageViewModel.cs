using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using XFoAuth2.Models;

namespace XFoAuth2.ViewModels
{
    /// <summary>
    /// 身分驗證完成後，需要顯示的 App 首頁
    /// </summary>
    public class HomePageViewModel : BindableBase,INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region 名稱
        private string _名稱;
        /// <summary>
        /// 名稱
        /// </summary>
        public string 名稱
        {
            get { return this._名稱; }
            set { this.SetProperty(ref this._名稱, value); }
        }
        #endregion

        #region 電子郵件
        private string _電子郵件;
        /// <summary>
        /// 電子郵件
        /// </summary>
        public string 電子郵件
        {
            get { return this._電子郵件; }
            set { this.SetProperty(ref this._電子郵件, value); }
        }
        #endregion

        #region 大頭貼URL
        private string _大頭貼URL;
        /// <summary>
        /// 大頭貼URL
        /// </summary>
        public string 大頭貼URL
        {
            get { return this._大頭貼URL; }
            set { this.SetProperty(ref this._大頭貼URL, value); }
        }
        #endregion

        #region 身分驗證方式
        private string _身分驗證方式;
        /// <summary>
        /// 身分驗證方式
        /// </summary>
        public string 身分驗證方式
        {
            get { return this._身分驗證方式; }
            set { this.SetProperty(ref this._身分驗證方式, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        private readonly INavigationService _navigationService;
        #endregion

        #region Constructor 建構式
        public HomePageViewModel(INavigationService navigationService)
        {

            _navigationService = navigationService;
        }
        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            // 當顯示這個頁面的時候，進行相關屬性 Property 的初始值設定
            if (parameters.ContainsKey("User"))
            {
                var fooUser = parameters["User"] as LoginUserParameter;
                身分驗證方式 = fooUser.身分驗證方式.ToString();
                名稱 = fooUser.名稱;
                電子郵件 = fooUser.電子郵件;
                大頭貼URL = fooUser.大頭貼URL;
            }
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

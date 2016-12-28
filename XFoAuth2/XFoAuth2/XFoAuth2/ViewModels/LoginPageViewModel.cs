using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using XFoAuth2.Helpers;
using XFoAuth2.Models;

namespace XFoAuth2.ViewModels
{
    /// <summary>
    /// 這個範例專案的啟動頁面，可以任使用者輸入帳、密，或者選擇不同的 OAuth2 認證方法
    /// </summary>
    public class LoginPageViewModel : BindableBase, INavigationAware
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

        #region 帳號
        private string _帳號 = "";
        /// <summary>
        /// 帳號
        /// </summary>
        public string 帳號
        {
            get { return this._帳號; }
            set { this.SetProperty(ref this._帳號, value); }
        }
        #endregion

        #region 密碼
        private string _密碼 = "";
        /// <summary>
        /// 密碼
        /// </summary>
        public string 密碼
        {
            get { return this._密碼; }
            set { this.SetProperty(ref this._密碼, value); }
        }
        #endregion

        #region UserName
        private string _UserName;
        /// <summary>
        /// UserName
        /// </summary>
        public string UserName
        {
            get { return this._UserName; }
            set { this.SetProperty(ref this._UserName, value); }
        }
        #endregion

        #region UserEmail
        private string _UserEmail;
        /// <summary>
        /// UserEmail
        /// </summary>
        public string UserEmail
        {
            get { return this._UserEmail; }
            set { this.SetProperty(ref this._UserEmail, value); }
        }
        #endregion

        #region ImageURL
        private string _ImageURL;
        /// <summary>
        /// ImageURL
        /// </summary>
        public string ImageURL
        {
            get { return this._ImageURL; }
            set { this.SetProperty(ref this._ImageURL, value); }
        }
        #endregion


        #endregion

        #region Field 欄位

        public DelegateCommand FacebookButtonCommand { get; set; }
        public DelegateCommand GoogleButtonCommand { get; set; }
        public DelegateCommand 登入Command { get; set; }
        public DelegateCommand 點選註冊一個新帳號Command { get; set; }

        public readonly IPageDialogService _dialogService;
        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructor 建構式
        public LoginPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator, IPageDialogService dialogService)
        {

            _dialogService = dialogService;
            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            GoogleButtonCommand = new DelegateCommand(async () =>
            {
                AuthenticationHelper.OAuthType = OAuthTypeEnum.Google;
                await _navigationService.NavigateAsync("oAuthPage", null);
            });
            FacebookButtonCommand = new DelegateCommand(async () =>
            {
                AuthenticationHelper.OAuthType = OAuthTypeEnum.Facebook;
                await _navigationService.NavigateAsync("oAuthPage", null);
            });
            _eventAggregator.GetEvent<LoginEvent>().Subscribe(async x =>
            {
                if (x == "Refresh")
                {
                    // 系統通知需要進入到應用程式的首頁，並且將使用者資訊，傳入到首頁
                    var account = await AuthenticationHelper.IsAlreadyAuthenticated();
                    if (account != null)
                    {
                        // 確認使用者已經通過驗證，必且取得該使用者的詳細資訊
                        ImageURL = account.Properties[Constants.PhotoAccountProperty];
                        UserName = account.Properties[Constants.NameAccountProperty];
                        UserEmail = account.Properties[Constants.EmailAccountProperty];
                        // 取得使用者的 OAuth2 認證方式
                        var LoginTypeAccount = account.Properties[Constants.LoginTypeAccountProperty];
                        var fooLoginUser = new LoginUserParameter();
                        if (LoginTypeAccount == OAuthTypeEnum.Facebook.ToString())
                        {
                            fooLoginUser.身分驗證方式 = LoginUserEnum.Facebook;
                        }
                        else
                        {
                            fooLoginUser.身分驗證方式 = LoginUserEnum.Google;
                        }
                        fooLoginUser.名稱 = UserName;
                        fooLoginUser.電子郵件 = UserEmail;
                        fooLoginUser.大頭貼URL = ImageURL;

                        // 建立要傳遞到另外一個頁面的參數物件
                        var fooPara = new NavigationParameters();
                        fooPara.Add("User", fooLoginUser);
                        // 使用絕對路徑方式導航到首頁，也就是要清除導航堆疊
                        await _navigationService.NavigateAsync("xf:///HomePage", fooPara);
                    }
                }
            });
            登入Command = new DelegateCommand(async () =>
            {
                if (帳號 != "1" && 密碼 != "1")
                {
                    await _dialogService.DisplayAlertAsync("警告", "帳號與密碼不正確，帳號與密碼都是數字1", "OK");
                }
                else
                {
                    var fooLoginUser = new LoginUserParameter();
                    fooLoginUser.身分驗證方式 = LoginUserEnum.AccountPassword;
                    fooLoginUser.名稱 = 帳號;
                    fooLoginUser.電子郵件 = "N/A";
                    fooLoginUser.大頭貼URL = "N/A";

                    var fooPara = new NavigationParameters();
                    fooPara.Add("User", fooLoginUser);
                    // 使用絕對路徑方式導航到首頁，也就是要清除導航堆疊
                    await _navigationService.NavigateAsync("xf:///HomePage", fooPara);
                }
            });
            點選註冊一個新帳號Command = new DelegateCommand(async() =>
            {
                await _dialogService.DisplayAlertAsync("警告", "該功能尚未建置", "OK");
            });

            Title = "多奇數位創意有限公司";
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

using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using XFoAuth2.Models;
using XFoAuth2.Services;

namespace XFoAuth2.Helpers
{
    /// <summary>
    /// 社群身分驗證會用到的定義常數
    /// </summary>
    public static class Constants
    {
        public const string IDAccountProperty = "id";
        public const string NameAccountProperty = "Name";
        public const string EmailAccountProperty = "email";
        public const string PhotoAccountProperty = "photo";
        public const string LoginTypeAccountProperty = "LoginType";
        public const string access_tokenAccountProperty = "access_token";
        public const string expires_inAccountProperty = "expires_in";
        public const string token_typeAccountProperty = "token_type";
        public const string id_tokenAccountProperty = "id_token";
    }

    /// <summary>
    /// OAuth2認證方式
    /// </summary>
    public enum OAuthTypeEnum
    {
        Google,
        Facebook
    }

    /// <summary>
    /// OAuth2認證需要用到的參數
    /// </summary>
    public class OAuth2AuthenticatorParameter
    {
        /// <summary>
        /// 此次 OAuth2 認證的方式
        /// </summary>
        public OAuthTypeEnum Type { get; set; } = OAuthTypeEnum.Google;
        public string clientId { set; get; }
        public string clientSecret { set; get; }
        public string scope { set; get; }
        public string authorizeUrl { set; get; }
        public string accessTokenUrl { set; get; }
        public string redirectUrl { set; get; }
        /// <summary>
        /// 取得使用者基本資訊的 Graph API URL
        /// </summary>
        public string UserInfoUrl { set; get; }
    }

    /// <summary>
    /// 提供 OAuth2 需要進行的初始化，取得使用者資訊方法與判斷使用者是否已經登入成功了
    /// </summary>
    public static class AuthenticationHelper
    {
        /// <summary>
        /// 要將通過認證的資訊，儲存到本機中的識別名稱
        /// </summary>
        public const string AppName = "XamFormsUWPOAuth";
        /// <summary>
        /// 此次需要進行的 OAuth2 的認證類別
        /// </summary>
        public static OAuthTypeEnum OAuthType { get; set; } = OAuthTypeEnum.Google;
        /// <summary>
        /// 進行 OAuth2 認證時候，需要用到的各種網址與設定參數
        /// </summary>
        public static List<OAuth2AuthenticatorParameter> OAuthParas { get; set; } = new List<OAuth2AuthenticatorParameter>();

        /// <summary>
        /// 進行 OAuth2 需要進行的初始化
        /// </summary>
        public static void Init()
        {
            OAuthType = OAuthTypeEnum.Google;
            OAuthParas = new List<OAuth2AuthenticatorParameter>();

            #region 進行 Google 身分驗證需要用到的參數
            OAuthParas.Add(new OAuth2AuthenticatorParameter
            {
                clientId = "213736323010-3qtl6tan7havj3clc1in4kk0t33of9ro.apps.googleusercontent.com",
                clientSecret = "UHfqLOu2up2SH8neBdcPzaBY",
                authorizeUrl = "https://accounts.google.com/o/oauth2/auth",
                accessTokenUrl = "https://accounts.google.com/o/oauth2/token",
                UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo",
                redirectUrl = "http://mylabtw.blogspot.tw/oauth2callback",
                scope = "profile email",
                Type = OAuthTypeEnum.Google,
            });
            #endregion

            #region 進行 Facebook 身分驗證需要用到的參數
            OAuthParas.Add(new OAuth2AuthenticatorParameter
            {
                clientId = "1144209035696864",
                clientSecret = "5cb5c9154fd9d86ca2c4ff6d003849e5",
                authorizeUrl = "https://m.facebook.com/dialog/oauth/",
                accessTokenUrl = "https://graph.facebook.com/v2.8/oauth/access_token",
                UserInfoUrl = "https://graph.facebook.com/me?fields=email,name,first_name,last_name,gender,picture",
                redirectUrl = "http://www.facebook.com/connect/login_success.html",
                scope = "email",
                Type = OAuthTypeEnum.Facebook,
            });
            #endregion
        }

        /// <summary>
        /// 根據通過身分驗證的資訊(如 存取權杖)，取得該使用者的明細資訊
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task FetchUserProfile(Account account)
        {
            // 取得 Prism 相依性服務使用到的容器
            IUnityContainer fooContainer = (XFoAuth2.App.Current as PrismApplication).Container;
            // 取得 IAccountStore 介面實際實作的類別物件
            IAccountStore fooIAccountStore = fooContainer.Resolve<IAccountStore>();

            if (AuthenticationHelper.OAuthType == OAuthTypeEnum.Google)
            {
                // 取得 OAuth2 需要用到的參數定義物件
                var fooOAuthParas = OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Google);
                // 取得使用者的詳細資訊
                var request = new OAuth2Request("GET", new Uri(fooOAuthParas.UserInfoUrl), null, account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // 取得回傳的 JSON 格式文字
                    var userJson = response.GetResponseText();
                    var user = JsonConvert.DeserializeObject<GoogleUserProfile>(userJson);
                    var foo = user;
                   
                    #region 將認證通過的相關使用者明細資訊，儲存到 Account 物件內
                    account.Properties[Constants.IDAccountProperty] = user.Id;
                    account.Properties[Constants.NameAccountProperty] = user.Name;
                    account.Properties[Constants.EmailAccountProperty] = user.Email;
                    account.Properties[Constants.PhotoAccountProperty] = user.Picture;
                    account.Properties[Constants.LoginTypeAccountProperty] = AuthenticationHelper.OAuthType.ToString();
                    #endregion
                
                    #region 將通過認證的 OAuth2 帳號與使用者詳細資訊，儲存到本機上(其中 Xamarin.Auth 並不支援 UWP，所以，使用相依性注入服務來解決
                    if (fooIAccountStore.GetPlatform() == "UWP")
                    {
                        await fooIAccountStore.SaveAccount(account);
                    }
                    else
                    {
                        AccountStore.Create().Save(account, AuthenticationHelper.AppName);
                    }
                    #endregion
                }
            }
            else
            {
                // 取得 OAuth2 需要用到的參數定義物件
                var fooOAuthParas = OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Facebook);
                // 取得使用者的詳細資訊
                var request = new OAuth2Request("GET", new Uri(fooOAuthParas.UserInfoUrl), null, account);
                var response = await request.GetResponseAsync();
                if (response != null)
                {
                    // 取得回傳的 JSON 格式文字
                    var userJson = response.GetResponseText();
                    string fooJson = userJson;
                    var user = JsonConvert.DeserializeObject<FacebookUserProfile>(userJson);
                    var foo = user;

                    #region 將認證通過的相關使用者明細資訊，儲存到 Account 物件內
                    account.Properties[Constants.IDAccountProperty] = user.id;
                    account.Properties[Constants.NameAccountProperty] = $"{user.first_name} {user.name}";
                    account.Properties[Constants.EmailAccountProperty] = user.email;
                    account.Properties[Constants.PhotoAccountProperty] = user.picture.data.url;
                    account.Properties[Constants.LoginTypeAccountProperty] = AuthenticationHelper.OAuthType.ToString();
                    #endregion

                    #region 將通過認證的 OAuth2 帳號與使用者詳細資訊，儲存到本機上(其中 Xamarin.Auth 並不支援 UWP，所以，使用相依性注入服務來解決
                    if (fooIAccountStore.GetPlatform() == "UWP")
                    {
                        await fooIAccountStore.SaveAccount(account);
                    }
                    else
                    {
                        AccountStore.Create().Save(account, AuthenticationHelper.AppName);
                    }
                    #endregion
                }
            }
        }

        /// <summary>
        /// 判斷這個使用者是否已經通過了身分驗證
        /// </summary>
        /// <returns></returns>
        public static async Task<Account> IsAlreadyAuthenticated()
        {
            // 取得 Prism 相依性服務使用到的容器
            IUnityContainer fooContainer = (XFoAuth2.App.Current as PrismApplication).Container;
            // 取得 IAccountStore 介面實際實作的類別物件
            IAccountStore fooIAccountStore = fooContainer.Resolve<IAccountStore>();

            #region 若可以取得使用者認證的使用者資訊，表示，這個使用者已經通過認證了
            if (fooIAccountStore.GetPlatform() == "UWP")
            {
                return await fooIAccountStore.GetAccount();
            }
            else
            {
                // Retrieve any stored account information
                var accounts = await AccountStore.Create().FindAccountsForServiceAsync(AppName);
                var account = accounts.FirstOrDefault();

                // If we already have the account info then we are set
                if (account == null) return null;
                return account;
            }
            #endregion
        }
    }
}

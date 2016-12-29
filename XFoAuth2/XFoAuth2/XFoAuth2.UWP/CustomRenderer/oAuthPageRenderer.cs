using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Prism.Events;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using XFoAuth2.Helpers;
using XFoAuth2.Models;
using XFoAuth2.UWP;
using XFoAuth2.UWP.CustomRenderer;
using XFoAuth2.Views;

[assembly: ExportRenderer(typeof(oAuthPage), typeof(oAuthPageRenderer))]
namespace XFoAuth2.UWP.CustomRenderer
{
    // https://developer.xamarin.com/guides/xamarin-forms/custom-renderer/contentpage/
    /// <summary>
    /// 客製化 OAuth2 登入頁面要 Renderer 的相關設定
    /// </summary>
    public class oAuthPageRenderer : PageRenderer
    {
        /// <summary>
        /// 這個頁面是否已經顯示出來了
        /// </summary>
        private bool _isShown;

        /// <summary>
        /// OnElementChanged method is called when the corresponding Xamarin.Forms control is created.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || Element == null)
                return;
        }
        protected async override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (_isShown) return;
            _isShown = true;

            // 取得 Prism 相依性服務使用到的容器
            IUnityContainer fooContainer = (XFoAuth2.App.Current as PrismApplication).Container;
            // 取得 IAccountStore 介面實際實作的類別物件
            var fooIEventAggregator = fooContainer.Resolve<IEventAggregator>();

            // 使用 UWP 的 Web authentication broker
            // https://msdn.microsoft.com/en-us/windows/uwp/security/web-authentication-broker
            var code = await AuthenticateUsingWebAuthenticationBroker();
            if (string.IsNullOrEmpty(code))
            {
                fooIEventAggregator.GetEvent<AuthEvent>().Publish(AuthEventEnum.身分驗證失敗);
            }
            else
            {
                var account = await ConvertCodeToAccount(code);
                await AuthenticationHelper.FetchUserProfile(account);
                fooIEventAggregator.GetEvent<AuthEvent>().Publish(AuthEventEnum.身分驗證成功);
            }
        }

        /// <summary>
        /// 進行 OAuth2 的認證
        /// </summary>
        /// <returns></returns>
        private async Task<string> AuthenticateUsingWebAuthenticationBroker()
        {
            OAuth2Authenticator auth;
            OAuth2AuthenticatorParameter fooPara = new OAuth2AuthenticatorParameter();

            #region 依據指定的 OAuth2 認證方式，取得該認證要用到的相關參數
            if (AuthenticationHelper.OAuthType == OAuthTypeEnum.Google)
            {
                fooPara = AuthenticationHelper.OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Google);
            }
            else if (AuthenticationHelper.OAuthType == OAuthTypeEnum.Facebook)
            {
                fooPara = AuthenticationHelper.OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Facebook);
            }
            #endregion

            var fooauthorizeUrl = fooPara.authorizeUrl;
            var fooredirectUrl = fooPara.redirectUrl;
            var fooaccessTokenUrl = fooPara.accessTokenUrl;
            var fooclientId = fooPara.clientId;
            var fooclientSecret = fooPara.clientSecret;
            var fooscope = fooPara.scope;

            #region 建立要取得 OAuth2 認證的URL
            var fooOAuthLoginUrl = fooauthorizeUrl + "?client_id=" +
                            Uri.EscapeDataString(fooclientId);
            fooOAuthLoginUrl += "&redirect_uri=" + Uri.EscapeDataString(fooredirectUrl);
            fooOAuthLoginUrl += "&response_type=code";
            fooOAuthLoginUrl += "&scope=" + Uri.EscapeDataString(fooscope);
            #endregion

            var startUri = new Uri(fooOAuthLoginUrl);

            var webAuthenticationResult =
              await
                WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, startUri,
                  new Uri(fooredirectUrl));
            return webAuthenticationResult.ResponseStatus != WebAuthenticationStatus.Success ? null : webAuthenticationResult.ResponseData.Substring(webAuthenticationResult.ResponseData.IndexOf('=') + 1);
        }

        /// <summary>
        /// 分析認證通過後的字串，並且取得使用者詳細資訊，最後需要建立 Account 物件
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private static async Task<Account> ConvertCodeToAccount(string code)
        {
            OAuth2AuthenticatorParameter fooPara = new OAuth2AuthenticatorParameter();

            #region 依據指定的 OAuth2 認證方式，取得該認證要用到的相關參數
            if (AuthenticationHelper.OAuthType == OAuthTypeEnum.Google)
            {
                fooPara = AuthenticationHelper.OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Google);
            }
            else if (AuthenticationHelper.OAuthType == OAuthTypeEnum.Facebook)
            {
                fooPara = AuthenticationHelper.OAuthParas.FirstOrDefault(x => x.Type == OAuthTypeEnum.Facebook);
            }
            #endregion

            var fooauthorizeUrl = fooPara.authorizeUrl;
            var fooredirectUrl = fooPara.redirectUrl;
            var fooaccessTokenUrl = fooPara.accessTokenUrl;
            var fooclientId = fooPara.clientId;
            var fooclientSecret = fooPara.clientSecret;
            var fooscope = fooPara.scope;

            #region 取得 使用者詳細資訊
            var httpClient = new System.Net.Http.HttpClient();
            var content = new FormUrlEncodedContent(new Dictionary<string, string> {
                { "code", code},
                { "client_id", fooPara.clientId},
                { "client_secret", fooPara.clientSecret},
                { "redirect_uri", fooPara.redirectUrl},
                { "grant_type", "authorization_code"},
            });
            var accessTokenResponse = await httpClient.PostAsync((fooPara.accessTokenUrl), content);
            var fooRetStr = await accessTokenResponse.Content.ReadAsStringAsync();
            #endregion

            var FooresponseDict = JsonConvert.DeserializeObject<AccessTokenResponse>(fooRetStr);
            var responseDict = new Dictionary<string, string>
            {
                {Constants.access_tokenAccountProperty,FooresponseDict.access_token},
                {Constants.expires_inAccountProperty,FooresponseDict.expires_in},
                {Constants.token_typeAccountProperty,FooresponseDict.token_type},
                //{"id_token",FooresponseDict.id_token},
            };

            return new Account(null, responseDict);
        }
    }
}

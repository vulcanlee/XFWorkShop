using System.Linq;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using Xamarin.Auth;
using XFoAuth2.Helpers;
using XFoAuth2.Models;
using UIKit;
using Prism.Unity;
using Xamarin.Forms;
using XFoAuth2.Views;
using XFoAuth2.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(oAuthPage), typeof(oAuthPageRenderer))]
namespace XFoAuth2.iOS
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
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null ||  Element == null)
                return;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if (_isShown) return;
            _isShown = true;

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
            auth = new OAuth2Authenticator(
                fooclientId,
                fooclientSecret,
                fooscope,
                new Uri(fooauthorizeUrl),
                new Uri(fooredirectUrl),
                new Uri(fooaccessTokenUrl));

            auth.Completed += OnAuthenticationCompleted;

            // Display the UI
            PresentViewController(auth.GetUI(), true, null);
        }

        /// <summary>
        /// 認證完成後的 Callback 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void OnAuthenticationCompleted(object sender, AuthenticatorCompletedEventArgs e)
        {
            // 取得 Prism 相依性服務使用到的容器
            IUnityContainer fooContainer = (XFoAuth2.App.Current as PrismApplication).Container;
            // 取得 IAccountStore 介面實際實作的類別物件
            var fooIEventAggregator = fooContainer.Resolve<IEventAggregator>();

            if (e.IsAuthenticated)
            {
                await AuthenticationHelper.FetchUserProfile(e.Account);
                fooIEventAggregator.GetEvent<AuthEvent>().Publish("Success");
            }
            else
            {
                fooIEventAggregator.GetEvent<AuthEvent>().Publish("Fail");
            }
        }
    }
}
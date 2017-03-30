using Prism.Unity;
using XFLocalNotificationDroid.Views;
using Xamarin.Forms;
using Prism.Navigation;

namespace XFLocalNotificationDroid
{
    public partial class App : PrismApplication
    {
        // 取得點選推播後，所送出的物件
        public static LocalNotificationPayload fooLocalNotificationPayload = null;

        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            if (fooLocalNotificationPayload == null)
            {
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
            }
            else if (fooLocalNotificationPayload.NavigationPage == "明細頁面")
            {
                #region 因為有要指名切換到另外一個頁面，所以，在這裡，將本地通知物件，傳遞到首頁頁面，由首頁頁面接續顯示道明細頁面
                var fooPara = new NavigationParameters();
                fooPara.Add("LocalNotification", fooLocalNotificationPayload);
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms", fooPara);
                #endregion
            }
            else
            {
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
            }
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<DetailPage>();
        }
    }
}

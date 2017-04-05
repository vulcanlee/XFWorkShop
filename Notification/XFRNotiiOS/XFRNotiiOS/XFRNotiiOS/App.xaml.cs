using Prism.Unity;
using XFRNotiiOS.Views;
using Xamarin.Forms;
using XFRNotiiOS.Models;
using Prism.Navigation;

namespace XFRNotiiOS
{
    public partial class App : PrismApplication
    {
        public static LocalNotificationPayload fooLocalNotificationPayload = null;
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            if (fooLocalNotificationPayload == null)
            {
                // 這裡是使用直接點選桌面圖示來啟動，要執行這段程式碼
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
            }
            else
            {
                // 在這裡加入通知的 Payload，並且將這個當作引數傳遞到主頁面，因此，主頁面於收到這個參數之後，就會自動換頁
                var fooPara = new NavigationParameters();
                fooPara.Add("LocalNotification", fooLocalNotificationPayload);
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms", fooPara);
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

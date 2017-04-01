using Prism.Unity;
using XFRNotiDroid.Views;
using Xamarin.Forms;
using XFRNotiDroid.Models;
using Prism.Navigation;

namespace XFRNotiDroid
{
    public partial class App : PrismApplication
    {
        public static NotificationPayload NotificationPayload = null;
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            // 判斷是否是點選推播通知，而開啟這個應用程式的
            if (NotificationPayload == null)
            {
                // 使用者自己點選應用程式圖示來啟動
                NavigationService.NavigateAsync("NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms");
            }
            else
            {
                // 使用者點選推播通知訊息，來啟動這葛應用程式
                var fooPara = new NavigationParameters();
                // 取出推播通知所傳送的訊息
                fooPara.Add("NotificationPayload", NotificationPayload);
                // 開啟首頁，設定要自動換頁，並且將推播通知內容，傳遞到該頁面內
                NavigationService.NavigateAsync($"NavigationPage/MainPage?title=Hello%20from%20Xamarin.Forms&AutoNext=NextPage", fooPara);
            }
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<NavigationPage>();
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<NextPage>();
        }
    }
}

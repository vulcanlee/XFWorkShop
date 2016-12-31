using Prism.Unity;
using XFQRCode.Views;

namespace XFQRCode
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();
            // 首頁頁面將採用可以導航的頁面
            NavigationService.NavigateAsync("NaviPage/MainPage?title=Hello%20from%20Xamarin.Forms");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<CodeScannerPage>();
            Container.RegisterTypeForNavigation<NaviPage>();
        }
    }
}

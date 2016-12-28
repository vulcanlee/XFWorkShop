using Prism.Unity;
using XFoAuth2.Helpers;
using XFoAuth2.Views;

namespace XFoAuth2
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer initializer = null) : base(initializer) { }

        protected override void OnInitialized()
        {
            InitializeComponent();

            // OAuth認證參數初始化設定
            AuthenticationHelper.Init();
            NavigationService.NavigateAsync("LoginPage");
            //NavigationService.NavigateAsync("MainPage?title=Hello%20from%20Xamarin.Forms");
        }

        protected override void RegisterTypes()
        {
            Container.RegisterTypeForNavigation<MainPage>();
            Container.RegisterTypeForNavigation<LoginPage>();
            Container.RegisterTypeForNavigation<oAuthPage>();
            Container.RegisterTypeForNavigation<HomePage>();
        }
    }
}

using Xamarin.Forms;

namespace XFNavigationTitle.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            //NavigationPage.SetBackButtonTitle(this, "回去了");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

        }
    }
}

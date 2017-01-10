using Xamarin.Forms;

namespace XFNavigationTitle.Views
{
    public partial class Next1Page : ContentPage
    {
        public Next1Page()
        {
            InitializeComponent();
            NavigationPage.SetBackButtonTitle(this, "上一頁");
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void Button_Clicked(object sender, System.EventArgs e)
        {
            var foo = NavigationPage.GetBackButtonTitle(this);
            NavigationPage.SetBackButtonTitle(this, "ddd");
            //NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}

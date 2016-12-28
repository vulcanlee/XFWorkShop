using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Auth;
using XFoAuth2.Services;
using XFoAuth2.Droid.Services;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(MyAndroidAccountStore))]
namespace XFoAuth2.Droid.Services
{
    /// <summary>
    /// 用於解決 Xamarin.Auth 套件，現階段(2016.12.20)無法使用於 UWP 平台所製作的介面
    /// 針對 Android 平台，並不需要做任何客製化，因為，Android平台將會使用 Xamarin.Auth提供的內建功能
    /// </summary>
    public class MyAndroidAccountStore : IAccountStore
    {
        public Task<Account> GetAccount()
        {
            return Task.FromResult<Account>(null);
        }

        public string GetPlatform()
        {
            return "Android";
        }

        public Task SaveAccount(Account account)
        {
            return Task.FromResult(0);
        }
    }
}
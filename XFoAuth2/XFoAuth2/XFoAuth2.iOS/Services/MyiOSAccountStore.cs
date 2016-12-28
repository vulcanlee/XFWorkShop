using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using XFoAuth2.iOS.Services;
using XFoAuth2.Services;


[assembly: Xamarin.Forms.Dependency(typeof(MyiOSAccountStore))]
namespace XFoAuth2.iOS.Services
{
    /// <summary>
    /// 用於解決 Xamarin.Auth 套件，現階段(2016.12.20)無法使用於 UWP 平台所製作的介面
    /// 針對 iOS 平台，並不需要做任何客製化，因為，iOS 平台將會使用 Xamarin.Auth提供的內建功能
    /// </summary>
    class MyiOSAccountStore : IAccountStore
    {
        public Task<Account> GetAccount()
        {
            return Task.FromResult<Account>(null);
        }

        public string GetPlatform()
        {
            return "iOS";
        }

        public Task SaveAccount(Account account)
        {
            return Task.FromResult(0);
        }
    }
}
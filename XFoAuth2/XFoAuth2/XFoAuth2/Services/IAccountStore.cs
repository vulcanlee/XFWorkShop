using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;

namespace XFoAuth2.Services
{
    /// <summary>
    /// 用於解決 Xamarin.Auth 套件，現階段(2016.12.20)無法使用於 UWP 平台所製作的介面
    /// 是要用來儲存使用者認證完成後的相關資訊儲存用到的方法
    /// </summary>
    public interface IAccountStore
    {
        /// <summary>
        /// 可以在核心PCL專案內，取得正在執行的原生專案名稱
        /// </summary>
        /// <returns></returns>
        string GetPlatform();
        /// <summary>
        /// 取得通過認證的 Xamarin.Auth 帳號物件
        /// </summary>
        /// <returns></returns>
        Task<Account> GetAccount();
        /// <summary>
        /// 儲存通過認證的 Xamarin.Auth 帳號物件
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        Task SaveAccount(Account account);
    }
}

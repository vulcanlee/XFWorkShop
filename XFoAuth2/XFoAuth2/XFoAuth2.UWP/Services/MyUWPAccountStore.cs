using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Auth;
using XFoAuth2.Helpers;
using XFoAuth2.Models;
using XFoAuth2.Services;
using XFoAuth2.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(MyUWPAccountStore))]
namespace XFoAuth2.UWP.Services
{
    /// <summary>
    /// 用於解決 Xamarin.Auth 套件，現階段(2016.12.20)無法使用於 UWP 平台所製作的介面
    /// 針對 UWP 平台，需要時做這個介面的方法
    /// </summary>
    public class MyUWPAccountStore : IAccountStore
    {
        public async Task<Account> GetAccount()
        {
            Account fooAccount = null;

            #region 使用 UWP 提供的 IsolatedStorage 功能，從檔案中取出使用者認證過的相關明細資訊
            try
            {
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile = await storageFolder.GetFileAsync("account");
                var fooStr = await Windows.Storage.FileIO.ReadTextAsync(sampleFile);

                var fooAccessTokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(fooStr);
                var responseDict = new Dictionary<string, string>
            {
                {Constants.access_tokenAccountProperty,fooAccessTokenResponse.access_token},
                {Constants.expires_inAccountProperty,fooAccessTokenResponse.expires_in},
                {Constants.token_typeAccountProperty,fooAccessTokenResponse.token_type},
                {Constants.NameAccountProperty,fooAccessTokenResponse.Name},
                {Constants.EmailAccountProperty,fooAccessTokenResponse.email},
                {Constants.PhotoAccountProperty,fooAccessTokenResponse.photo},
                {Constants.LoginTypeAccountProperty,fooAccessTokenResponse.LoginType},
            };

                fooAccount = new Account(null, responseDict);
            }
            catch { }
            #endregion

            return fooAccount;
        }

        public string GetPlatform()
        {
            // 回報現在使用的平台是 UWP
            return "UWP";
        }

        public async Task SaveAccount(Account account)
        {
            var fooAccessTokenResponse = new AccessTokenResponse();

            #region 使用 UWP 提供的 IsolatedStorage 功能，將使用者認證過的相關明細資訊寫入到檔案中
            fooAccessTokenResponse.Name = account.Properties[Constants.NameAccountProperty];
            fooAccessTokenResponse.email = account.Properties[Constants.EmailAccountProperty];
            fooAccessTokenResponse.photo = account.Properties[Constants.PhotoAccountProperty];
            fooAccessTokenResponse.LoginType = account.Properties[Constants.LoginTypeAccountProperty];
            fooAccessTokenResponse.access_token = account.Properties[Constants.access_tokenAccountProperty];
            fooAccessTokenResponse.expires_in = account.Properties[Constants.expires_inAccountProperty];
            fooAccessTokenResponse.token_type = account.Properties[Constants.token_typeAccountProperty];

            var fooStr = JsonConvert.SerializeObject(fooAccessTokenResponse);

            Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            Windows.Storage.StorageFile sampleFile = await storageFolder.CreateFileAsync("account", Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(sampleFile, fooStr);
            #endregion
            return;
        }
    }
}

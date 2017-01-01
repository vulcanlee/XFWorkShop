using System;
using System.Collections.Generic;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml;
using XFFiles.Services;
using XFFiles.UWP.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NatvieAssyFile))]
namespace XFFiles.UWP.Services
{
    public class NatvieAssyFile : INatvieAssyFile
    {
        public string GetFileContent(string filename)
        {
            string fooResult;

            // 使用所指定的 URI ，取得該檔案的內容
            // https://msdn.microsoft.com/library/windows/apps/windows.storage.storagefile.getfilefromapplicationuriasync.aspx
            var fooStorageFile = StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///Assets/{filename}", UriKind.Absolute)).GetResults();
            fooResult = Windows.Storage.FileIO.ReadTextAsync(fooStorageFile).GetResults();

            return fooResult;
        }

        public string GetPlatformName()
        {
            return "UWP";
        }
    }
}

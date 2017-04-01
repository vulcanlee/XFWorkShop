using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFRNotiDroid.Helpers
{
    public class GlobalHelper
    {
        // Replace strings with your mobile services and gateway URLs.
        public static string ApplicationURL = @"https://xamarinazureday.azurewebsites.net";

        public static MobileServiceClient AzureMobileClient = new MobileServiceClient(ApplicationURL);

    }
}

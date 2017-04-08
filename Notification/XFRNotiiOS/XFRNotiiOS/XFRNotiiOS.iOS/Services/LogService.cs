using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using XFRNotiiOS.Services;
using XFRNotiiOS.iOS.Services;

[assembly: Xamarin.Forms.Dependency(typeof(LogService))]
namespace XFRNotiiOS.iOS.Services
{
    public class LogService : ILogService
    {
        public string Read()
        {
            string fooResult = NSUserDefaults.StandardUserDefaults.StringForKey("RemoteNotificationTest");
            if (string.IsNullOrEmpty(fooResult))
            {
                fooResult = "";
            }
            return fooResult;
        }

        public void Write(string Content)
        {
            //retreive 
            string fooResult = Read();
            fooResult = $"{Content}\r\n{fooResult}";
            if (string.IsNullOrEmpty(Content) == true)
            {
                fooResult = "";
            }
            NSUserDefaults.StandardUserDefaults.SetString(fooResult, "RemoteNotificationTest");
        }
    }
}
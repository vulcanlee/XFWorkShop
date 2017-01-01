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
using XFFiles.Services;
using XFFiles.Droid.Services;

[assembly: Xamarin.Forms.Dependency(typeof(NatvieAssyFile))]
namespace XFFiles.Droid.Services
{
    public class NatvieAssyFile : INatvieAssyFile
    {
        public string GetFileContent(string filename)
        {
            string fooResult;

            using (var stream = new System.IO.StreamReader(Application.Context.Assets.Open(filename)))
            {
                fooResult = stream.ReadToEnd();
            }

            return fooResult;
        }

        public string GetPlatformName()
        {
            return "Android";
        }
    }
}
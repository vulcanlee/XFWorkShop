using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XFFiles.iOS.Services;
using XFFiles.Services;


[assembly: Xamarin.Forms.Dependency(typeof(NatvieAssyFile))]
namespace XFFiles.iOS.Services
{
    class NatvieAssyFile : INatvieAssyFile
    {
        public string GetFileContent(string filename)
        {
            string fooResult;

            fooResult = System.IO.File.ReadAllText(filename);

            return fooResult;
        }

        public string GetPlatformName()
        {
            return "iOS";
        }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XFRNotiiOS.Models;

namespace ConvertNotification
{
    class Program
    {
        static void Main(string[] args)
        {
            var fooLocalNotificationPayload = new LocalNotificationPayload
            {
                NavigationPage = "DetailPage",
                OtherInformation = "這裡可以放入其他額外資訊",
            };

            var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(fooLocalNotificationPayload));
            var base64 = Convert.ToBase64String(bytes);

            Console.WriteLine(base64);
            Console.ReadKey();
        }
    }
}

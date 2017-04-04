using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Prism.Events;
using Newtonsoft.Json;

namespace XFLocalNotificationiOS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        IUnityContainer myContainer;

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();


            #region 這個，將會當該應用程式在背景或者沒有執行的時候，且使用點選通知後，將會被執行

            // -----------------------------------------------
            // 這裡的方法，需要在  LoadApplication(new App(new iOSInitializer())) 程序前先執行
            // -----------------------------------------------

            //  系統版本是否大於或等於指定的主要和次要值.
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                app.RegisterUserNotificationSettings(notificationSettings);
            }

            #region 檢查此次啟動應用程式，是否因為點選了通知的關係
            if (options != null)
            {
                #region 若有本地端的通知 Payload 傳入，需要取出這些資訊，並且
                if (options.ContainsKey(UIApplication.LaunchOptionsLocalNotificationKey))
                {
                    var localNotification = options[UIApplication.LaunchOptionsLocalNotificationKey] as UILocalNotification;
                    if (localNotification != null)
                    {
                        // 底下為顯示出本地端的對話窗
                        //UIAlertView avAlert = new UIAlertView(localNotification.AlertAction, localNotification.AlertBody, null, "OK", null);
                        //avAlert.Show();

                        // 設定徽章為 0
                        UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;

                        #region 將 Payload 設定 核心PCL 的 App 類別中，所以，當應用程式一開啟的時候，就會自動切換到指定頁面
                        if (localNotification.UserInfo.ContainsKey(new NSString("LocalNotification")))
                        {
                            // 取出這個通知的額外夾帶 Payload
                            var fooPayloadtmp = localNotification.UserInfo[new NSString("LocalNotification")];
                            // 將夾帶的 Payload 的 JSON 字串取出來
                            var fooPayload = fooPayloadtmp.ToString();

                            // 將 JSON 字串反序列化，並送到 核心PCL 
                            LocalNotificationPayload fooLocalNotificationPayload = JsonConvert.DeserializeObject<LocalNotificationPayload>(fooPayload);

                            // 設定這個應用程式冷啟動的時候，將會依據 Payload 的內容，切換到指定頁面內
                            XFLocalNotificationiOS.App.fooLocalNotificationPayload = fooLocalNotificationPayload;
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion
            #endregion

            LoadApplication(new App(new iOSInitializer()));

            #region 訂閱要發送本地通知的事件(事件將會從 核心PCL 送出)
            // 取得 Xamarin.Forms 中的 Prism 注入物件管理容器
            myContainer = (App.Current as PrismApplication).Container;
            var fooEvent = myContainer.Resolve<IEventAggregator>().GetEvent<LocalNotificationEvent>().Subscribe(x =>
            {
                #region 建立本地訊息通知物件
                // 建立通知物件
                var notification = new UILocalNotification();

                // 設定顯示通知的延遲時間
                notification.FireDate = NSDate.FromTimeIntervalSinceNow(10);

                // 設定通知的主題與內容
                notification.AlertAction = x.ContentTitle;
                notification.AlertBody = x.ContentText;

                // 修改徽章數值
                notification.ApplicationIconBadgeNumber = 68;

                // 使用預設音效來撥放聲音
                notification.SoundName = UILocalNotification.DefaultSoundName;

                #region 加入額外的 Payload 資訊到此次的通知中
                NSMutableDictionary dict = new NSMutableDictionary();
                var fooPayload = JsonConvert.SerializeObject(x);
                dict.Add(new NSString("LocalNotification"), new NSString(fooPayload));
                notification.UserInfo = dict;
                #endregion

                #region 啟動這個預約通知對話窗
                UIApplication.SharedApplication.ScheduleLocalNotification(notification);
                Console.WriteLine("Scheduled...");
                #endregion
                #endregion
            });
            #endregion

            return base.FinishedLaunching(app, options);
        }

        /// <summary>
        /// 這個方法，將會於該應用程式在前景的時候，並且有本地端通知出現的時候，將會被執行
        /// </summary>
        /// <param name="application"></param>
        /// <param name="notification"></param>
        public override void ReceivedLocalNotification(UIApplication application, UILocalNotification notification)
        {
            #region 因為應用程式正在前景，所以，顯示一個提示訊息對話窗
            UIAlertView avAlert = new UIAlertView(notification.AlertAction, notification.AlertBody, null, "OK", null);
            avAlert.Show();
            #endregion

            #region 重新設定徽章為 0
            UIApplication.SharedApplication.ApplicationIconBadgeNumber = 0;
            #endregion

            #region 使用 Prism 事件聚合器，送訊息給 核心PCL，切換到所指定的頁面
            if (notification.UserInfo.ContainsKey(new NSString("LocalNotification")))
            {
                // 取出這個通知的額外夾帶 Payload
                var fooPayloadtmp = notification.UserInfo[new NSString("LocalNotification")];
                // 將夾帶的 Payload 的 JSON 字串取出來
                var fooPayload = fooPayloadtmp.ToString();
                // 將 JSON 字串反序列化，並送到 核心PCL 
                LocalNotificationPayload fooLocalNotificationPayload = JsonConvert.DeserializeObject<LocalNotificationPayload>(fooPayload);
                myContainer.Resolve<IEventAggregator>().GetEvent<LocalNotificationToPCLEvent>().Publish(fooLocalNotificationPayload);
            }
            #endregion
        }

    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }

}

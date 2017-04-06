using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using Microsoft.WindowsAzure.MobileServices;
using XFRNotiiOS.Helpers;
using Newtonsoft.Json;
using XFRNotiiOS.Models;
using AudioToolbox;
using System.Text;
using Prism.Events;
using System.Threading.Tasks;
using System.Threading;
using PCLStorage;
using System.IO;

namespace XFRNotiiOS.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        IUnityContainer myContainer;
        bool CoolStartApp = true;
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

            #region 要使用者允許接收通知之設定
            //  系統版本是否大於或等於指定的主要和次要值.
            if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
            {
                var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
                    UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null
                );

                app.RegisterUserNotificationSettings(notificationSettings);
                // 這能夠支援遠端通知，並要求推播註冊
                UIApplication.SharedApplication.RegisterForRemoteNotifications();
            }
            else
            {
                UIRemoteNotificationType notificationTypes = UIRemoteNotificationType.Alert | UIRemoteNotificationType.Badge | UIRemoteNotificationType.Sound;
                UIApplication.SharedApplication.RegisterForRemoteNotificationTypes(notificationTypes);
            }
            #endregion

            #region 檢查此次啟動應用程式，是否因為點選了通知的關係
            if (options != null)
            {
                #region 若有本地端的通知 Payload 傳入，需要取出這些資訊，並且
                if (options.ContainsKey(UIApplication.LaunchOptionsRemoteNotificationKey))
                {
                    //var foo4 = options.ToString();
                    //UIAlertView avAlert4 = new UIAlertView("Re-Check", fooO, null, "OK", null);
                    //avAlert4.Show();

                    var OptionNotification = options[UIApplication.LaunchOptionsRemoteNotificationKey] as NSDictionary;
                    if (OptionNotification != null)
                    {
                        #region 將 Payload 設定 核心PCL 的 App 類別中，所以，當應用程式一開啟的時候，就會自動切換到指定頁面
                        if (OptionNotification.ContainsKey(new NSString("aps")))
                        {
                            try
                            {
                                // 取出這個通知的額外夾帶 aps Payload
                                var aps = OptionNotification[new NSString("aps")] as NSDictionary;

                                #region 取出 asp 內的兩個項目 alert / args
                                string alert = string.Empty;
                                string args = string.Empty;
                                if (aps.ContainsKey(new NSString("alert")))
                                    alert = (aps[new NSString("alert")] as NSString).ToString();

                                if (aps.ContainsKey(new NSString("args")))
                                    args = (aps[new NSString("args")] as NSString).ToString();
                                #endregion

                                // 將夾帶的 Payload 的 JSON 字串取出來
                                var fooPayload = args;

                                // 將 JSON 字串反序列化，並送到 核心PCL 
                                var fooFromBase64 = Convert.FromBase64String(fooPayload);
                                fooPayload = Encoding.UTF8.GetString(fooFromBase64);

                                LocalNotificationPayload fooLocalNotificationPayload = JsonConvert.DeserializeObject<LocalNotificationPayload>(fooPayload);
                                // 設定這個應用程式冷啟動的時候，將會依據 Payload 的內容，切換到指定頁面內
                                XFRNotiiOS.App.fooLocalNotificationPayload = fooLocalNotificationPayload;
                            }
                            catch (Exception ex)
                            {
                                UIAlertView avAlertEx = new UIAlertView("Exception", ex.Message, null, "OK", null);
                                avAlertEx.Show();
                            }
                        }
                        else
                        {
                            UIAlertView avAlert168 = new UIAlertView("aps", "Not Found", null, "OK", null);
                            avAlert168.Show();
                        }
                        #endregion
                    }
                }
                #endregion
            }
            #endregion

            LoadApplication(new App(new iOSInitializer()));

            #region 延緩 1.5 秒，不要讓冷啟動 App的同時，DidReceiveRemoteNotification 也會一併執行
            Task.Factory.StartNew(() =>
            {

                Thread.Sleep(1500);
                CoolStartApp = false;
            });
            #endregion

            #region 取得 Xamarin.Forms 中的 Prism 注入物件管理容器
            myContainer = (App.Current as PrismApplication).Container;
            #endregion

            #region 檢測點
            myContainer.Resolve<IEventAggregator>().GetEvent<UpdateInfoEvent>().Publish(new UpdateInfoEventPayload
            {
                Name = "FinishedLaunching",
                time = DateTime.Now,
            });

            WriteNotificationLog("FinishedLaunching" + DateTime.Now.ToString());
            #endregion
            return base.FinishedLaunching(app, options);

        }

        /// <summary>
        /// 這台裝置與 APNS 註冊完成後，會執行底下方法，需要將 deviceToken 送到 NotificationHub 推播中樞 來進行 Azure Notificatio Hub 的註冊
        /// </summary>
        /// <param name="application"></param>
        /// <param name="deviceToken"></param>
        public override async void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            #region 檢測點
            myContainer.Resolve<IEventAggregator>().GetEvent<UpdateInfoEvent>().Publish(new UpdateInfoEventPayload
            {
                Name = "RegisteredForRemoteNotifications",
                time = DateTime.Now,
            });
            WriteNotificationLog("RegisteredForRemoteNotifications" + DateTime.Now.ToString());
            #endregion

            #region 建立要接收的推播格式
            //string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
            //string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\", \"args\":\"$(argsParam)\"}}";
            string templateBodyAPNS = "{\"aps\":{\"alert\":\"$(messageParam)\", \"sound\" : \"default\", \"args\":\"$(argsParam)\"}}";

            JObject templates = new JObject();
            templates["genericMessage"] = new JObject
            {
                { "body", templateBodyAPNS}
            };
            #endregion

            #region 註冊新的 DeviceToken 到 Azure 推播中樞內
            #region 取得現在的 DeviceToken
            var DeviceToken = deviceToken.Description;
            if (!string.IsNullOrWhiteSpace(DeviceToken))
            {
                DeviceToken = DeviceToken.Trim('<').Trim('>');
                DeviceToken = DeviceToken.Replace(" ", String.Empty);
            }
            #endregion

            #region 取得先前的 DeviceToken
            // https://developer.apple.com/reference/foundation/nsuserdefaults/1416603-standarduserdefaults
            var oldDeviceToken = NSUserDefaults.StandardUserDefaults.StringForKey("PushDeviceToken");
            NSUserDefaults.StandardUserDefaults.SetString(DeviceToken, "PushDeviceToken");
            #endregion

            #region DeviceToken 是否有變動過
            if (string.IsNullOrEmpty(oldDeviceToken) || !oldDeviceToken.Equals(DeviceToken))
            {
                // 在這裡可以處理當DeviceToken沒有異動時，無須再進行註冊
                //Push push = GlobalHelper.AzureMobileClient.GetPush();
                //await push.RegisterAsync(deviceToken, templates);
            }
            Push push = GlobalHelper.AzureMobileClient.GetPush();
            await push.RegisterAsync(deviceToken, templates);
            #endregion
            #endregion
        }

        public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
        {
            #region 檢測點
            myContainer.Resolve<IEventAggregator>().GetEvent<UpdateInfoEvent>().Publish(new UpdateInfoEventPayload
            {
                Name = "FailedToRegisterForRemoteNotifications",
                time = DateTime.Now,
            });
            WriteNotificationLog("FailedToRegisterForRemoteNotifications" + DateTime.Now.ToString());
            #endregion

            var alert = new UIAlertView("警告", "註冊 APNS 失敗", null, "OK", null);
            alert.Show();
        }

        /// <summary>
        /// 當應用程式執行時，此方法會處理傳入的通知
        /// </summary>
        /// <param name="application"></param>
        /// <param name="userInfo"></param>
        /// <param name="completionHandler"></param>
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            #region 檢測點
            myContainer.Resolve<IEventAggregator>().GetEvent<UpdateInfoEvent>().Publish(new UpdateInfoEventPayload
            {
                Name = "DidReceiveRemoteNotification",
                time = DateTime.Now,
            });
            WriteNotificationLog("DidReceiveRemoteNotification" + DateTime.Now.ToString());
            #endregion

            //if (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Background || (UIApplication.SharedApplication.ApplicationState == UIApplicationState.Inactive ))
            //{
            //completionHandler(UIBackgroundFetchResult.NewData);
            //    return;
            //}

            if (CoolStartApp == true)
            {
                return;
            }

            if (userInfo.ContainsKey(new NSString("aps")))
            {
                try
                {
                    NSDictionary aps = userInfo.ObjectForKey(new NSString("aps")) as NSDictionary;

                    #region 取出相關推播通知的 Payload
                    string alert = string.Empty;
                    string args = string.Empty;
                    if (aps.ContainsKey(new NSString("alert")))
                        alert = (aps[new NSString("alert")] as NSString).ToString();

                    if (aps.ContainsKey(new NSString("args")))
                        args = (aps[new NSString("args")] as NSString).ToString();
                    #endregion

                    #region 因為應用程式正在前景，所以，顯示一個提示訊息對話窗
                    if (!string.IsNullOrEmpty(args))
                    {
                        SystemSound.Vibrate.PlaySystemSound();
                        UIAlertView avAlert = new UIAlertView("Notification", alert, null, "OK", null);
                        avAlert.Show();

                        #region 使用 Prism 事件聚合器，送訊息給 核心PCL，切換到所指定的頁面
                        if (string.IsNullOrEmpty(args) == false)
                        {
                            // 將夾帶的 Payload 的 JSON 字串取出來
                            var fooPayload = args;

                            // 將 JSON 字串反序列化，並送到 核心PCL 
                            var fooFromBase64 = Convert.FromBase64String(fooPayload);
                            fooPayload = Encoding.UTF8.GetString(fooFromBase64);

                            LocalNotificationPayload fooLocalNotificationPayload = JsonConvert.DeserializeObject<LocalNotificationPayload>(fooPayload);

                            myContainer.Resolve<IEventAggregator>().GetEvent<LocalNotificationToPCLEvent>().Publish(fooLocalNotificationPayload);
                        }
                        #endregion
                    }
                    #endregion
                }
                catch { }
            }
        }


        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            #region 檢測點
            myContainer.Resolve<IEventAggregator>().GetEvent<UpdateInfoEvent>().Publish(new UpdateInfoEventPayload
            {
                Name = "ReceivedRemoteNotification",
                time = DateTime.Now,
            });
            WriteNotificationLog("ReceivedRemoteNotification" + DateTime.Now.ToString());
            #endregion

            //NSObject inAppMessage;

            //var alert = new UIAlertView("Got push notification", "ReceivedRemoteNotification", null, "OK", null);
            //alert.Show();
            //bool success = userInfo.TryGetValue(new NSString("inAppMessage"), out inAppMessage);

            //if (success)
            //{
            //    var alert = new UIAlertView("Got push notification", inAppMessage.ToString(), null, "OK", null);
            //    alert.Show();
            //}
            //var success = userInfo.ToString();

        }

        #region Notification Log 的檔案讀寫
        object thisLock = new object();
        public string ReadNotificationLog()
        {
            string fooFilename = "NotificationLog.txt";
            string fooContent = "";
            IFile file;

            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "../Library/Logs");
            var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "../Library/Logs");
            var filename = Path.Combine(folder, fooFilename);
            try
            {
                Directory.CreateDirectory(folder);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            try
            {
                fooContent = File.ReadAllText(filename);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            //IFolder rootFolder = FileSystem.Current.LocalStorage;
            //IFolder folder =  rootFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists).Result;
            //var fooResult =  folder.CheckExistsAsync(fooFilename).Result;
            //if (fooResult == ExistenceCheckResult.NotFound)
            //{
            //    file =  folder.CreateFileAsync(fooFilename, CreationCollisionOption.ReplaceExisting).Result;
            //}
            //else
            //{
            //    file =  folder.GetFileAsync(fooFilename).Result;
            //    fooContent =  file.ReadAllTextAsync().Result;
            //}
            return fooContent;
        }

        public void WriteNotificationLog(string fooContent)
        {
            string fooFilename = "NotificationLog.txt";
            IFile file;

            lock (thisLock)
            {
                var folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), "../Library/Logs");
                var filename = Path.Combine(folder, fooFilename);

                var fooReadContent = ReadNotificationLog();
                var fooObj = $"{fooReadContent}\r\n{fooContent}";
                File.WriteAllText(filename, fooObj);

                //var fooReadContent = ReadNotificationLog();
                //var fooObj = $"{fooReadContent}\r\n{fooContent}";
                //IFolder rootFolder = FileSystem.Current.LocalStorage;
                //IFolder folder = rootFolder.CreateFolderAsync("Logs", CreationCollisionOption.OpenIfExists).Result;
                //file = folder.CreateFileAsync(fooFilename, CreationCollisionOption.ReplaceExisting).Result;
                //file.WriteAllTextAsync(fooObj).Wait();
            }
            return;
        }
        #endregion
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }

}

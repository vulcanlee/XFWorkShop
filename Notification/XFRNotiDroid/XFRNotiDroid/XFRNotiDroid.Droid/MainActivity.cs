using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Gcm.Client;
using XFRNotiDroid.Droid.Services;
using Android.Content;
using XFRNotiDroid.Models;
using Newtonsoft.Json;

namespace XFRNotiDroid.Droid
{
    [Activity(Label = "XFRNotiDroid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        #region MainActivity Instance
        // Create a new instance field for this activity.
        static MainActivity instance = null;

        // Return the current activity instance.
        public static MainActivity CurrentActivity
        {
            get
            {
                return instance;
            }
        }

        #endregion

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            #region 設定該應用程式的主要 Activity
            instance = this;
            #endregion

            #region 檢查是否是由通知開啟 App，並且依據通知，切換到適當頁面

            //自訂通知被點擊後的動作
            //當通知出現在通知欄之後，在習慣的趨使下，有很大的機率會被使用者點擊。
            //所以應該要實作出通知被點擊後的動作，好比開啟哪個Activity之類的。
            //通知被點擊後的動作可以使用PendingIntent來實作，PendingIntent並不是一個Intent，它是一個Intent的容器，
            // 可以傳入context物件，並以這個context的身份來做一些事情，例如開啟Activity、開啟Service，或是發送Broadcast。
            // 如果要使通知可以在被點擊之後做點什麼事，可以使用Notification.Builder的setContentIntent方法來替通知加入PendingIntent

            NotificationPayload fooNotificationPayload = null;
            if (Intent.Extras != null)
            {
                if (Intent.Extras.ContainsKey("NotificationPayload"))
                {
                    string fooNotificationObject = Intent.Extras.GetString("NotificationPayload", "");
                    fooNotificationPayload = JsonConvert.DeserializeObject<NotificationPayload>(fooNotificationObject);
                }
            }

            // 將讀取到的推播通知內容，儲存到 PCL 專案內
            App.NotificationPayload = fooNotificationPayload;
            #endregion

            global::Xamarin.Forms.Forms.Init(this, bundle);

            #region 進行 Azure Mobile Client 套件初始化
            Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
            #endregion

            LoadApplication(new App(new AndroidInitializer()));

            #region Firebase 的推播設定用程式碼
            try
            {
                // 確定 GcmClinet 的需求都有設定完成
                GcmClient.CheckDevice(this);
                GcmClient.CheckManifest(this);

                // 進行遠端推播通知的註冊(含註冊 Azure Mobile App)
                System.Diagnostics.Debug.WriteLine("Registering...");
                GcmClient.Register(this, PushHandlerBroadcastReceiver.SENDER_IDS);


            }
            catch (Java.Net.MalformedURLException)
            {
                CreateAndShowDialog("There was an error creating the client. Verify the URL.", "Error");
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e.Message, "Error");
            }
            #endregion
        }

        #region Firebase 的推播設定用程式碼
        private void CreateAndShowDialog(String message, String title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
        #endregion

    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}


using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;
using Prism.Events;
using Android.Graphics;
using Android.Content;
using Newtonsoft.Json;

namespace XFLocalNotificationDroid.Droid
{
    [Activity(Label = "XFLocalNotificationDroid", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        // Notification ID 將會套用在這個應用程式所發出的所有通知訊息
        // 重複使用相同的 notification ID 將會覆蓋掉之前已經送出的通知訊息
        int notificationId = 1;

         int pendingIntentId = 0;

        // 取得點選推播後，所送出的物件
        LocalNotificationPayload fooLocalNotificationPayload;

        // 取得 notifications manager:
        NotificationManager notificationManager;

        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.tabs;
            ToolbarResource = Resource.Layout.toolbar;

            base.OnCreate(bundle);

            notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;

            global::Xamarin.Forms.Forms.Init(this, bundle);
            var fooApp = new App(new AndroidInitializer());

            #region 檢查是否是由通知開啟 App，並且依據通知，切換到適當頁面
            fooLocalNotificationPayload = null;
            if (Intent.Extras != null)
            {
                if (Intent.Extras.ContainsKey("NotificationObject"))
                {
                    string fooNotificationObject = Intent.Extras.GetString("NotificationObject", "");
                    fooLocalNotificationPayload = JsonConvert.DeserializeObject<LocalNotificationPayload>(fooNotificationObject);
                }
            }
            #endregion

            XFLocalNotificationDroid.App.fooLocalNotificationPayload = fooLocalNotificationPayload;
            LoadApplication(new App(new AndroidInitializer()));

            #region 訂閱要發送本地通知的事件
            // 取得 Xamarin.Forms 中的 Prism 注入物件管理容器
            IUnityContainer myContainer = (App.Current as PrismApplication).Container;
            var fooEvent = myContainer.Resolve<IEventAggregator>().GetEvent<LocalNotificationEvent>().Subscribe(x =>
            {
                #region 建立本地訊息通知物件
                Notification.Builder builder = new Notification.Builder(this)
                    .SetContentTitle(x.ContentTitle)
                    .SetContentText(x.ContentText)
                    .SetSmallIcon(Resource.Drawable.ic_notification)
                    .SetAutoCancel(true);

                // 決定是否要顯示大圖示
                if (x.LargeIcon)
                {
                    builder.SetLargeIcon(BitmapFactory.DecodeResource(Resources, Resource.Drawable.monkey_icon));
                }

                #region 針對不同通知類型作出設定
                switch (x.Style)
                {
                    case LocalNotificationStyleEnum.Normal:
                        break;
                    case LocalNotificationStyleEnum.BigText:
                        //builder.SetContentText(x.ContentText);
                        var textStyle = new Notification.BigTextStyle();
                        textStyle.BigText(x.ContentText);
                        textStyle.SetSummaryText(x.SummaryText);

                        builder.SetStyle(textStyle);
                        break;

                    case LocalNotificationStyleEnum.Inbox:
                        var inboxStyle = new Notification.InboxStyle();
                        foreach (var item in x.InboxStyleList)
                        {
                            inboxStyle.AddLine(item);
                        }
                        inboxStyle.SetSummaryText(x.SummaryText);

                        builder.SetStyle(inboxStyle);
                        break;

                    case LocalNotificationStyleEnum.Image:
                        var picStyle = new Notification.BigPictureStyle();
                        picStyle.BigPicture(BitmapFactory.DecodeResource(Resources, Resource.Drawable.x_bldg));
                        picStyle.SetSummaryText(x.SummaryText);

                        builder.SetStyle(picStyle);
                        break;
                    default:
                        break;
                }
                #endregion

                #region 設定 visibility
                switch (x.Visibility)
                {
                    case LocalNotificationVisibilityEnum.Public:
                        builder.SetVisibility(NotificationVisibility.Public);
                        break;
                    case LocalNotificationVisibilityEnum.Private:
                        builder.SetVisibility(NotificationVisibility.Private);
                        break;
                    case LocalNotificationVisibilityEnum.Secret:
                        builder.SetVisibility(NotificationVisibility.Secret);
                        break;
                    default:
                        break;
                }
                #endregion

                #region 設定 priority
                switch (x.Priority)
                {
                    case LocalNotificationPriorityEnum.Default:
                        builder.SetPriority((int)NotificationPriority.Default);
                        break;
                    case LocalNotificationPriorityEnum.High:
                        builder.SetPriority((int)NotificationPriority.High);
                        break;
                    case LocalNotificationPriorityEnum.Low:
                        builder.SetPriority((int)NotificationPriority.Low);
                        break;
                    case LocalNotificationPriorityEnum.Maximum:
                        builder.SetPriority((int)NotificationPriority.Max);
                        break;
                    case LocalNotificationPriorityEnum.Minimum:
                        builder.SetPriority((int)NotificationPriority.Min);
                        break;
                    default:
                        break;
                }
                #endregion

                #region 設定 category
                switch (x.Category)
                {
                    case LocalNotificationCategoryEnum.Call:
                        builder.SetCategory(LocalNotificationCategoryEnum.Call.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Message:
                        builder.SetCategory(LocalNotificationCategoryEnum.Message.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Alarm:
                        builder.SetCategory(LocalNotificationCategoryEnum.Alarm.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Email:
                        builder.SetCategory(LocalNotificationCategoryEnum.Email.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Event:
                        builder.SetCategory(LocalNotificationCategoryEnum.Event.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Promo:
                        builder.SetCategory(LocalNotificationCategoryEnum.Promo.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Progress:
                        builder.SetCategory(LocalNotificationCategoryEnum.Progress.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Social:
                        builder.SetCategory(LocalNotificationCategoryEnum.Social.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Error:
                        builder.SetCategory(LocalNotificationCategoryEnum.Error.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Transport:
                        builder.SetCategory(LocalNotificationCategoryEnum.Transport.ToString());
                        break;
                    case LocalNotificationCategoryEnum.System:
                        builder.SetCategory(LocalNotificationCategoryEnum.System.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Service:
                        builder.SetCategory(LocalNotificationCategoryEnum.Service.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Recommendation:
                        builder.SetCategory(LocalNotificationCategoryEnum.Recommendation.ToString());
                        break;
                    case LocalNotificationCategoryEnum.Status:
                        builder.SetCategory(LocalNotificationCategoryEnum.Status.ToString());
                        break;
                    default:
                        break;
                }
                #endregion

                #region 準備設定當使用者點選通知之後要做的動作
                // Setup an intent for SecondActivity:
                Intent secondIntent = new Intent(this, typeof(MainActivity));

                // 設定當使用點選這個通知之後，要傳遞過去的資料
                secondIntent.PutExtra("NotificationObject", JsonConvert.SerializeObject(x));

                // 若在首頁且使用者按下回上頁實體按鈕，則會離開這個 App
                TaskStackBuilder stackBuilder = TaskStackBuilder.Create(this);

                stackBuilder.AddNextIntent(secondIntent);
                PendingIntent pendingIntent = stackBuilder.GetPendingIntent(pendingIntentId++, PendingIntentFlags.OneShot);

                // Uncomment this code to setup an intent so that notifications return to this app:
                // Intent intent = new Intent (this, typeof(MainActivity));
                // const int pendingIntentId = 0;
                // pendingIntent = PendingIntent.GetActivity (this, pendingIntentId, intent, PendingIntentFlags.OneShot);
                // builder.SetContentText("Hello World! This is my first action notification!");

                // Launch SecondActivity when the users taps the notification:
                builder.SetContentIntent(pendingIntent);

                // 產生一個 notification 物件
                Notification notification = builder.Build();

                #region 決定是否要發出聲音
                if (x.Sound)
                {
                    notification.Defaults |= NotificationDefaults.Sound;
                }
                #endregion

                #region 決定是否要有震動
                if (x.Vibrate)
                {
                    notification.Defaults |= NotificationDefaults.Vibrate;
                }
                #endregion


                // 顯示本地通知:
                notificationManager.Notify(notificationId++, notification);

                // 解開底下程式碼註解，將會於五秒鐘之後，才會發生本地通知
                // Thread.Sleep(5000);
                // builder.SetContentTitle("Updated Notification");
                // builder.SetContentText("Changed to this message after five seconds.");
                // notification = builder.Build();
                // notificationManager.Notify(notificationId, notification);
                #endregion
                #endregion
            });
            #endregion
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IUnityContainer container)
        {

        }
    }
}


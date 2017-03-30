using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFLocalNotificationDroid
{
    public class LocalNotificationEvent : PubSubEvent<LocalNotificationPayload>
    {
    }

    public class LocalNotificationPayload
    {
        /// <summary>
        /// 要導航到的頁面名稱
        /// </summary>
        public string NavigationPage { get; set; }
        /// <summary>
        /// 其他相關資訊
        /// </summary>
        public string OtherInformation { get; set; }
        /// <summary>
        /// 通知訊息主旨
        /// </summary>
        public string ContentTitle { get; set; }
        /// <summary>
        /// 通知訊息內容
        /// </summary>
        public string ContentText { get; set; }
        public string SummaryText { get; set; }
        public List<string> InboxStyleList { get; set; } = new List<string>();

        // 底下欄位的意思，請參考 https://developer.xamarin.com/api/type/Android.App.Notification+Builder/

        public LocalNotificationStyleEnum Style { get; set; }
        public LocalNotificationVisibilityEnum Visibility { get; set; }
        public LocalNotificationPriorityEnum Priority { get; set; }
        public LocalNotificationCategoryEnum Category { get; set; }
        public bool LargeIcon { get; set; }
        public bool Sound { get; set; }
        public bool Vibrate { get; set; }
    }

    public enum LocalNotificationStyleEnum
    {
        Normal,
        BigText,
        Inbox,
        Image,
    }
    public enum LocalNotificationVisibilityEnum
    {
        Public,
        Private,
        Secret
    }
    public enum LocalNotificationPriorityEnum
    {
        Default,
        High,
        Low,
        Maximum,
        Minimum,
    }
    public enum LocalNotificationCategoryEnum
    {
        Call,
        Message,
        Alarm,
        Email,
        Event,
        Promo,
        Progress,
        Social,
        Error,
        Transport,
        System,
        Service,
        Recommendation,
        Status,                            
    }
}

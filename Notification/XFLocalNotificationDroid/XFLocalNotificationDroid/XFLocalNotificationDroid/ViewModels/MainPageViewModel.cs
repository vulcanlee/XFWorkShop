using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace XFLocalNotificationDroid.ViewModels
{

    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)

        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)


        #region NaviPageSource
        private ObservableCollection<string> _NaviPageSource = new ObservableCollection<string>();
        /// <summary>
        /// NaviPageSource
        /// </summary>
        public ObservableCollection<string> NaviPageSource
        {
            get { return _NaviPageSource; }
            set { SetProperty(ref _NaviPageSource, value); }
        }
        #endregion

        #region NaviPageSelectedItem
        private string _NaviPageSelectedItem;
        /// <summary>
        /// NaviPageSelectedItem
        /// </summary>
        public string NaviPageSelectedItem
        {
            get { return this._NaviPageSelectedItem; }
            set { this.SetProperty(ref this._NaviPageSelectedItem, value); }
        }
        #endregion

        #region Title
        private string _Title;
        /// <summary>
        /// 主題
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #region Message
        private string _Message;
        /// <summary>
        /// 訊息內文
        /// </summary>
        public string Message
        {
            get { return this._Message; }
            set { this.SetProperty(ref this._Message, value); }
        }
        #endregion
        #region StyleSource
        private ObservableCollection<string> _StyleSource = new ObservableCollection<string>();
        /// <summary>
        /// StyleSource
        /// </summary>
        public ObservableCollection<string> StyleSource
        {
            get { return _StyleSource; }
            set { SetProperty(ref _StyleSource, value); }
        }
        #endregion

        #region StyleSelectedItem
        private string _StyleSelectedItem;
        /// <summary>
        /// StyleSelectedItem
        /// </summary>
        public string StyleSelectedItem
        {
            get { return this._StyleSelectedItem; }
            set
            {
                this.SetProperty(ref this._StyleSelectedItem, value);
                ResetContentText();
            }
        }
        #endregion
        #region VisibilitySource
        private ObservableCollection<string> _VisibilitySource = new ObservableCollection<string>();
        /// <summary>
        /// VisibilitySource
        /// </summary>
        public ObservableCollection<string> VisibilitySource
        {
            get { return _VisibilitySource; }
            set { SetProperty(ref _VisibilitySource, value); }
        }
        #endregion

        #region VisibilitySelectedItem
        private string _VisibilitySelectedItem;
        /// <summary>
        /// VisibilitySelectedItem
        /// </summary>
        public string VisibilitySelectedItem
        {
            get { return this._VisibilitySelectedItem; }
            set
            {
                this.SetProperty(ref this._VisibilitySelectedItem, value);
                ResetContentText();
            }
        }
        #endregion
        #region PrioritySource
        private ObservableCollection<string> _PrioritySource = new ObservableCollection<string>();
        /// <summary>
        /// PrioritySource
        /// </summary>
        public ObservableCollection<string> PrioritySource
        {
            get { return _PrioritySource; }
            set { SetProperty(ref _PrioritySource, value); }
        }
        #endregion

        #region PrioritySelectedItem
        private string _PrioritySelectedItem;
        /// <summary>
        /// PrioritySelectedItem
        /// </summary>
        public string PrioritySelectedItem
        {
            get { return this._PrioritySelectedItem; }
            set
            {
                this.SetProperty(ref this._PrioritySelectedItem, value);
                ResetContentText();
            }
        }
        #endregion
        #region CategorySource
        private ObservableCollection<string> _CategorySource = new ObservableCollection<string>();
        /// <summary>
        /// CategorySource
        /// </summary>
        public ObservableCollection<string> CategorySource
        {
            get { return _CategorySource; }
            set { SetProperty(ref _CategorySource, value); }
        }
        #endregion

        #region CategorySelectedItem
        private string _CategorySelectedItem;
        /// <summary>
        /// CategorySelectedItem
        /// </summary>
        public string CategorySelectedItem
        {
            get { return this._CategorySelectedItem; }
            set { this.SetProperty(ref this._CategorySelectedItem, value); }
        }
        #endregion

        #region LargeIcon
        private bool _LargeIcon;
        /// <summary>
        /// LargeIcon
        /// </summary>
        public bool LargeIcon
        {
            get { return this._LargeIcon; }
            set { this.SetProperty(ref this._LargeIcon, value); }
        }
        #endregion

        #region Sound
        private bool _Sound;
        /// <summary>
        /// Sound
        /// </summary>
        public bool Sound
        {
            get { return this._Sound; }
            set { this.SetProperty(ref this._Sound, value); }
        }
        #endregion

        #region Vibrate
        private bool _Vibrate;
        /// <summary>
        /// Vibrate
        /// </summary>
        public bool Vibrate
        {
            get { return this._Vibrate; }
            set { this.SetProperty(ref this._Vibrate, value); }
        }
        #endregion

        #region SummaryText
        private string _SummaryText;
        /// <summary>
        /// SummaryText
        /// </summary>
        public string SummaryText
        {
            get { return this._SummaryText; }
            set { this.SetProperty(ref this._SummaryText, value); }
        }
        #endregion

        #endregion

        #region Field 欄位
        public List<string> InboxStyleList { get; set; } = new List<string>();

        #region ViewModel 內使用到的欄位
        #endregion

        #region 命令物件欄位

        public DelegateCommand LaunchNotificationCommand { get; set; }
        #endregion

        #region 注入物件欄位

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        #endregion

        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {

            #region 相依性服務注入的物件

            _eventAggregator = eventAggregator;
            _navigationService = navigationService;
            #endregion

            #region 頁面中綁定的命令
            LaunchNotificationCommand = new DelegateCommand(() =>
            {
                var fooLocalNotificationPayload = new LocalNotificationPayload
                {
                    ContentTitle = Title,
                    ContentText = Message,
                    NavigationPage = NaviPageSelectedItem,
                    OtherInformation = "這是額外要傳遞的資訊",
                    SummaryText = SummaryText,
                    Style = (LocalNotificationStyleEnum)Enum.Parse(typeof(LocalNotificationStyleEnum), StyleSelectedItem),
                    Visibility = (LocalNotificationVisibilityEnum)Enum.Parse(typeof(LocalNotificationVisibilityEnum), VisibilitySelectedItem),
                    Priority = (LocalNotificationPriorityEnum)Enum.Parse(typeof(LocalNotificationPriorityEnum), PrioritySelectedItem),
                    Category = (LocalNotificationCategoryEnum)Enum.Parse(typeof(LocalNotificationCategoryEnum), CategorySelectedItem),
                    LargeIcon = LargeIcon,
                    Sound = Sound,
                    Vibrate = Vibrate,
                };
                foreach (var item in InboxStyleList)
                {
                    fooLocalNotificationPayload.InboxStyleList.Add(item);
                }
                _eventAggregator.GetEvent<LocalNotificationEvent>().Publish(fooLocalNotificationPayload);
            });
            #endregion

            #region 事件聚合器訂閱

            #endregion
        }

        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
        }

        public async void OnNavigatedTo(NavigationParameters parameters)
        {
            if (parameters.ContainsKey("LocalNotification"))
            {
                #region 需要繼續顯示到明細頁面
                LocalNotificationPayload fooLocalNotificationPayload = parameters["LocalNotification"] as LocalNotificationPayload;
                var fooPara = new NavigationParameters();
                fooPara.Add("ShowIt", fooLocalNotificationPayload);
                await _navigationService.NavigateAsync("DetailPage", fooPara);
                #endregion
            }
            await ViewModelInit();
        }
        #endregion

        #region 設計時期或者執行時期的ViewModel初始化
        #endregion

        #region 相關事件
        #endregion

        #region 相關的Command定義
        #endregion

        #region 其他方法

        /// <summary>
        /// ViewModel 資料初始化
        /// </summary>
        /// <returns></returns>
        private async Task ViewModelInit()
        {
            Title = "你有一則新留言";

            NaviPageSource.Clear();
            NaviPageSource.Add("首頁");
            NaviPageSource.Add("明細頁面");
            NaviPageSelectedItem = NaviPageSource[1];

            StyleSource.Clear();
            StyleSource.Add(LocalNotificationStyleEnum.BigText.ToString());
            StyleSource.Add(LocalNotificationStyleEnum.Image.ToString());
            StyleSource.Add(LocalNotificationStyleEnum.Inbox.ToString());
            StyleSource.Add(LocalNotificationStyleEnum.Normal.ToString());
            StyleSelectedItem = StyleSource[0];

            VisibilitySource.Clear();
            VisibilitySource.Add(LocalNotificationVisibilityEnum.Private.ToString());
            VisibilitySource.Add(LocalNotificationVisibilityEnum.Public.ToString());
            VisibilitySource.Add(LocalNotificationVisibilityEnum.Secret.ToString());
            VisibilitySelectedItem = VisibilitySource[0];

            PrioritySource.Clear();
            PrioritySource.Add(LocalNotificationPriorityEnum.Default.ToString());
            PrioritySource.Add(LocalNotificationPriorityEnum.High.ToString());
            PrioritySource.Add(LocalNotificationPriorityEnum.Low.ToString());
            PrioritySource.Add(LocalNotificationPriorityEnum.Maximum.ToString());
            PrioritySource.Add(LocalNotificationPriorityEnum.Minimum.ToString());
            PrioritySelectedItem = PrioritySource[0];

            CategorySource.Clear();
            CategorySource.Add(LocalNotificationCategoryEnum.Alarm.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Call.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Email.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Error.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Event.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Message.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Progress.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Promo.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Recommendation.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Service.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Social.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Status.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.System.ToString());
            CategorySource.Add(LocalNotificationCategoryEnum.Transport.ToString());
            CategorySelectedItem = CategorySource[0];

            ResetContentText();
            await Task.Delay(100);
        }

        void ResetContentText()
        {
            if (StyleSelectedItem == LocalNotificationStyleEnum.BigText.ToString())
            {
                Title = "這是Big Text的通知內容";
                Message = "在此處通知訊息文本。您可以在啟動通知，修改此文字方塊中的消息之前，或您可以只使用此預設文本。";
                InboxStyleList.Clear();
                SummaryText = "此處顯示摘要文本";
            }
            else if (StyleSelectedItem == LocalNotificationStyleEnum.Inbox.ToString())
            {
                Title = "5 個新消息";
                Message = "vulcan@miniasp.com";
                Message = "此通知包含示例電子郵件摘要。發射後的通知，您可以 dragdown 上要查看摘要的通知。";
                InboxStyleList.Clear();
                InboxStyleList.Add("香蕉出售");
                InboxStyleList.Add("好奇你的部落格文章");
                InboxStyleList.Add("需要騎才能進化嗎？");
                SummaryText = "+ 2 更多";
            }
            else if (StyleSelectedItem == LocalNotificationStyleEnum.Image.ToString())
            {
                Title = "圖片通知";
                Message = "";
                InboxStyleList.Clear();
                SummaryText = "此處顯示摘要文本";
            }
            else if (StyleSelectedItem == LocalNotificationStyleEnum.Normal.ToString())
            {
                Title = "這是一般的通知內容";
                Message = "此處顯示通知訊息文本";
                InboxStyleList.Clear();
                SummaryText = "";
            }
        }
        #endregion

    }
}

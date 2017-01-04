using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XFMqtt.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        #region SendingMessage
        private string _SendingMessage = "";
        /// <summary>
        /// SendingMessage
        /// </summary>
        public string SendingMessage
        {
            get { return this._SendingMessage; }
            set { this.SetProperty(ref this._SendingMessage, value); }
        }
        #endregion


        #region UserID
        private string _UserID = "";
        /// <summary>
        /// UserID
        /// </summary>
        public string UserID
        {
            get { return this._UserID; }
            set { this.SetProperty(ref this._UserID, value); }
        }
        #endregion


        #region SendingUserID
        private string _SendingUserID = "";
        /// <summary>
        /// SendingUserID
        /// </summary>
        public string SendingUserID
        {
            get { return this._SendingUserID; }
            set { this.SetProperty(ref this._SendingUserID, value); }
        }
        #endregion


        public DelegateCommand SendingCommand { get; set; }

        private readonly IEventAggregator _eventAggregator;

        public MainPageViewModel(IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;

            // 原生專案 -> 核心 PCL 專案
            // 當訂閱 Prism 型別的 ReceiveMessageEvent 事件有訊息接收到之後，在此將會更新檢視模型的屬性值
            // 這個型別的 ReceiveMessageEvent 事件，將會從原生專案內送出來
            _eventAggregator.GetEvent<ReceiveMessageEvent>().Subscribe(x =>
            {
                // 判斷是否該訊息是要傳送給這個裝置
                if (x.To == UserID)
                {
                    Title = $"接收到的訊息:{x.Content}";
                }
            });

            SendingCommand = new DelegateCommand(() =>
            {
                // 核心 PCL 專案 -> 原生專案
                // 透過 Prism 事件聚合器，將希望原生專案內的方法要執行的工作，以 SendMessageEvent 型別訊息送出，
                // 請求原生專案收到這個事件之後，將這個 Payload 使用 MQTT 方式送出
                _eventAggregator.GetEvent<SendMessageEvent>().Publish(new MessageType
                {
                    Content = SendingMessage,
                    To = SendingUserID
                });
            });
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Title = "顯示收到的訊息";
        }
    }
}

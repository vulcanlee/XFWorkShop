using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using XFQRCode.Models;
using ZXing.Net.Mobile.Forms;

namespace XFQRCode.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region Title
        private string _Title;
        /// <summary>
        /// PropertyDescription
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #region 掃描結果
        private string _掃描結果;
        /// <summary>
        /// 掃描結果
        /// </summary>
        public string 掃描結果
        {
            get { return this._掃描結果; }
            set { this.SetProperty(ref this._掃描結果, value); }
        }
        #endregion

        #region 要產生的QRCode文字
        private string _要產生的QRCode文字= "https://mylabtw.blogspot.tw/";
        /// <summary>
        /// 要產生的QRCode文字
        /// </summary>
        public string 要產生的QRCode文字
        {
            get { return this._要產生的QRCode文字; }
            set { this.SetProperty(ref this._要產生的QRCode文字, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        public DelegateCommand 開始掃描Command { get; set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            // 訂閱條碼掃描結果的事件
            _eventAggregator.GetEvent<ScanResultEvent>().Subscribe(x =>
            {
                掃描結果 = x;
            });

            開始掃描Command = new DelegateCommand(async () =>
            {
                // 切換到條碼掃描頁面
                await _navigationService.NavigateAsync("CodeScannerPage");
            });
        }
        #endregion

        #region Navigation Events (頁面導航事件)
        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
                Title = "多奇數位創意有限公司";
        }
        #endregion

        #region 設計時期或者執行時期的ViewModel初始化
        #endregion

        #region 相關事件
        #endregion

        #region 相關的Command定義
        #endregion

        #region 其他方法
        #endregion
    }
}

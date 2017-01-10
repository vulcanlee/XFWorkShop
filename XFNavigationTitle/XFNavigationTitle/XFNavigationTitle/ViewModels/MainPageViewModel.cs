using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XFNavigationTitle.ViewModels
{
    public class MainPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)
        #region PageTitle
        private string _PageTitle = "多奇數位創意有限公司首頁";
        /// <summary>
        /// PageTitle
        /// </summary>
        public string PageTitle
        {
            get { return this._PageTitle; }
            set { this.SetProperty(ref this._PageTitle, value); }
        }
        #endregion


        #region Title
        private string _Title= "多奇數位創意有限公司";
        /// <summary>
        /// Title
        /// </summary>
        public string Title
        {
            get { return this._Title; }
            set { this.SetProperty(ref this._Title, value); }
        }
        #endregion

        #endregion

        #region Field 欄位

        public DelegateCommand 測試回上一頁文字Command { get; set; }

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        public readonly IPageDialogService _dialogService;
        #endregion

        #region Constructor 建構式
        public MainPageViewModel(INavigationService navigationService, IPageDialogService dialogService, IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;
            _dialogService = dialogService;
            _navigationService = navigationService;
            
            測試回上一頁文字Command = new DelegateCommand(async () =>
            {
                await _navigationService.NavigateAsync("Next1Page");
            });
        }
        #endregion

        #region Navigation Events (頁面導航事件)

        public void OnNavigatedFrom(NavigationParameters parameters)
        {

        }

        public void OnNavigatedTo(NavigationParameters parameters)
        {
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

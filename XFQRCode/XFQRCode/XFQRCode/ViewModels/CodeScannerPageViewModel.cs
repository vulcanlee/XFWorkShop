using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Events;

namespace XFQRCode.ViewModels
{
    public class CodeScannerPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)
        #endregion

        #region Field 欄位
        // 這裡宣告為 Public ，目的是為了能夠讓 Code Behind 可以存取這兩個注入的物件
        public readonly IEventAggregator _eventAggregator;
        public readonly INavigationService _navigationService;
        #endregion

        #region Constructor 建構式
        public CodeScannerPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {
            _navigationService = navigationService;
            _eventAggregator = eventAggregator;

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

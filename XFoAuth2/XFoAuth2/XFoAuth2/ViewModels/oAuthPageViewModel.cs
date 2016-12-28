using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Prism.Navigation;
using Prism.Events;
using XFoAuth2.Models;

namespace XFoAuth2.ViewModels
{
    /// <summary>
    /// 用來顯示 OAuth登入頁面的網頁內容，每個平台會有這個頁面的客製化 Renderer
    /// </summary>
    public class oAuthPageViewModel : BindableBase, INavigationAware
    {
        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)
        #endregion

        #region Field 欄位
        SubscriptionToken fooSubscriptionToken;

        private readonly IEventAggregator _eventAggregator;
        private readonly INavigationService _navigationService;
        #endregion

        #region Constructor 建構式
        public oAuthPageViewModel(INavigationService navigationService, IEventAggregator eventAggregator)
        {

            _eventAggregator = eventAggregator;
            _navigationService = navigationService;

            // 訂閱使用者認證結果通知事件，認證成功之後，會收到 Success
            fooSubscriptionToken = _eventAggregator.GetEvent<AuthEvent>().Subscribe(async x =>
              {
                  await _navigationService.GoBackAsync();
                  if (x == "Success")
                  {
                      // 送出通知事件，告知 登入頁面，需要自動切換到下一個頁面，也就是首頁
                      _eventAggregator.GetEvent<LoginEvent>().Publish("Refresh");
                  }
                  else
                  {

                  }
                  // 取消訂閱使用者認證結果通知事件，避免訂閱事件會反覆執行
                  _eventAggregator.GetEvent<AuthEvent>().Unsubscribe(fooSubscriptionToken);
  
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

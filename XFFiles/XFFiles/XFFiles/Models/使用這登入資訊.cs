using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFFiles.Models
{
    public class 使用這登入資訊 : BindableBase
    {

        #region Repositories (遠端或本地資料存取)
        #endregion

        #region ViewModel Property (用於在 View 中作為綁定之用)

        #region 姓名
        private string _姓名;
        /// <summary>
        /// 姓名
        /// </summary>
        public string 姓名
        {
            get { return this._姓名; }
            set { this.SetProperty(ref this._姓名, value); }
        }
        #endregion

        #region 帳號
        private string _帳號;
        /// <summary>
        /// 帳號
        /// </summary>
        public string 帳號
        {
            get { return this._帳號; }
            set { this.SetProperty(ref this._帳號, value); }
        }
        #endregion

        #region 密碼
        private string _密碼;
        /// <summary>
        /// 密碼
        /// </summary>
        public string 密碼
        {
            get { return this._密碼; }
            set { this.SetProperty(ref this._密碼, value); }
        }
        #endregion

        #region 記憶密碼
        private bool _記憶密碼;
        /// <summary>
        /// 記憶密碼
        /// </summary>
        public bool 記憶密碼
        {
            get { return this._記憶密碼; }
            set { this.SetProperty(ref this._記憶密碼, value); }
        }
        #endregion

        #endregion

        #region Field 欄位
        #endregion

        #region Constructor 建構式
        #endregion

        #region Navigation Events (頁面導航事件)
        #endregion

        #region 設計時期或者執行時期的ViewModel初始化
        #endregion

        #region 相關事件
        #endregion

        #region 相關的Command定義
        #endregion

        #region 其他方法
        public 使用這登入資訊 ShallowCopy()
        {
            return (使用這登入資訊)this.MemberwiseClone();
        }
        #endregion

    }
}

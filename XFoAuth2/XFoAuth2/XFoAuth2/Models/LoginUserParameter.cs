using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFoAuth2.Models
{
    public enum LoginUserEnum
    {
        AccountPassword,
        Facebook,
        Google
    }

    /// <summary>
    /// 用於在登入頁面，當使用者認證通通過之後，要將該使用者明細資訊，傳遞到首頁，用到的類別定義
    /// </summary>
    public class LoginUserParameter
    {
        public string 名稱 { get; set; }
        public string 電子郵件 { get; set; }
        public string 大頭貼URL { get; set; }
        public LoginUserEnum 身分驗證方式 { get; set; }

    }
}

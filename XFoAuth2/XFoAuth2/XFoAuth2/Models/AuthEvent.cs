using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace XFoAuth2.Models
{
    /// <summary>
    /// Prism 用到的使用者驗證結果 通知事件定義
    /// </summary>
    public class AuthEvent : PubSubEvent<AuthEventEnum>
    {
    }

    public enum AuthEventEnum
    {
        身分驗證成功,
        身分驗證失敗
    }
}

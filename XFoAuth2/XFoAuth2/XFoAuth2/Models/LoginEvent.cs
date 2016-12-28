using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace XFoAuth2.Models
{
    /// <summary>
    /// Prism 會用到的 使用者需要更新資訊 通知事件定義
    /// </summary>
    public class LoginEvent : PubSubEvent<string>
    {
    }
}

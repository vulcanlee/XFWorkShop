using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace XFQRCode.Models
{
    /// <summary>
    /// 條碼掃描結果事件類別
    /// </summary>
    public class ScanResultEvent : PubSubEvent<string>
    {
    }
}

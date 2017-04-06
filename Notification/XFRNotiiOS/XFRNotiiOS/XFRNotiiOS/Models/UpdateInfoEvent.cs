using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFRNotiiOS.Models
{
    /// <summary>
    /// 更新推播事件的觸發情況
    /// </summary>
    public class UpdateInfoEvent : PubSubEvent<UpdateInfoEventPayload>
    {
    }


    public class UpdateInfoEventPayload
    {
        public string Name { get; set; }
        public DateTime time { get; set; }
    }
}

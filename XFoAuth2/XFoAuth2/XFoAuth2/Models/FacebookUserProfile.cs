using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFoAuth2.Models
{
    /// <summary>
    /// 呼叫 Facebook Graph API，取得使用者明細資訊的物件定義
    /// </summary>
    public class FacebookUserProfile
    {
        public string email { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string gender { get; set; }
        public Picture picture { get; set; }
        public string id { get; set; }
    }

    public class Picture
    {
        public Data data { get; set; }
    }

    public class Data
    {
        public bool is_silhouette { get; set; }
        public string url { get; set; }
    }

}

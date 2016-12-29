using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;

namespace XFoAuth2.Models
{
    /// <summary>
    /// 要將 OAuth2 的使用者資訊，進行 JSON 序列化/反序列化，並且儲存到本機時候，會用到的類別定義(針對 UWP 平台會使用到)
    /// </summary>
    public class AccessTokenResponse
    {
        public string id { get; set; } = "";
        public string access_token { get; set; } = "";
        public string expires_in { get; set; } = "";
        public string token_type { get; set; } = "";
        public string id_token { get; set; } = "";
        public string Name { get; set; } = "";
        public string email { get; set; } = "";
        public string photo { get; set; } = "";
        public string LoginType { get; set; } = "";
    }
}

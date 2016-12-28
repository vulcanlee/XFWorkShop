using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XFoAuth2.CustomRenderer
{
    /// <summary>
    /// 客製化控制項，用來顯示 Font Awesome 的字體圖片
    /// </summary>
    public class FontAwesomeLabel : Label
    {
        public FontAwesomeLabel()
        {
            // 在這裡設定 Font Awesome 字體檔案所在的路徑
            // Font Awesome 字體檔案需要複製到各個原生專案的特定路徑內
            FontFamily = Device.OnPlatform("fontawesome", "fontawesome", "/Assets/fontawesome.ttf#FontAwesome");
        }
    }
}

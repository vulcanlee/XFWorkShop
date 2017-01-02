using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFFiles.Services
{
    /// <summary>
    /// 取得當時作業系統的類型與讀取原生專案內使用了建置動作為非`內嵌資源`的文字檔案內容
    /// </summary>
    public interface INatvieAssyFile
    {
        /// <summary>
        /// 取得作業系統版本名稱
        /// </summary>
        /// <returns></returns>
        string GetPlatformName();
        /// <summary>
        /// 從原生專案中讀取檔案內容
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        string GetFileContent(string filename);
    }
}

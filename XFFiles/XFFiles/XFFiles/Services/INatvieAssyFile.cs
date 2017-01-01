using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFFiles.Services
{
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

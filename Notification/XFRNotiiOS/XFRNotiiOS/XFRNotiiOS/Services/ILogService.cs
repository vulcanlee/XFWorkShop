using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XFRNotiiOS.Services
{
    public interface ILogService
    {
        string Read();
        void Write(string Content);
        
    }
}

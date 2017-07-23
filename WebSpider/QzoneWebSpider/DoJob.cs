using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace QzoneWebSpider
{
    public class DoJob
    {
        public static void GetAllData(string httptext)
        {
            List<string> list = new List<string>();
            list=httptext.ToRegexStringArray("(?<=link=\").*(?=\">)").ToList();
        }
    }
}

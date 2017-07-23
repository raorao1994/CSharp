using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Route("/UpFile")]
    public class UpFileModel
    {
        public string url { get; set; }
        public string name { get; set; }
    }
}

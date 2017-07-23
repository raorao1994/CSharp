using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using ServiceStack;
using System.Drawing;

namespace Service
{
    public class UpFileService:ServiceStack.Service
    {
        public string POST(UpFileModel request)
        {
            if (request.url != null)
            {
                //GetBytesFromUrl是来自ServiceStack的拓展方法
                using (var ms = new MemoryStream(request.url.GetBytesFromUrl()))
                {
                    WriteFile(ms);
                }
            }
            var re = Request.FormData["name"];//通过Request来获取表单数据
            foreach (var uploadedFile in Request.Files.Where(uploadedFile => uploadedFile.ContentLength > 0))
            {
                var name=uploadedFile.FileName;
                using (var ms = new MemoryStream())
                {
                    uploadedFile.WriteTo(ms);
                    WriteFile(ms);
                }
            }
            return "";
        }

        private void WriteFile(MemoryStream ms)
        {
            ms.Position = 0;
            var fileName = "1.jpg";
            using (var img = Image.FromStream(ms))
            {
                string path = "~/upfile".MapAbsolutePath();
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                img.Save(path+"/"+fileName);
            }
        }
    }
}

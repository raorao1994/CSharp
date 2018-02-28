using PublicLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace WeiXin.Model
{
    /// <summary>
    /// 回复信息类
    /// </summary>
    public class ResponseMsg
    {
        public virtual string ResponseNull()
        {
            return "";
        }
        /// <summary>
        /// 回复消息(文字)
        /// </summary>
        public virtual string ResText(TextMsg model)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", model.FromUserName, model.ToUserName, model.CreateTime));
            resxml.AppendFormat("<MsgType><![CDATA[text]]></MsgType><Content><![CDATA[{0}]]></Content><FuncFlag>0</FuncFlag></xml>", model.Content);
            return Response(resxml.ToString());
        }
        /// <summary>
        /// 回复消息(音乐)
        /// </summary>
        public string ResMusic(VoiceMsg model)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", model.FromUserName, model.ToUserName, model.CreateTime));
            resxml.Append(" <MsgType><![CDATA[music]]></MsgType>");
            resxml.AppendFormat("<Music><Title><![CDATA[{0}]]></Title><Description><![CDATA[{1}]]></Description>", mu.Title, mu.Description);
            resxml.AppendFormat("<MusicUrl><![CDATA[{0}]]></MusicUrl><HQMusicUrl><![CDATA[http://{2}{3}]]></HQMusicUrl></Music><FuncFlag>0</FuncFlag></xml>", model.MediaId);
            return Response(resxml.ToString());
        }
        public string ResVideo(VoiceMsg model)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", model.FromUserName, model.ToUserName, model.CreateTime));
            resxml.Append(" <MsgType><![CDATA[video]]></MsgType>");
            resxml.AppendFormat("<Video><MediaId><![CDATA[{0}]]></MediaId>", model.media_id);
            resxml.AppendFormat("<Title><![CDATA[{0}]]></Title>", model.title);
            resxml.AppendFormat("<Description><![CDATA[{0}]]></Description></Video></xml>", model.description);
            return Response(param, resxml.ToString());
        }

        /// <summary>
        /// 回复消息(图片)
        /// </summary>
        public string ResPicture(PictureMsg model)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", model.FromUserName, model.ToUserName, model.CreateTime));
            resxml.Append(" <MsgType><![CDATA[image]]></MsgType>");
            resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl></xml>", model.PicUrl);
            return Response(resxml.ToString());
        }

        /// <summary>
        /// 回复消息（图文列表）
        /// </summary>
        /// <param name="param"></param>
        /// <param name="art"></param>
        public string ResArticles(PictureMsg model, List<Article> art)
        {
            StringBuilder resxml = new StringBuilder(string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime>", model.FromUserName, model.ToUserName, model.CreateTime));
            resxml.AppendFormat("<MsgType><![CDATA[news]]></MsgType><ArticleCount>{0}</ArticleCount><Articles>", art.Count);
            for (int i = 0; i < art.Count; i++)
            {
                resxml.AppendFormat("<item><Title><![CDATA[{0}]]></Title>  <Description><![CDATA[{1}]]></Description>", art[i].Title, art[i].Description);
                resxml.AppendFormat("<PicUrl><![CDATA[{0}]]></PicUrl><Url><![CDATA[{1}]]></Url></item>", art[i].PicUrl.Contains("http://") ? art[i].PicUrl : "http://" + VqiRequest.GetCurrentFullHost() + art[i].PicUrl, art[i].Url.Contains("http://") ? art[i].Url : "http://" + VqiRequest.GetCurrentFullHost() + art[i].Url);
            }
            resxml.Append("</Articles><FuncFlag>0</FuncFlag></xml>");
            return Response(resxml.ToString());
        }
        /// <summary>
        /// 多客服转发
        /// </summary>
        /// <param name="param"></param>
        public string ResDKF(BaseMsg model)
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", model.FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", model.ToUserName, model.CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType></xml>");
            return Response(resxml.ToString());
        }
        /// <summary>
        /// 多客服转发如果指定的客服没有接入能力(不在线、没有开启自动接入或者自动接入已满)，该用户会一直等待指定客服有接入能力后才会被接入，而不会被其他客服接待。建议在指定客服时，先查询客服的接入能力指定到有能力接入的客服，保证客户能够及时得到服务。
        /// </summary>
        /// <param name="param">用户发送的消息体</param>
        /// <param name="KfAccount">多客服账号</param>
        public string ResDKF(BaseMsg model, string KfAccount)
        {
            StringBuilder resxml = new StringBuilder();
            resxml.AppendFormat("<xml><ToUserName><![CDATA[{0}]]></ToUserName>", model.FromUserName);
            resxml.AppendFormat("<FromUserName><![CDATA[{0}]]></FromUserName><CreateTime>{1}</CreateTime>", model.ToUserName, model.CreateTime);
            resxml.AppendFormat("<MsgType><![CDATA[transfer_customer_service]]></MsgType><TransInfo><KfAccount>{0}</KfAccount></TransInfo></xml>", KfAccount);
            return Response(resxml.ToString());
        }
        private string Response(EnterParam param, string data)
        {
            if (param.IsAes)
            {
                Convert.ToInt64(DateTime.Now).ToString();
                var wxcpt = new MsgCrypt(param.token, param.EncodingAESKey, param.appid);
                wxcpt.EncryptMsg(data, Utility.GetTimeStamp(), Utils.GetRamCode(), ref data);
            }
            return data;
        }
    }
}
}
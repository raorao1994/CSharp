using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PublicAccount
{
    /// <summary>
    /// 菜单辅助类
    /// </summary>
    public static class MenuHelper
    {
        /// <summary>
        /// 一级菜单的最大数目
        /// </summary>
        private const int maxMenuCount = 3;
        /// <summary>
        /// 创建菜单的地址
        /// </summary>
        private const string urlForCreating = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";
        /// <summary>
        /// 创建菜单的http方法
        /// </summary>
        private const string httpMethodForCreating = WebRequestMethods.Http.Post;
        /// <summary>
        /// 查询菜单的地址
        /// </summary>
        private const string urlForGetting = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";
        /// <summary>
        /// 查询菜单的http方法
        /// </summary>
        private const string httpMethodForGetting = WebRequestMethods.Http.Get;
        /// <summary>
        /// 删除菜单的地址
        /// </summary>
        private const string urlForDeleting = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";
        /// <summary>
        /// 删除菜单的http方法 
        /// </summary>
        private const string httpMethodForDeleting = WebRequestMethods.Http.Get;
        /// <summary>
        /// 获取自定义菜单配置的地址
        /// </summary>
        private const string urlForGettingSelfMenuInfo = "https://api.weixin.qq.com/cgi-bin/get_current_selfmenu_info?access_token={0}";
        /// <summary>
        /// 获取自定义菜单配置的http方法
        /// </summary>
        private const string httpMethodForGettingSelfMenuInfo = WebRequestMethods.Http.Get;

        /// <summary>
        /// 创建菜单容器
        /// </summary>
        /// <param name="name">名称</param>
        /// <returns>返回菜单容器</returns>
        public static MenuContainer CreateContainer(string name)
        {
            return new MenuContainer(name);
        }

        /// <summary>
        /// 创建包含子菜单的菜单容器
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="submenus">子菜单</param>
        /// <returns>返回菜单容器</returns>
        public static MenuContainer CreateContainer(string name, IEnumerable<BaseMenu> submenus)
        {
            return new MenuContainer(name, submenus);
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="type">菜单类型</param>
        /// <param name="name">名称</param>
        /// <param name="keyOrUrl">键或者跳转地址</param>
        /// <returns>返回菜单</returns>
        public static BaseMenu CreateItem(MenuTypeEnum type, string name, string keyOrUrl)
        {
            if (!(type == MenuTypeEnum.view || type == MenuTypeEnum.click || type == MenuTypeEnum.scancode_push ||
                type == MenuTypeEnum.scancode_waitmsg || type == MenuTypeEnum.pic_sysphoto || type == MenuTypeEnum.pic_photo_or_album ||
                type == MenuTypeEnum.pic_weixin || type == MenuTypeEnum.location_select))
                throw new ArgumentException("无效的菜单类型。", "type");
            else
                return CreateItemByProgram(type, name, keyOrUrl);
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="type">菜单类型</param>
        /// <param name="name">名称</param>
        /// <param name="info">菜单信息，可能为：value, key 或者 url</param>
        /// <returns>返回菜单</returns>
        private static BaseMenu CreateItemByProgram(MenuTypeEnum type, string name, string info)
        {
            if (type == MenuTypeEnum.view)
                return new ViewMenu(name, info);
            else if (type == MenuTypeEnum.text || type == MenuTypeEnum.img || type == MenuTypeEnum.video || type == MenuTypeEnum.voice)
                return new ValueMenu(type, name, info);
            else if (type == MenuTypeEnum.click || type == MenuTypeEnum.scancode_push || type == MenuTypeEnum.scancode_waitmsg ||
                type == MenuTypeEnum.pic_sysphoto || type == MenuTypeEnum.pic_photo_or_album || type == MenuTypeEnum.pic_weixin ||
                type == MenuTypeEnum.location_select)
                return new KeyMenu(type, name, info);
            else
                throw new ArgumentException("无效的菜单类型。", "type");
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="menus">一级菜单（注：如果一级菜单数目大于最大数目，后面的项将被忽略。）</param>
        /// <returns>返回创建是否成功</returns>
        public static ErrorMessage Create(string userName, IEnumerable<BaseMenu> menus)
        {
            if (menus == null)
                return new ErrorMessage(ErrorMessage.ExceptionCode, "菜单不能为空。");
            string json = GetMenuJsonString(menus);
            return HttpHelper.RequestErrorMessage(urlForCreating, userName, null, httpMethodForCreating, json);
        }

        /// <summary>
        /// 获取菜单的json字符串
        /// </summary>
        /// <param name="menus">一级菜单</param>
        /// <returns>返回菜单的json字符串</returns>
        private static string GetMenuJsonString(IEnumerable<BaseMenu> menus)
        {
            List<object> buttons = new List<object>();
            int idx = 0;
            foreach (BaseMenu menu in menus)
            {
                buttons.Add(menu.ToAnonymousObject());
                idx++;
                if (idx > maxMenuCount)
                    break;
            }
            return JsonConvert.SerializeObject(new { button = buttons });
        }

        /// <summary>
        /// 查询菜单
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="errorMessage">返回查询是否成功</param>
        /// <returns>返回菜单数组；如果获取失败，返回null。</returns>
        public static BaseMenu[] Get(string userName, out ErrorMessage errorMessage)
        {
            BaseMenu[] menus = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGetting, userName, null, httpMethodForGetting, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "查询菜单成功");
                menus = Parse(responseContent);
            }
            return menus;
        }

        /// <summary>
        /// 解析查询到的菜单数组
        /// </summary>
        /// <param name="json">查询到的json字符串</param>
        /// <returns>返回菜单数组</returns>
        private static BaseMenu[] Parse(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return null;
            JObject jo = JObject.Parse(json);
            JToken jt;
            if (!jo.TryGetValue("menu", out jt) && !jo.TryGetValue("selfmenu_info", out jt))
                return null;
            if (!((JObject)jt).TryGetValue("button", out jt))
                return null;
            JArray ja = (JArray)jt;
            BaseMenu[] menus = new BaseMenu[ja.Count];
            int idx = 0;
            foreach (JObject item in ja)
            {
                menus[idx] = Parse(item);
                idx++;
            }
            return menus;
        }

        /// <summary>
        /// 解析菜单
        /// </summary>
        /// <param name="jo"></param>
        /// <returns></returns>
        private static BaseMenu Parse(JObject jo)
        {
            JToken jt;
            if (jo.TryGetValue("type", out jt) && jo.TryGetValue("name", out jt))
            {
                MenuTypeEnum type = (MenuTypeEnum)Enum.Parse(typeof(MenuTypeEnum), (string)jo["type"]);
                string name = (string)jo["name"];
                if (jo.TryGetValue("key", out jt) || jo.TryGetValue("url", out jt) || jo.TryGetValue("value", out jt))
                {
                    string info = (string)jt;
                    return CreateItemByProgram(type, name, info);
                }
                else
                {
                    JArray ja = (JArray)jo["news_info"]["list"];
                    NewsForMenu[] news_info = new NewsForMenu[ja.Count];
                    for (int i = 0; i < ja.Count; i++)
                        news_info[i] = NewsForMenu.Parse((JObject)ja[i]);
                    return new NewsMenu(name, news_info);
                }
            }
            else
            {
                string name = (string)jo["name"];
                MenuContainer mc = CreateContainer(name);
                JArray ja = null;
                if (jo.TryGetValue("sub_button", out jt))
                {
                    if (jt.Type == JTokenType.Array)
                        ja = (JArray)jt;
                    else if (jt.Type == JTokenType.Object && ((JObject)jt).TryGetValue("list", out jt))
                    {
                        if (jt.Type == JTokenType.Array)
                            ja = (JArray)jt;
                    }
                }
                if (ja != null)
                {
                    foreach (JObject item in ja)
                        mc.Add(Parse(item));
                }
                return mc;
            }
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <returns>返回删除是否成功</returns>
        public static ErrorMessage Delete(string userName)
        {
            return HttpHelper.RequestErrorMessage(urlForDeleting, userName, null, httpMethodForDeleting, null);
        }

        /// <summary>
        /// 获取自定义菜单配置
        /// </summary>
        /// <param name="userName">公众号</param>
        /// <param name="isOpened">返回是否开启了菜单；如果获取失败，返回false。</param>
        /// <param name="errorMessage">返回获取是否成功</param>
        /// <returns>返回是菜单数组；如果获取失败，或者尚未开启菜单，返回null。</returns>
        public static BaseMenu[] GetSelfMenuInfo(string userName, out bool isOpened, out ErrorMessage errorMessage)
        {
            isOpened = false;
            BaseMenu[] menus = null;
            string responseContent = HttpHelper.RequestResponseContent(urlForGettingSelfMenuInfo, userName, null, httpMethodForGettingSelfMenuInfo, null);
            if (string.IsNullOrWhiteSpace(responseContent))
                errorMessage = new ErrorMessage(ErrorMessage.ExceptionCode, "请求失败。");
            else if (ErrorMessage.IsErrorMessage(responseContent))
                errorMessage = ErrorMessage.Parse(responseContent);
            else
            {
                errorMessage = new ErrorMessage(ErrorMessage.SuccessCode, "查询菜单成功");
                JObject jo = JObject.Parse(responseContent);
                isOpened = (int)jo["is_menu_open"] == 1;
                if (isOpened)
                    menus = Parse(responseContent);
            }
            return menus;
        }
    }
}
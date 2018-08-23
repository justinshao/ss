using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using SmartSystem.WeiXinBase;
using System.IO;
using System.Xml;
using System.Web;

namespace SmartSystem.WeiXinInteraction
{
    public abstract class Conversation
    {
        #region IConversation
        /// <summary>
        /// 是否使用了代理
        /// </summary>
        public bool UsedAgent { get; set; }

        /// <summary>
        /// 发送者用户名（OpenId）
        /// </summary>
        public string OpenId
        {
            get
            {
                return Request != null ? Request.FromUserName : null;
            }
        }

        /// <summary>
        /// 开发者微信号
        /// </summary>
        public string ServiceId
        {
            get
            {
                return Request != null ? Request.ToUserName : null;
            }
        }

        /// <summary>
        /// 取消执行Execute()方法。一般在OnExecuting()中用于临时阻止执行Execute()。
        /// 默认为False。
        /// 如果在执行OnExecuting()执行前设为True，则所有OnExecuting()、Execute()、OnExecuted()代码都不会被执行。
        /// 如果在执行OnExecuting()执行过程中设为True，则后续Execute()及OnExecuted()代码不会被执行。
        /// </summary>
        public bool CancelExcute { get; set; }

        /// <summary>
        /// 在构造函数中转换得到原始XML数据
        /// </summary>
        public XDocument RequestDocument { get; set; }

        /// <summary>
        /// 根据ResponseMessageBase获得转换后的ResponseDocument
        /// 注意：这里每次请求都会根据当前的ResponseMessageBase生成一次，如需重用此数据，建议使用缓存或局部变量
        /// </summary>
        public XDocument ResponseDocument
        {
            get
            {
                return Response == null ? null : (Response as WRespBase).ConvertToXml();
            }
        }

        /// <summary>
        /// 请求实体
        /// </summary>
        public IWReqBase Request { get; set; }

        /// <summary>
        /// 响应实体
        /// 只有当执行Execute()方法后才可能有值
        /// </summary>
        public IWRespBase Response { get; set; }
        #endregion IConversation

        protected Conversation(Stream inputStream)
        {
            using (var xr = XmlReader.Create(inputStream))
            {
                Init(XDocument.Load(xr));
            }
        }

        protected Conversation(XDocument requestDocument)
        {
            Init(requestDocument);
        }


        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="requestXml"></param>
        private void Init(XDocument requestXml)
        {
            RequestDocument = requestXml;
            Request = WRequestFactory.GetRequestEntity(RequestDocument);
        }


        /// <summary>
        /// Execute之前执行的方法
        /// </summary>
        public virtual void OnExecuting()
        {
        }

        #region Enter & Exit

        public string EnterKey = "xNet_Wx_Enter_{0}";
        /// <summary>
        /// EnterCache进入会话，默认缓存时间300秒
        /// </summary>
        public void Enter(string key, double expseconds = 300)
        {
            if (!string.IsNullOrWhiteSpace(key))
            {
                key = key.Trim();
                var enterKey = string.Format(EnterKey, Request.FromUserName);
                HttpRuntime.Cache.Insert(enterKey, key, null, DateTime.Now.AddSeconds(expseconds), System.Web.Caching.Cache.NoSlidingExpiration);
                OnEnter(key);
            }
        }

        public void Exit(string key = null)
        {
            var enterKey = string.Format(EnterKey, Request.FromUserName);
            var cachekey = HttpRuntime.Cache[enterKey] as string;
            if (string.IsNullOrWhiteSpace(key) || key == cachekey)
            {
                HttpRuntime.Cache.Remove(enterKey);
                OnExit(cachekey);
            }
        }
        /// <summary>
        /// 返回的key不为空
        /// </summary>
        /// <returns></returns>
        public string CheckGetEnterKey()
        {
            var enterKey = string.Format(EnterKey, Request.FromUserName);
            return HttpRuntime.Cache[enterKey] as string;
        }

        public void OnEnter(string key)
        {

        }

        public void OnExit(string key)
        {

        }

        #endregion
        /// <summary>
        /// 执行微信请求
        /// </summary>
        public void Execute()
        {
            if (CancelExcute)
            {
                return;
            }

            OnExecuting();

            if (CancelExcute)
            {
                return;
            }


            if (Request == null)
            {
                return;
            }

            //重复推送信息处理，缓存回复30秒 
            var key = string.Format("xNet_Wx_Cache_{0}_{1}_{2}", Request.FromUserName, Request.CreateTime, Request.MsgId);
            var cache = HttpRuntime.Cache[key];
            if (cache != null)
            {
                Response = cache as IWRespBase;
                return;
            }
            try
            {
                var enterkey = CheckGetEnterKey();
                if (!string.IsNullOrWhiteSpace(enterkey))
                {
                    Response = DoForRequest_Enter(enterkey);
                }
                if (Response == null)
                {
                    switch (Request.MsgType)
                    {
                        case WReqType.Text:
                            var wReqText = Request as WReqText;
                            Response = DoForRequest_Text(wReqText);
                            break;
                        case WReqType.Location:
                            Response = DoForRequest_Location(Request as WReqLocation);
                            break;
                        case WReqType.Image:
                            Response = DoForRequest_Image(Request as WReqImage);
                            break;
                        case WReqType.Voice:
                            Response = DoForRequest_Voice(Request as WReqVoice);
                            break;
                        case WReqType.Link:
                            Response = DoForRequest_Link(Request as WReqLink);
                            break;
                        case WReqType.Video:
                            Response = DoForRequest_Video(Request as WReqVideo);
                            break;
                        case WReqType.Event:
                            Response = DoForRequest_Event(Request as WReqEventBase);
                            break;
                        default:
                            throw new Exception("未知的MsgType请求类型", null);
                    }
                }
                if (Response == null)
                {
                    bool needRequestDefault = true;
                    if (Request.MsgType == WReqType.Event)
                    {//上报地理位置 不需要默认回复
                        WReqEventBase wreqevent = Request as WReqEventBase;
                        if (wreqevent != null && wreqevent.Event == EventType.Location)
                        {
                            needRequestDefault = false;
                        }
                    }
                    if (needRequestDefault)
                    {
                        Response = DoForRequest_Default(Request as WReqBase);
                    }

                }
                if (Response != null)
                {
                    HttpRuntime.Cache.Insert(key, Response, null, DateTime.Now.AddSeconds(30), System.Web.Caching.Cache.NoSlidingExpiration);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Execute Root:" + ex.Message);
            }
            finally
            {
                OnExecuted();
            }
        }

        /// <summary>
        /// Execute之后执行的方法
        /// </summary>
        public virtual void OnExecuted()
        {
        }
        #region DoForRequest

        #region MsgType类型
        /// <summary>
        /// 处理文本消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Text(WReqText request)
        {
            return null;
        }
        /// <summary>
        /// 处理图片消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Image(WReqImage request)
        {
            return null;
        }
        /// <summary>
        /// 处理声音消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Voice(WReqVoice request)
        {
            return null;
        }
        /// <summary>
        /// 处理链接消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Link(WReqLink request)
        {
            return null;
        }
        /// <summary>
        /// 处理视频消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Video(WReqVideo request)
        {
            return null;
        }
        /// <summary>
        /// 处理地理位置消息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Location(WReqLocation request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event(WReqEventBase request)
        {
            switch (request.Event)
            {
                case EventType.Subscribe: return DoForRequest_Event_Subscribe(Request as WReqEventSubscribe);
                case EventType.UnSubscribe: return DoForRequest_Event_Unsubscribe(Request as WReqEventUnSubscribe);
                case EventType.Scan: return DoForRequest_Event_Scan(Request as WReqEventScan);
                case EventType.Location: return DoForRequest_Event_Location(Request as WReqEventLocation);
                case EventType.Click: return DoForRequest_Event_Click(Request as WReqEventClick);
                case EventType.View: return DoForRequest_Event_View(Request as WReqEventView);
                case EventType.Masssendjobfinish: return DoForRequest_Event_MasssendJobFinish(Request as WReqEventMasssendJobFinish);
                case EventType.Card: return DoForRequest_Event_Card(Request as WReqEventCard);
                default: return DoForRequest_Event_Undefine(request);
            }
        }
        #endregion

        #region Event类型
        /// <summary>
        /// 处理事件推送--订阅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Subscribe(WReqEventSubscribe request)
        {
            return null;
        }
        protected virtual IWRespBase DoForRequest_Event_Card(WReqEventCard request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--扫描带参数二维码事件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Scan(WReqEventScan request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--取消订阅
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Unsubscribe(WReqEventUnSubscribe request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--上报地理位置
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Location(WReqEventLocation request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--自定义菜单点击事件
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Click(WReqEventClick request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--页面链接跳转
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_View(WReqEventView request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--未定义
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_Undefine(WReqEventBase request)
        {
            return null;
        }
        /// <summary>
        /// 处理事件推送--群发回执
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected virtual IWRespBase DoForRequest_Event_MasssendJobFinish(WReqEventBase request)
        {
            return null;
        }
        #endregion

        protected virtual IWRespBase DoForRequest_Enter(string key)
        {
            return null;
        }
        /// <summary>
        /// 处理没有匹配到的所有事件的默认实现
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected abstract IWRespBase DoForRequest_Default(WReqBase request);

        #endregion DoForRequest
    }
}

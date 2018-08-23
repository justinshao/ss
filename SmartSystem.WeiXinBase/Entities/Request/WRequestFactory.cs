using System;
using System.Xml.Linq;
using Common.Utilities.Helpers;

namespace SmartSystem.WeiXinBase
{
    public static class WRequestFactory
    {
        public static IWReqBase GetRequestEntity(XDocument doc)
        {
            WReqBase request = null;
            try
            {
                if (doc.Root != null)
                {
                    var xElement = doc.Root.Element("MsgType");
                    if (xElement != null)
                    {
                        var msgType = EnumHelper.ParseEnum(xElement.Value, WReqType.Text);
                        switch (msgType)
                        {
                            case WReqType.Text:
                                request = new WReqText();
                                break;
                            case WReqType.Location:
                                request = new WReqLocation();
                                break;
                            case WReqType.Image:
                                request = new WReqImage();
                                break;
                            case WReqType.Voice:
                                request = new WReqVoice();
                                break;
                            case WReqType.Video:
                                request = new WReqVideo();
                                break;
                            case WReqType.Link:
                                request = new WReqLink();
                                break;
                            case WReqType.Event:    //判断Event类型
                                var eventElement = doc.Root.Element("Event");
                                if (eventElement != null)
                                {
                                    var eventType = EnumHelper.ParseEnum(eventElement.Value, EventType.UnDefine);
                                    switch (eventType)
                                    {
                                        case EventType.Subscribe://订阅（关注）
                                            request = new WReqEventSubscribe();
                                            break;
                                        case EventType.UnSubscribe://取消订阅（关注）
                                            request = new WReqEventUnSubscribe();
                                            break;
                                        case EventType.Scan://二维码扫描
                                            request = new WReqEventScan();
                                            break;
                                        case EventType.Click://菜单点击
                                            request = new WReqEventClick();
                                            break;
                                        case EventType.View://Url跳转
                                            request = new WReqEventView();
                                            break;
                                        case EventType.Location://地理位置上报
                                            request = new WReqEventLocation();
                                            break;
                                        case EventType.Masssendjobfinish:
                                            request = new WReqEventMasssendJobFinish();
                                            break;
                                        case EventType.UnDefine: {
                                                if (eventElement.Value.ToLower().Contains("card")) {
                                                    WReqEventCard cardRequest = new WReqEventCard();
                                                    cardRequest.EventKey = eventElement.Value.ToLower();
                                                    request = cardRequest;
                                                    doc.Root.Element("Event").SetValue("Card");
                                                    break;
                                                }
                                                request = new WReqEventBase();
                                                //LogServices.WriteTxtLogEx("WRequestFactory", "未定义的Event{0},Xml:{1}", eventElement.Value, doc.ToString());
                                                break;
                                            }
                                        default: //其他意外类型（也可以选择抛出异常）
                                            request = new WReqEventBase();
                                            //LogServices.WriteTxtLogEx("WRequestFactory", "未定义的Event{0},Xml:{1}", eventElement.Value, doc.ToString());
                                            break;
                                    }
                                }
                                break;
                            default:
                                //为了能够对类型变动最大程度容错（如微信目前还可以对公众账号suscribe等未知类型，但API没有开放），建议在使用的时候catch这个异常
                                //LogServices.WriteTxtLogEx("WRequestFactory", "未定义的MsgType{0},Xml:{1}", xElement.Value, doc.ToString());
                                throw new Exception(string.Format("MsgType：{0} 在RequestFactory中没有对应的处理程序！", msgType), new ArgumentOutOfRangeException());
                        }
                        request.FillWithXml(doc);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new Exception(string.Format("Request转换出错！可能是MsgType不存在！，XML：{0} ,错误信息：{1}", doc.ToString(SaveOptions.None), ex.Message), ex);
            }
            return request;
        }
    }
}

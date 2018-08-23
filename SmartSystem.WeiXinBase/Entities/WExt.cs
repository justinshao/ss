using System;
using System.Xml.Linq;


namespace SmartSystem.WeiXinBase
{
    public static class WExt
    {
        /// <summary>
        /// 获取响应类型实例，并初始化
        /// </summary>
        /// <param name="requestMessage">请求</param>
        /// <returns></returns>
        public static T CreateResponse<T>(this IWReqBase requestMessage) where T : WRespBase, new()
        {
            var response = new T
            {
                ToUserName = requestMessage.FromUserName,
                FromUserName = requestMessage.ToUserName,
                CreateTime = DateTime.Now
            };
            return response;
        }

        public static WRespText CreateTextResponse(this IWReqBase requestMessage, string content)
        {
            if (content == null)
                content = string.Empty;
            var response = new WRespText
            {
                ToUserName = requestMessage.FromUserName,
                FromUserName = requestMessage.ToUserName,
                CreateTime = DateTime.Now,
                Content = content
            };
            return response;
        }

        public static WRespImage CreateImageResponse(this IWReqBase requestMessage, string mediaId)
        {
            var response = new WRespImage
            {
                ToUserName = requestMessage.FromUserName,
                FromUserName = requestMessage.ToUserName,
                CreateTime = DateTime.Now,
                Image = { MediaId = mediaId }
            };
            return response;
        }

        /// <summary>
        /// 从返回结果XML转换成IResponseMessageBase实体类
        /// </summary>
        /// <param name="xml">返回给服务器的Response Xml</param>
        /// <returns></returns>
        public static IWRespBase CreateResponseFromXml(string xml)
        {
            try
            {
                if (string.IsNullOrEmpty(xml))
                {
                    return null;
                }

                var doc = XDocument.Parse(xml);
                WRespBase responseMessage = null;
                if (doc.Root != null)
                {
                    var xElement = doc.Root.Element("MsgType");
                    if (xElement != null)
                    {
                        var msgType = (WRespType)Enum.Parse(typeof(WRespType), xElement.Value, true);
                        switch (msgType)
                        {
                            case WRespType.Text:
                                responseMessage = new WRespText();
                                break;
                            case WRespType.Image:
                                responseMessage = new WRespImage();
                                break;
                            case WRespType.Voice:
                                responseMessage = new WRespVoice();
                                break;
                            case WRespType.Video:
                                responseMessage = new WRespVideo();
                                break;
                            case WRespType.Music:
                                responseMessage = new WRespMusic();
                                break;
                            case WRespType.News:
                                responseMessage = new WRespNews();
                                break;
                            default:
                                responseMessage = new WRespBase();
                                //LogServices.WriteTxtLogEx("CreateResponseFromXml", "未定义的RespType：{0}，XML：{1}", xElement.Value, xml);
                                break;
                        }
                    }
                }

                responseMessage.FillWithXml(doc);
                return responseMessage;
            }
            catch (Exception ex)
            {
                throw new Exception("CreateResponseFromXml过程发生异常！" + ex.Message, ex);
            }
        }
    }
}

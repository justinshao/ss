using System;
using System.Xml.Linq;
using Common.Utilities.Helpers;

namespace SmartSystem.WeiXinBase
{
    public class WResponseFactory
    {
        //<?xml version="1.0" encoding="utf-8"?>
        //<xml>
        //  <ToUserName><![CDATA[olPjZjsXuQPJoV0HlruZkNzKc91E]]></ToUserName>
        //  <FromUserName><![CDATA[gh_a96a4a619366]]></FromUserName>
        //  <CreateTime>63497820384</CreateTime>
        //  <MsgType>text</MsgType>
        //  <Content><![CDATA[您刚才发送了文字信息：中文
        //您还可以发送【位置】【图片】【语音】信息，查看不同格式的回复。
        //SDK官方地址：http://weixin.senparc.com]]></Content>
        //  <FuncFlag>0</FuncFlag>
        //</xml>

        /// <summary>
        /// 获取XDocument转换后的IResponseMessageBase实例（通常在反向读取日志的时候用到）。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IWRespBase GetResponse(XDocument doc)
        {
            WRespBase responseMessage = null;
            try
            {
                if (doc.Root != null)
                {
                    var xElement = doc.Root.Element("MsgType");
                    if (xElement != null)
                    {
                        var msgType = EnumHelper.ParseEnum(xElement.Value, WRespType.Text);
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
                                throw new Exception(
                                    string.Format("MsgType：{0} 在ResponseMessageFactory中没有对应的处理程序！", msgType),
                                    new ArgumentOutOfRangeException());
                        }
                        responseMessage.FillWithXml(doc);
                    }
                }
            }
            catch (ArgumentException ex)
            {
                throw new Exception(string.Format("ResponseMessage转换出错！可能是MsgType不存在！，XML：{0}", doc), ex);
            }
            return responseMessage;
        }


        /// <summary>
        /// 获取XDocument转换后的IRequestMessageBase实例。
        /// 如果MsgType不存在，抛出UnknownRequestMsgTypeException异常
        /// </summary>
        /// <returns></returns>
        public static IWRespBase GetResponse(string xml)
        {
            return GetResponse(XDocument.Parse(xml));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using Common.Utilities.Helpers;

namespace SmartSystem.WeiXinBase
{
    public static class WxXml
    {
        public static void FillWithXml<T>(this T entity, string xml) where T : class,new()
        {
            entity.FillWithXml(XDocument.Parse(xml));
        }

        public static void FillWithXml<T>(this T entity, XDocument doc) where T : class,new()
        {
            entity = entity ?? new T();
            var root = doc.Root;
            if (root == null)
            {
                return;
            }
            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (root.Element(propName) != null)
                {
                    var xElement = root.Element(propName);
                    if (xElement == null) continue;
                    switch (prop.PropertyType.Name)
                    {
                        //case "String":
                        //    goto default;
                        case "DateTime":
                            prop.SetValue(entity, DateTimeHelper.TransferUnixDateTime(long.Parse(xElement.Value)), null);
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                prop.SetValue(entity, xElement.Value == "1", null);
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "Int32":
                            prop.SetValue(entity, int.Parse(xElement.Value), null);
                            break;
                        case "Int64":
                            prop.SetValue(entity, long.Parse(xElement.Value), null);
                            break;
                        case "Double":
                            prop.SetValue(entity, double.Parse(xElement.Value), null);
                            break;
                        //以下为枚举类型
                        case "WReqType":
                            //已设为只读
                            break;
                        case "WRespType"://Response适用
                            //已设为只读
                            break;
                        case "EventType":
                            //已设为只读
                            break;
                        //以下为实体类型
                        case "List`1"://List<T>类型，Article适用
                            var genericArguments = prop.PropertyType.GetGenericArguments();
                            if (genericArguments[0].Name == "Article")//Response适用
                            {
                                //文章下属节点item
                                var articles = new List<Article>();
                                foreach (var item in xElement.Elements("item"))
                                {
                                    var article = new Article();
                                    article.FillWithXml(new XDocument(item));
                                    articles.Add(article);
                                }
                                prop.SetValue(entity, articles, null);
                            }
                            break;
                        case "Music"://Music适用
                            var music = new Music();
                            music.FillWithXml(new XDocument(root.Element(propName)));
                            prop.SetValue(entity, music, null);
                            break;
                        case "Image"://Image适用
                            var image = new Image();
                            image.FillWithXml(new XDocument(root.Element(propName)));
                            prop.SetValue(entity, image, null);
                            break;
                        case "Voice"://Voice适用
                            var voice = new Voice();
                            voice.FillWithXml(new XDocument(root.Element(propName)));
                            prop.SetValue(entity, voice, null);
                            break;
                        case "Video"://Video适用
                            var video = new Video();
                            video.FillWithXml(new XDocument(root.Element(propName)));
                            prop.SetValue(entity, video, null);
                            break;
                        default:
                            prop.SetValue(entity, xElement.Value, null);
                            break;
                    }
                }
            }
        }
        
        /// <summary>
        /// 将实体转为XML
        /// </summary>
        /// <typeparam name="T">RequestMessage或ResponseMessage</typeparam>
        /// <param name="entity">实体</param>
        /// <returns></returns>
        public static XDocument ConvertToXml<T>(this T entity) where T : class , new()
        {
            entity = entity ?? new T();
            var doc = new XDocument();
            doc.Add(new XElement("xml"));
            var root = doc.Root;
            Debug.Assert(root != null, "root != null");
            /* 注意！
             * 经过测试，微信对字段排序有严格要求，这里对排序进行强制约束
            */
            var propNameOrder = new List<string> { "ToUserName", "FromUserName", "CreateTime", "MsgType" };
            //不同返回类型需要对应不同特殊格式的排序
            if (entity is WRespNews)
            {
                propNameOrder.AddRange(new[] { "ArticleCount", "Articles", "FuncFlag",/*以下是Atricle属性*/ "Title ", "Description ", "PicUrl", "Url" });
            }
            else if (entity is WRespMusic)
            {
                propNameOrder.AddRange(new[] { "Music", "FuncFlag", "ThumbMediaId",/*以下是Music属性*/ "Title ", "Description ", "MusicUrl", "HQMusicUrl" });
            }
            else if (entity is WRespImage)
            {
                propNameOrder.AddRange(new[] { "Image",/*以下是Image属性*/ "MediaId " });
            }
            else if (entity is WRespVoice)
            {
                propNameOrder.AddRange(new[] { "Voice",/*以下是Voice属性*/ "MediaId " });
            }
            else if (entity is WRespVideo)
            {
                propNameOrder.AddRange(new[] { "Video",/*以下是Video属性*/ "MediaId ", "Title", "Description" });
            }
            else
            {
                //如Text类型
                propNameOrder.AddRange(new[] { "Content", "FuncFlag" });
            }

            var props = entity.GetType().GetProperties().OrderBy(p => propNameOrder.IndexOf(p.Name)).ToList();
            foreach (var prop in props)
            {
                var propName = prop.Name;
                if (propName == "Articles")
                {
                    //文章列表
                    var atriclesElement = new XElement("Articles");
                    var articales = prop.GetValue(entity, null) as List<Article>;
                    if (articales != null)
                    {
                        foreach (var articale in articales)
                        {
                            var xElement = articale.ConvertToXml().Root;
                            if (xElement != null)
                            {
                                var subNodes = xElement.Elements();
                                atriclesElement.Add(new XElement("item", subNodes));
                            }
                        }
                    }
                    root.Add(atriclesElement);
                }
                else if (propName == "Music" || propName == "Image" || propName == "Video" || propName == "Voice")
                {
                    //音乐、图片、视频、语音格式
                    var musicElement = new XElement(propName);
                    var media = prop.GetValue(entity, null);// as Music;
                    var xElement = ConvertToXml(media).Root;
                    if (xElement != null)
                    {
                        var subNodes = xElement.Elements();
                        musicElement.Add(subNodes);
                    }
                    root.Add(musicElement);
                }
                else
                {
                    switch (prop.PropertyType.Name)
                    {
                        case "String":
                            root.Add(new XElement(propName, new XCData(prop.GetValue(entity, null) as string ?? "")));
                            break;
                        case "DateTime":
                            root.Add(new XElement(propName, DateTimeHelper.TransferUnixDateTime((DateTime)prop.GetValue(entity, null))));
                            break;
                        case "Boolean":
                            if (propName == "FuncFlag")
                            {
                                root.Add(new XElement(propName, (bool)prop.GetValue(entity, null) ? "1" : "0"));
                            }
                            else
                            {
                                goto default;
                            }
                            break;
                        case "WRespType":
                            root.Add(new XElement(propName, new XCData(prop.GetValue(entity, null).ToString().ToLower())));
                            break;
                        case "Article":
                            root.Add(new XElement(propName, prop.GetValue(entity, null).ToString().ToLower()));
                            break;
                        default:
                            root.Add(new XElement(propName, prop.GetValue(entity, null)));
                            break;
                    }
                }
            }
            return doc;
        }
    }
}

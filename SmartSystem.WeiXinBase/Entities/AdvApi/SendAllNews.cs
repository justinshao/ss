using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SmartSystem.WeiXinBase
{
    public class SendAllNews
    {
        /// <summary>
        /// 图文消息，一个图文消息支持1到10条图文
        /// </summary>
        public List<SendAllNewsItem> articles { get; set; }
    }

    public class SendAllNewsItem
    {
        /// <summary>
        /// 图文消息缩略图的media_id
        /// </summary>
        public string thumb_media_id { get; set; }
        /// <summary>
        /// 图文消息的作者
        /// </summary>
        public string author { get; set; }
        /// <summary>
        /// 图文消息的标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 在图文消息页面点击“阅读原文”后的页面
        /// </summary>
        public string content_source_url { get; set; }
        /// <summary>
        /// 图文消息页面的内容，支持HTML标签
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// 图文消息的描述
        /// </summary>
        public string digest { get; set; }
    }
}

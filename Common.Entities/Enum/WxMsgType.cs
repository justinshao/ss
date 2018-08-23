using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Common.Entities.Enum
{
    public enum WxMsgType
    {
        /// <summary>
        /// 文字
        /// </summary>
        [Description("文字")]
        Text,
        /// <summary>
        /// 新闻
        /// </summary>
        [Description("新闻")]
        News,
        /// <summary>
        /// 音乐
        /// </summary>
        [Description("音乐")]
        Music,
        /// <summary>
        /// 图片
        /// </summary>
        [Description("图片")]
        Image,
        /// <summary>
        /// 语音
        /// </summary>
        [Description("语音")]
        Voice,
        /// <summary>
        /// 视频
        /// </summary>
        [Description("视频")]
        Video
    }
}

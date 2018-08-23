using System.Collections.Generic;

namespace SmartSystem.WeiXinBase
{
    public class WRespNews : WRespBase
    {
        public override WRespType MsgType
        {
            get { return WRespType.News; }
        }

        public int ArticleCount
        {
            get { return (Articles ?? new List<Article>()).Count; }
            set { }
        }

        /// <summary>
        /// 文章列表，微信客户端只能输出前10条（可能未来数字会有变化，出于视觉效果考虑，建议控制在8条以内）
        /// </summary>
        public List<Article> Articles { get; set; }

        public WRespNews()
        {
            Articles = new List<Article>();
        }
    }
}

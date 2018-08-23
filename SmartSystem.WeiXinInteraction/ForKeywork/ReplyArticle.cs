using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Entities.WX;
using SmartSystem.WeiXinBase;
using Common.Entities.Enum;
using System.IO;
using Common.Services.WeiXin;

namespace SmartSystem.WeiXinInteraction.ForKeywork
{
    public class ReplyArticle : IKey
    {
        public IWRespBase ReplyContent(WX_ApiConfig config,WX_Keyword gKey, WReqBase request, string qValue)
        {
            var response = request.CreateResponse<WRespNews>();
            var articles = WXArticleServices.QueryByGroupID(gKey.ArticleGroupID);
            foreach (var item in articles.OrderBy(o => o.Sort))
            {
                var article = new Article
                {
                    Title = item.Title,
                    Description = item.Description,
                    PicUrl = string.IsNullOrWhiteSpace(item.ImagePath) ? string.Empty : string.Format("{0}{1}", config.Domain, item.ImagePath)
                };
                switch (item.ArticleType)
                {
                    case ArticleType.Text:
                        article.Url = string.Format("{0}/WxArticleDetail/Index?openId={1}&articleId={2}", config.Domain, request.FromUserName, item.ID);
                        break;
                    case ArticleType.Url:
                        article.Url = item.Url;
                        break;
                    case ArticleType.Module:
                        {
                            string link = ((WeiXinModule)int.Parse(item.Url)).GetEnumDefaultValue();
                            if (!string.IsNullOrWhiteSpace(link))
                            {
                                if (link.Contains("="))
                                {
                                    link = string.Format("{0}^cid={1}", link, config.CompanyID);
                                }
                                else
                                {
                                    link = string.Format("{0}_cid={1}", link, config.CompanyID);
                                }
                            }
                            article.Url = string.Format("{0}{1}", config.Domain, link);
                            break;
                        }
                    default: continue;
                }
                response.Articles.Add(article);
            }
            return response;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Caching;

namespace Common.Utilities
{
    public class DataCache
    {



        #region 缓存依赖文件web.config

        private static string _webconfigfile = string.Empty;

        /// <summary>  

        /// 缓存依赖文件web.config  

        /// </summary>  

        public static string webconfigfile
        {

            get
            {

                if (_webconfigfile == string.Empty) _webconfigfile = HttpContext.Current.Server.MapPath("/web.config");

                return _webconfigfile;

            }

        }

        #endregion



        #region 删除缓存

        /// <summary>  

        /// 删除缓存  

        /// </summary>  

        /// <param name="CacheKey">键</param>  

        public static void DeleteCache(string CacheKey)
        {

            HttpRuntime.Cache.Remove(CacheKey);

        }

        #endregion



        #region 获取缓存

        /// <summary>  

        /// 获取缓存  

        /// </summary>  

        /// <param name="CacheKey">键</param>  

        /// <returns></returns>  

        public static object GetCache(string CacheKey)
        {

            return HttpRuntime.Cache[CacheKey];

        }

        #endregion



        #region 简单的插入缓存

        /// <summary>  

        /// 简单的插入缓存  

        /// </summary>  

        /// <param name="CacheKey">键</param>  

        /// <param name="objObject">数据</param>  

        public static void SetCache(string CacheKey, object objObject)
        {

            HttpRuntime.Cache.Insert(CacheKey, objObject);

        }

        #endregion



        #region 有过期时间的插入缓存数据

        /// <summary>  

        /// 有过期时间的插入缓存数据  

        /// </summary>  

        /// <param name="CacheKey">键</param>  

        /// <param name="objObject">数据</param>  

        /// <param name="absoluteExpiration">过期时间</param>  

        /// <param name="slidingExpiration">可调度参数，传null就是禁用可调度</param>  

        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {

            if (slidingExpiration == null) slidingExpiration = Cache.NoSlidingExpiration;

            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);

        }
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, Cache.NoSlidingExpiration);

        }
        #endregion



        #region 插入缓存数据，指定缓存多少秒

        /// <summary>  

        /// 插入缓存数据，指定缓存多少秒  

        /// </summary>  

        /// <param name="CacheKey">缓存的键</param>  

        /// <param name="objObject">缓存的数据</param>  

        /// <param name="seconds">缓存秒数</param>  

        /// <param name="slidingExpiration">传null就是禁用可调度过期</param>  

        public static void SetCacheSecond(string CacheKey, object objObject, int seconds, TimeSpan slidingExpiration)
        {

            DateTime absoluteExpiration = DateTime.Now.AddSeconds(seconds);

            if (slidingExpiration == null) slidingExpiration = Cache.NoSlidingExpiration;

            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);

        }

        #endregion



        #region 依赖文件的缓存，文件没改不会过期

        /// <summary>  

        /// 依赖文件的缓存，文件没改不会过期  

        /// </summary>  

        /// <param name="CacheKey">键</param>  

        /// <param name="objObject">数据</param>  

        /// <param name="depfilename">依赖文件，可调用 DataCache 里的变量</param>  

        public static void SetCacheDepFile(string CacheKey, object objObject, string depfilename)
        {

            //缓存依赖对象  

            System.Web.Caching.CacheDependency dep = new System.Web.Caching.CacheDependency(depfilename);

            System.Web.Caching.Cache objCache = HttpRuntime.Cache;

            objCache.Insert(

                CacheKey,

                objObject,

                dep,

                System.Web.Caching.Cache.NoAbsoluteExpiration, //从不过期  

                System.Web.Caching.Cache.NoSlidingExpiration, //禁用可调过期  

                System.Web.Caching.CacheItemPriority.Default,

                null);

        }

        #endregion  
    }
}

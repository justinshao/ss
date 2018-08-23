using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Common.Entities
{
    /// <summary>
    /// 分页对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Paging<T> : List<T>
    {
        /// <summary>
        /// 当前页数1~num
        /// </summary>
        public int CurrentPage { get; private set; }
        /// <summary>
        /// 分页大小 1~num
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; private set; }
        /// <summary>
        /// 显示按钮与当前的页的范围
        /// </summary>
        public const int ShowSize = 3;
        /// <summary>
        /// 取得设置条件
        /// </summary>
        public Hashtable HsCondition { get; set; }

        /// <summary>
        /// 此方法是特殊处理时才会用到
        /// </summary>
        /// <param name="currentPage"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalCount"></param>
        /// <param name="totalPages"></param>
        public void GetParameter(int currentPage, int pageSize, int totalCount, int totalPages)
        {
            this.CurrentPage = currentPage;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
        }

        public Paging(IEnumerable<T> source, int currentPage, int pageSize, int totalCount)
        {
            HsCondition = new Hashtable();

            if (pageSize < 1)
                pageSize = 1;
            PageSize = pageSize;

            TotalCount = totalCount;

            TotalPages = (int)Math.Ceiling(TotalCount / (double)PageSize);

            if (currentPage > TotalPages)
                currentPage = TotalPages;
            if (currentPage < 1)
                currentPage = 1;

            CurrentPage = currentPage;

            AddRange(source);
        }
    }
}

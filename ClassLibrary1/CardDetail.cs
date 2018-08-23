using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClassLibrary1
{
    public class CardDetail
    {
        /// <summary>
        /// 
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public CardDetailResult Result { get; set; }

    }
    public class CardDetailResult
    {
        /// <summary>
        /// 
        /// </summary>
        public string IsNext { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<CardDetailItem> List { get; set; }
    }
    public class CardDetailItem
    {
        public string Title { get; set; }
        public string Price { get; set; }
        public string CreateTime { get; set; }
        public string LicensePlate { get; set; }

    }
}

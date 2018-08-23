using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.Entities.Validation
{
    public class IdentityInfo
    {
        /// <summary>
        /// 证件抓拍到的图片路径
        /// </summary>
        public string PicPath { set; get; }

        public string IdentityNo { set; get; }
        /// <summary>
        /// 证件头像的图片路径
        /// </summary>
        public string IDCardPhoto { set; get; }

        /// <summary>
        /// 进证件姓名
        /// </summary>
        public string CertName { set; get; }
        /// <summary>
        /// 进证件性别
        /// </summary>
        public string Sex { set; get; }
        /// <summary>
        /// 进证件名族
        /// </summary>
        public string Nation { set; get; }
        /// <summary>
        /// 进证件生日
        /// </summary>
        public DateTime BirthDate { set; get; }
        /// <summary>
        /// 进证件住址
        /// </summary>
        public string Address { set; get; }
    }
}

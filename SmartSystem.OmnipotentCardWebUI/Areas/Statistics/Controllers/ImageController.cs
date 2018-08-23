using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.IO;
using Common.Entities.Other;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Statistics.Controllers
{
    public class ImageController : Controller
    {
        string ImagePath =System.Configuration.ConfigurationManager.AppSettings["ImagePath"];
        /// <summary>
        /// 获得进场图片
        /// </summary>
        /// <param name="imgpath">进场图片路径</param>
        /// <returns></returns>
        public JsonResult GetEntranceImages(string ImagePathEntrance)
        {
            try
            {
                if (string.IsNullOrEmpty(ImagePathEntrance))
                    throw new Exception("图片为空");
                ImgModel img = new ImgModel();
                string path = System.IO.Path.Combine(ImagePath, ImagePathEntrance);
                img.ImgBig = GetPic(path);
                path = path.Replace("Big", "Small");
                img.ImgSmall = GetPic(path);
                if (img.ImgSmall.Length == 0)
                {
                    img.ImgSmall = LoadingPic(false);
                }
                if (img.ImgBig.Length == 0)
                {
                    img.ImgBig = LoadingPic(true);
                }
                object entranceimg = new
                {
                    Base64ImageEntrance = img.ImgBig,
                    Base64ImageEntranceSmall = img.ImgSmall
                };
                return Json(entranceimg, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                object entranceimg = new
                {
                    Base64ImageEntrance = LoadingPic(true),
                    Base64ImageEntranceSmall = LoadingPic(false)
                };
                TxtLogServices.WriteTxtLog("获取进场图片异常 异常信息:{0}",ex.Message);
                return Json(entranceimg, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetImages(string ImagePathEntrance, string ImagePathExit)
        {
            try
            {
                ImgModel imgentrance = new ImgModel();
                ImgModel imgexit = new ImgModel();
                if (!string.IsNullOrEmpty(ImagePathEntrance))
                {
                    string path = System.IO.Path.Combine(ImagePath, ImagePathEntrance);
                    imgentrance.ImgBig = GetPic(path);
                    path = path.Replace("Big", "Small");
                    imgentrance.ImgSmall = GetPic(path);
                   
                }
                if (!string.IsNullOrEmpty(ImagePathExit))
                {
                    string path = System.IO.Path.Combine(ImagePath, ImagePathExit);
                    imgexit.ImgBig = GetPic(path);
                    path = path.Replace("Big", "Small");
                    imgexit.ImgSmall = GetPic(path);
                }
                if (imgentrance.ImgSmall.Length == 0 || imgexit.ImgSmall.Length == 0)
                {
                    var v = LoadingPic(false);
                    if (imgentrance.ImgSmall.Length == 0)
                    {
                        imgentrance.ImgSmall = v;
                    }
                    if (imgexit.ImgSmall.Length == 0)
                    {
                        imgexit.ImgSmall = v;
                    }
                }
                if (imgentrance.ImgBig.Length == 0 || imgexit.ImgBig.Length == 0)
                {
                    var v = LoadingPic(true);
                    if (imgentrance.ImgBig.Length == 0)
                    {
                        imgentrance.ImgBig = v;
                    }
                    if (imgexit.ImgBig.Length == 0)
                    {
                        imgexit.ImgBig = v;
                    }
                }
                object entranceimg = new
                {
                    Base64ImageEntrance = imgentrance.ImgBig,
                    Base64ImageExit = imgexit.ImgBig,
                    Base64ImageEntranceSmall = imgentrance.ImgSmall,
                    Base64ImageExitSmall = imgexit.ImgSmall
                };
                return Json(entranceimg, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                TxtLogServices.WriteTxtLog("获取进出场图片异常 异常信息:{0}", ex.Message);
                string bigimg = LoadingPic(true);
                string smallimg = LoadingPic(false);
                object entranceimg = new
                {
                    Base64ImageEntrance = bigimg,
                    Base64ImageExit = bigimg,
                    Base64ImageEntranceSmall = smallimg,
                    Base64ImageExitSmall = smallimg
                };
                return Json(entranceimg, JsonRequestBehavior.AllowGet);
            }
        }

        string GetPic(string imagepath)
        {
            string imagedata = "";
            if (System.IO.File.Exists(imagepath))
            {
                Stream s = System.IO.File.Open(imagepath, FileMode.Open);
                try
                {
                    byte[] bt = new byte[s.Length];
                    s.Read(bt, 0, (int)s.Length);
                    imagedata = Convert.ToBase64String(bt);
                }
                catch (Exception ex)
                {
                    Common.Services.TxtLogServices.WriteTxtLog("获取图片异常 图片路径:{0} 异常信息:{1}", imagepath, ex.Message);
                }
                finally
                {
                    s.Close();
                }
            }
            else
            {
                TxtLogServices.WriteTxtLog("图片文件路径不存在", imagepath);
 
            }
            return imagedata;
        }

        /// <summary>
        /// 获取默认图片
        /// </summary>
        /// <param name="IsBig">是否大图</param>
        /// <returns></returns>
        string LoadingPic(bool IsBig)
        {
            string defaultbigpic = HttpContext.Server.MapPath("~/Content/images/default_not_image_big.png");
            if( !IsBig)
                defaultbigpic = HttpContext.Server.MapPath("~/Content/images/default_not_image_small.png");
            System.IO.FileStream emptyreader = new System.IO.FileStream(defaultbigpic, System.IO.FileMode.Open);
            byte[] emptyimage = new byte[emptyreader.Length];
            emptyreader.Read(emptyimage, 0, emptyimage.Length);
            emptyreader.Close();
            return Convert.ToBase64String(emptyimage);
        }
    }
}

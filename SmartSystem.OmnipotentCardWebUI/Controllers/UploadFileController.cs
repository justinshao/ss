using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using System.IO;
using Common.Services;

namespace SmartSystem.OmnipotentCardWebUI.Controllers
{
    public class UploadFileController : BaseController
    {
        /// <summary>
        /// 将文件上传到指定路径中保存
        /// </summary>
        /// <returns>上传文件结果信息</returns>
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostCretData()
        {
            string info = string.Empty;
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    if (fileName != "apiclient_cert.p12"){
                        return Json(MyResult.Error("上传的文件名称不正确"));
                    }

                    string filePath = string.Format("{0}/{1}/{2}", "Uploads", Request["companyId"].ToString(), "Cret");

                    string targetDir = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), filePath);
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    //组合成文件的完整路径
                    string path = System.IO.Path.Combine(targetDir,fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    //保存上传的文件到指定路径中
                    file.SaveAs(path);
                    string savePath = string.Format("/{0}/{1}", filePath, fileName);
                    return Json(MyResult.Success(string.Empty, savePath));
                }
                else
                {
                    return Json(MyResult.Error("获取上传文件失败"));
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "上传支付证书失败");
                return Json(MyResult.Error("上传文件异常"));
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostWeiXinLogo()
        {
            string info = string.Empty;
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    var extension = Path.GetExtension(file.FileName);
                    if (extension != null && (extension.ToUpper() != ".JPG" &&
                                                                         extension.ToUpper() != ".GIF"
                                                                         && extension.ToUpper() != ".PNG" &&
                                                                         extension.ToUpper() != ".JPEG"))
                    {
                        throw new MyException("选择的文件类型不正确（必须为JPG,GIF,PNG,JPEG文件）");
                    }

                    string fileName = string.Format("{0}{1}", DateTime.Now.Ticks, extension);
                    string filePath = string.Format("{0}/{1}/{2}", "Uploads", Request["companyId"].ToString(), "WeiXinImages");

                    string targetDir = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), filePath);
                    if (!Directory.Exists(targetDir))
                    {
                        Directory.CreateDirectory(targetDir);
                    }
                    //组合成文件的完整路径
                    string path = System.IO.Path.Combine(targetDir, fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    //保存上传的文件到指定路径中
                    file.SaveAs(path);
                    string savePath = string.Format("/{0}/{1}", filePath,fileName);
                    return Json(MyResult.Success(string.Empty, savePath));
                }
                else
                {
                    return Json(MyResult.Error("获取上传的图片失败"));
                }
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "获取获微信LOGO图片失败");
                return Json(MyResult.Error("上传图片失败"));
            }

        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult PostWeiXinVerificationFile() {
            string info = string.Empty;
            try
            {
                HttpFileCollection files = System.Web.HttpContext.Current.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    var extension = Path.GetExtension(file.FileName);
                    if (extension != ".txt")
                    {
                        return Json(MyResult.Error("上传的文件必须为txt文件"));
                    }
                    string fileName = System.IO.Path.GetFileName(file.FileName);
                    string path = System.IO.Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    //保存上传的文件到指定路径中
                    file.SaveAs(path);
                    return Json(MyResult.Success());
                }
                else
                {
                    return Json(MyResult.Error("获取上传文件失败"));
                }
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "上传支付证书失败");
                return Json(MyResult.Error("上传文件异常"));
            }
        }
    }
}

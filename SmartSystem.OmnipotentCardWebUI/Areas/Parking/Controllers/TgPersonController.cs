using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common.Entities;
using SmartSystem.OmnipotentCardWebUI.Controllers;
using Common.Services.Statistics;
using Common.Entities.Statistics;
using System.Drawing;
using System.Drawing.Drawing2D;
using ClassLibrary1;
using System.Net;
using System.IO;
using Common.Services;
using Common.Services.BB;
using Common.Entities.Other;
using System.Text;
using Common.Utilities.Helpers;
using SmartSystem.WeiXinServices;

namespace SmartSystem.OmnipotentCardWebUI.Areas.Parking.Controllers
{
    public class TgPersonController :BaseController
    {
        //
        // GET: /Parking/TgPerson/
        [CheckPurview(Roles = "PK010108")]
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GETtgOperatePurview()
        {
            JsonResult tg = new JsonResult();
            List<tgOperatePurview> options = new List<tgOperatePurview>();
            List<SysRoleAuthorize> roleAuthorizes = GetLoginUserRoleAuthorize.Where(p => p.ParentID == "PK010108").ToList();
            foreach (var item in roleAuthorizes)
            {
                switch (item.ModuleID)
                {
                    case "PK01010801":
                        {
                            tgOperatePurview option = new tgOperatePurview();
                            option.text = "增加";
                            option.sort = 1;
                            option.handler = "Add";
                            options.Add(option);
                            break;
                        }
                    case "PK01010802":
                        {
                            tgOperatePurview option = new tgOperatePurview();
                            option.text = "修改";
                            option.sort = 2;
                            option.handler = "Update";
                            options.Add(option);
                            break;
                        }
                    case "PK01010803":
                        {
                            tgOperatePurview option = new tgOperatePurview();
                            option.text = "删除";
                            option.sort = 3;
                            option.handler = "Delete";
                            options.Add(option);
                            break;
                        }
                    case "PK01010804":
                        {
                            tgOperatePurview option = new tgOperatePurview();
                            option.text = "刷新";
                            option.sort = 4;
                            option.handler = "Refresh";
                            options.Add(option);
                            break;
                        }
                    case "PK01010805":
                        {
                            tgOperatePurview option = new tgOperatePurview();
                            option.text = "下载二维码";
                            option.sort = 5;
                            option.id = "btndownloadqrcode";
                            option.handler = "DownloadQRCode";
                            option.iconCls = "icon-import";
                            options.Add(option);
                            break;
                        }
                }
            }

            tg.Data = options.OrderBy(p => p.sort);
            return tg;
        }



        //初始化
        public string tgPersonInfo() { 
            string str = tgPersonServices.infotgPerson();
            return str;
        }

        [HttpPost]
        public JsonResult EdittgPerson(tgPerson modle) {
            bool result = false;

            try {
                if (Request["title"]=="增加")
                {
                    result = tgPersonServices.Addperson(modle);
                    if (!result) throw new Exception();
                    return Json(prompt.Success("添加成功!"));
                }
                else {
                    result = tgPersonServices.UpdatePerson(modle);
                    return Json(prompt.Success("修改成功!"));
                }
            }catch(Exception ex){
                return Json(prompt.Error(ex.Message));
            }
        }


        [HttpPost]
        public JsonResult Delete(string PersonId)
        {
            try
            {
                bool result = tgPersonServices.DeletePerson("PersonTg", "id", PersonId);
                if (!result) throw new MyException("删除人员失败");
                return Json(MyResult.Success());
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                 return Json(MyResult.Error("人员失败"));
            }
        }
        //根据人员和车场信息 下载二维码
        public JsonResult DownloadQRCode(string parkingId, string personId,int size)
        {
            try
            {
                List<int> dics = new List<int>();
                dics.Add(258);
                dics.Add(344);
                dics.Add(430);
                dics.Add(860);
                dics.Add(1280);

                List<string> imgs = new List<string>();
                //if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                //{
                //    throw new MyException("获取系统域名失败");
                //}
                BaseParkinfo parking = ParkingServices.QueryParkingByParkingID(parkingId);
                if (parking == null) throw new MyException("获取车场信息失败");

                BaseVillage village = VillageServices.QueryVillageByRecordId(parking.VID);
                if (village == null) throw new MyException("获取小区信息失败");
                //string url = SystemDefaultConfig.SystemDomain;
                string url = "http://spscs.spsing.cn";
                string content = string.Format("{0}/qrl/qrp_ix_pid={1}^personId={2}", url, parkingId.Trim(), personId);
                foreach (var item in dics)
                {
                    try
                    {
                        string parkingName = string.Format("{0}_{1}", parking.PKName, item);
                        string result = QRCodeServices.GenerateQRCode(village.CPID, content, item, parkingName);
                        imgs.Add(item.ToString() + "|" + result);
                        TxtLogServices.WriteTxtLogEx("DownloadQRCode", item.ToString() + "|" + result);
                    }
                    catch (Exception ex)
                    {
                        ExceptionsServices.AddExceptions(ex, "生存车场二维码失败");
                        imgs.Add(item.ToString() + "|");
                    }

                }

                return Json(MyResult.Success("", imgs));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "下载二维码失败");
                return Json(MyResult.Error("下载二维码失败"));
            }
        }

        //根据车场id和时间查询人员记录
        public string QuerytgPersonInfo()
        { 
            try
            {
                InParams paras = new InParams
                {
                    ParkingID = Request.Params["parkingid"],
                    StartTime = DateTime.Parse(Request.Params["starttime"]),
                    EndTime = DateTime.Parse(Request.Params["endtime"]),
                };
                int page = string.IsNullOrEmpty(Request.Params["page"]) ? 0 : int.Parse(Request.Params["page"]);
                int rows = string.IsNullOrEmpty(Request.Params["rows"]) ? 0 : int.Parse(Request.Params["rows"]);
                Pagination pagination = tgPersonServices.Search_DailyStatistics(paras, rows, page);
                StringBuilder sb = new StringBuilder();
                sb.Append("{");
                sb.Append("\"total\":" + pagination.Total + ",");
                sb.Append("\"rows\":" + JsonHelper.GetJsonString(pagination.StatisticsGatherList) + ",");
                sb.Append("\"index\":" + rows);
                sb.Append("}");
                return sb.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }




        [HttpPost]
        public JsonResult getEWM(string parkingId)
        {
            //string strSql = "select AppId,AppSecret from WX_ApiConfig";
            //using (DbOperator dboperator = ConnectionManager.CreateReadConnection())
            //{
            //    DataTable dt = dboperator.ExecuteTable(strSql, 30000);
            //    if (dt.Columns.Count > 0)
            //    {
            //        appid = dt.Rows[0][0].ToString();
            //        appsecret = dt.Rows[0][1].ToString();
            //    }
            //    else
            //    {
            //        throw new MyException("获取AppId失败!");
            //    }
            //}

            string appid = appidsecret.appid(); string appsecret = appidsecret.appsecret();
            AccessToken Token = wxApi.GetToken(appid, appsecret);
            int sss = Convert.ToInt32(parkingId);
            ewm em = wxApi.CreateQrCode("", Token.Accesstoken, sss);

            //代码Logo的二维码下载
            string downloadUrl = string.Format("https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket={0}", em.Ticket);
            string urlPath = "/Uploads/QRCode/" + parkingId + "_258.jpg"; // out 文件路径

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(downloadUrl);
            req.Method = "GET";
            using (WebResponse wr = req.GetResponse())
            {
                HttpWebResponse myResponse = (HttpWebResponse)req.GetResponse();
                string strpath = myResponse.ResponseUri.ToString();
                WebClient mywebclient = new WebClient();


                //开始了
                //素净的二维码
                byte[] bytelist = mywebclient.DownloadData(strpath);

                MemoryStream ms1 = new MemoryStream(bytelist);
                Bitmap b1 = (Bitmap)Image.FromStream(ms1);
                ms1.Close();

                //logo图片
                Bitmap b2 = new Bitmap(AppDomain.CurrentDomain.BaseDirectory + @"Uploads/bb.png");

                //合并
                var ret = new ImageUtility().MergeQrImg(b1, b2, 1);
                Image img = ret;
                img.Save(AppDomain.CurrentDomain.BaseDirectory + urlPath);

                //返回最终路径

            }

            try
            {
                List<int> dics = new List<int>();
                dics.Add(258);
                dics.Add(344);
                dics.Add(430);
                dics.Add(860);
                dics.Add(1280);

                List<string> imgs = new List<string>();
                if (string.IsNullOrWhiteSpace(SystemDefaultConfig.SystemDomain))
                {
                    throw new MyException("获取系统域名失败");
                }
                string content = string.Format("{0}/qrl/qrp_ix_pid={1}", SystemDefaultConfig.SystemDomain, parkingId);
                foreach (var item in dics)
                {
                    try
                    {
                        string result = urlPath;
                        imgs.Add(item.ToString() + "|" + result);
                    }
                    catch (Exception ex)
                    {
                        ExceptionsServices.AddExceptions(ex, "生存人员二维码失败");
                        imgs.Add(item.ToString() + "|");
                    }

                }

                return Json(MyResult.Success("", imgs));
            }
            catch (MyException ex)
            {
                return Json(MyResult.Error(ex.Message));
            }
            catch (Exception ex)
            {
                ExceptionsServices.AddExceptions(ex, "下载二维码失败");
                return Json(MyResult.Error("下载二维码失败"));
            }

        }


        public class ImageUtility
        {
            #region 合并用户QR图片和用户头像
            /// <summary>   
            /// 合并用户QR图片和用户头像   
            /// </summary>   
            /// <param name="qrImg">QR图片</param>   
            /// <param name="headerImg">用户头像</param>   
            /// <param name="n">缩放比例</param>   
            /// <returns></returns>   
            public Bitmap MergeQrImg(Bitmap qrImg, Bitmap headerImg, double n = 0.23)
            {
                int margin = 10;
                float dpix = qrImg.HorizontalResolution;
                float dpiy = qrImg.VerticalResolution;
                var _newWidth = (10 * qrImg.Width - 46 * margin) * 1.0f / 46;
                var _headerImg = ZoomPic(headerImg, _newWidth / headerImg.Width);
                //处理头像   
                int newImgWidth = _headerImg.Width + margin;
                Bitmap headerBgImg = new Bitmap(newImgWidth, newImgWidth);
                headerBgImg.MakeTransparent();
                Graphics g = Graphics.FromImage(headerBgImg);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.Clear(Color.Transparent);
                Pen p = new Pen(new SolidBrush(Color.White));
                //位置和大小
                Rectangle rect = new Rectangle(0, 0, newImgWidth - 1, newImgWidth - 1);
                using (GraphicsPath path = CreateRoundedRectanglePath(rect, 7))
                {
                    g.DrawPath(p, path);
                    g.FillPath(new SolidBrush(Color.White), path);
                }
                //画头像   
                Bitmap img1 = new Bitmap(_headerImg.Width, _headerImg.Width);
                Graphics g1 = Graphics.FromImage(img1);
                g1.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g1.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g1.Clear(Color.Transparent);
                //Pen p1 = new Pen(new SolidBrush(Color.Gray));
                Pen p1 = new Pen(new SolidBrush(Color.White));
                Rectangle rect1 = new Rectangle(0, 0, _headerImg.Width - 1, _headerImg.Width - 1);
                using (GraphicsPath path1 = CreateRoundedRectanglePath(rect1, 8))
                {
                    g1.DrawPath(p1, path1);
                    TextureBrush brush = new TextureBrush(_headerImg);
                    g1.FillPath(brush, path1);
                }
                g1.Dispose();
                PointF center = new PointF((newImgWidth - _headerImg.Width) / 2, (newImgWidth - _headerImg.Height) / 2);
                g.DrawImage(img1, center.X, center.Y, _headerImg.Width, _headerImg.Height);
                g.Dispose();
                Bitmap backgroudImg = new Bitmap(qrImg.Width, qrImg.Height);
                backgroudImg.MakeTransparent();
                backgroudImg.SetResolution(dpix, dpiy);
                headerBgImg.SetResolution(dpix, dpiy);
                Graphics g2 = Graphics.FromImage(backgroudImg);
                g2.Clear(Color.Transparent);
                g2.DrawImage(qrImg, 0, 0);
                PointF center2 = new PointF((qrImg.Width - headerBgImg.Width) / 2, (qrImg.Height - headerBgImg.Height) / 2);
                g2.DrawImage(headerBgImg, center2);
                g2.Dispose();
                return backgroudImg;
            }
            #endregion

            #region 图形处理
            /// <summary>   
            /// 创建圆角矩形   
            /// </summary>   
            /// <param name="rect">区域</param>   
            /// <param name="cornerRadius">圆角角度</param>   
            /// <returns></returns>   
            private GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
            {
                //下午重新整理下，圆角矩形   
                GraphicsPath roundedRect = new GraphicsPath();
                roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
                roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
                roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
                roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
                roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
                roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
                roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
                roundedRect.CloseFigure();
                return roundedRect;
            }
            /// <summary>   
            /// 图片按比例缩放   
            /// </summary>   
            private Image ZoomPic(Image initImage, double n)
            {
                //缩略图宽、高计算   
                double newWidth = initImage.Width;
                double newHeight = initImage.Height;
                newWidth = n * initImage.Width;
                newHeight = n * initImage.Height;
                //生成新图   
                //新建一个bmp图片   
                System.Drawing.Image newImage = new System.Drawing.Bitmap((int)newWidth, (int)newHeight);
                //新建一个画板   
                System.Drawing.Graphics newG = System.Drawing.Graphics.FromImage(newImage);
                //设置质量   
                newG.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                newG.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //置背景色   
                newG.Clear(Color.Transparent);
                //画图   
                newG.DrawImage(initImage, new System.Drawing.Rectangle(0, 0, newImage.Width, newImage.Height), new System.Drawing.Rectangle(0, 0, initImage.Width, initImage.Height), System.Drawing.GraphicsUnit.Pixel);
                newG.Dispose();
                return newImage;
            }
            #endregion

        }

    }
}

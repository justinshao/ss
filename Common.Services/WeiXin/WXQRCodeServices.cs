using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThoughtWorks.QRCode.Codec;
using Common.Entities.WX;
using Common.Services.WeiXin;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Web;

namespace Common.Services.WeiXin
{
    public class WXQRCodeServices
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="content">二维码内容</param>
        /// <param name="size">二维码大小</param>
        /// <param name="qrCodeName">二维码文件名称</param>
        /// <returns>保存二维码的路径</returns>
        public static string GenerateQRCode(string companyId,string content, int size, string qrCodeName)
        {
            QRCodeEncoder.ENCODE_MODE QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            QRCodeEncoder.ERROR_CORRECTION QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            int border = GetQRCodeBorder(size);
            string imageUrl = string.Empty;
            int QRCodeVersion = 0; int QRCodeScale = 5;
            WX_ApiConfig config = WXApiConfigServices.QueryWXApiConfig(companyId);
            if (config != null && !string.IsNullOrWhiteSpace(config.SystemLogo))
            {
                imageUrl = config.SystemLogo;
            }
            var bitmap = GenerateQRCode(content, QRCodeEncodeMode, QRCodeErrorCorrect, QRCodeVersion, QRCodeScale, size, border);
            if (bitmap == null)
            {
                return string.Empty;
            }
            string filePath = string.Format("/Uploads/QRCode/{0}.{1}", qrCodeName, "jpg");
            string savePath = GetFileSavePath("/Uploads/QRCode", string.Format("{0}.{1}", qrCodeName, "jpg"));
            string pathImage = string.Empty;
            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                pathImage = System.Web.HttpContext.Current.Server.MapPath("~" + imageUrl);
            }
            if (!string.IsNullOrWhiteSpace(pathImage) && File.Exists(pathImage))
            {
                var logoSize = GetLogoImageSize(size);
                Image bitmap1 = CombinImage(bitmap, pathImage, logoSize);
                bitmap1.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            else
            {
                bitmap.Save(savePath, System.Drawing.Imaging.ImageFormat.Jpeg);
            }
            return filePath;
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="domain">域名</param>
        /// <param name="parkingId">车场编号</param>
        /// <param name="plateNo">车牌号</param>
        /// <param name="size">二维码大小 258、344、430、860、1280</param>
        /// <param name="logoPath">二维码中间小图路径</param>
        /// <returns>二维码字节</returns>
        public static byte[] GenerateByteQRCode(string domain,string parkingId,string plateNo, int size,string logoPath)
        {
            //if (!string.IsNullOrWhiteSpace(plateNo)) {
            //    plateNo = HttpUtility.UrlEncode(plateNo, Encoding.UTF8);
            //}
            string content = string.Format("{0}/qrl/qrp_ix_pid={1}^pn={2}", domain.TrimEnd('/'), parkingId, plateNo);

            QRCodeEncoder.ENCODE_MODE QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            QRCodeEncoder.ERROR_CORRECTION QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.M;
            int border = GetQRCodeBorder(size);
 
            int QRCodeVersion = 0; int QRCodeScale = 5;

          
            var bitmap = GenerateQRCode(content, QRCodeEncodeMode, QRCodeErrorCorrect, QRCodeVersion, QRCodeScale, size, border);
            if (bitmap == null)
            {
                return null;
            }
            string pathImage = string.Empty;
            if (!string.IsNullOrWhiteSpace(logoPath))
            {
                pathImage = System.Web.HttpContext.Current.Server.MapPath("~" + logoPath);
            }
            if (!string.IsNullOrWhiteSpace(pathImage) && File.Exists(pathImage))
            {
                var logoSize = GetLogoImageSize(size);
                Image bitmap1 = CombinImage(bitmap, pathImage, logoSize);
               return GetByteImage(bitmap1);
            }
            else
            {
                return GetByteImage(bitmap);

            }
        }
        //将image转化为二进制 
        public static byte[] GetByteImage(Image img)
        {
            byte[] bt = null;
            if (!img.Equals(null))
            {
                using (MemoryStream mostream = new MemoryStream())
                {
                    Bitmap bmp = new Bitmap(img);
                    bmp.Save(mostream, System.Drawing.Imaging.ImageFormat.Jpeg);//将图像以指定的格式存入缓存内存流
                    bt = new byte[mostream.Length];
                    mostream.Position = 0;//设置留的初始位置
                    mostream.Read(bt, 0, Convert.ToInt32(bt.Length));
                }
            }
            return bt;
        }
        private static string GetFileSavePath(string relativelyPath, string savefileName)
        {
            string absolutePath = System.Web.HttpContext.Current.Server.MapPath(relativelyPath);

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            return Path.Combine(absolutePath, savefileName);
        }
        private static int GetQRCodeBorder(int size)
        {
            switch (size)
            {
                case 258:
                    {
                        //0.5M
                        return 8;
                    }
                case 344:
                    {
                        //0.8M
                        return 12;
                    }
                case 430:
                    {
                        //1M
                        return 15;
                    }
                case 860:
                    {
                        //1.5M
                        return 30;
                    }
                case 1280:
                    {
                        //2M
                        return 50;
                    }
                default: return 10;
            }
        }
        private static int GetLogoImageSize(int size)
        {
            switch (size)
            {
                case 258:
                    {
                        //0.5M
                        return 60;
                    }
                case 344:
                    {
                        //0.8M
                        return 76;
                    }
                case 430:
                    {
                        //1M
                        return 95;
                    }
                case 860:
                    {
                        //1.5M
                        return 180;
                    }
                case 1280:
                    {
                        //2M
                        return 200;
                    }
                default: return 60;
            }
        }
        /// <summary>
        /// 生成二维码
        /// </summary>
        /// <param name="Content">内容文本</param>
        /// <param name="QRCodeEncodeMode">二维码编码方式</param>
        /// <param name="QRCodeErrorCorrect">纠错码等级</param>
        /// <param name="QRCodeVersion">二维码版本号 0-40</param>
        /// <param name="QRCodeScale">每个小方格的预设宽度（像素），正整数</param>
        /// <param name="size">图片尺寸（像素），0表示不设置</param>
        /// <param name="border">图片白边（像素），当size大于0时有效</param>
        /// <returns></returns>
        public static System.Drawing.Image GenerateQRCode(string Content, QRCodeEncoder.ENCODE_MODE QRCodeEncodeMode, QRCodeEncoder.ERROR_CORRECTION QRCodeErrorCorrect, int QRCodeVersion, int QRCodeScale, int size, int border)
        {
            QRCodeEncoder qrCodeEncoder = new QRCodeEncoder();
            qrCodeEncoder.QRCodeEncodeMode = QRCodeEncodeMode;
            qrCodeEncoder.QRCodeErrorCorrect = QRCodeErrorCorrect;
            qrCodeEncoder.QRCodeScale = QRCodeScale;
            qrCodeEncoder.QRCodeVersion = QRCodeVersion;
            System.Drawing.Image image = qrCodeEncoder.Encode(Content, Encoding.UTF8);

            #region 根据设定的目标图片尺寸调整二维码QRCodeScale设置，并添加边框
            if (size > 0)
            {
                //当设定目标图片尺寸大于生成的尺寸时，逐步增大方格尺寸
                #region 当设定目标图片尺寸大于生成的尺寸时，逐步增大方格尺寸
                while (image.Width < size)
                {
                    qrCodeEncoder.QRCodeScale++;
                    System.Drawing.Image imageNew = qrCodeEncoder.Encode(Content, Encoding.UTF8);
                    if (imageNew.Width < size)
                    {
                        image = new System.Drawing.Bitmap(imageNew);
                        imageNew.Dispose();
                        imageNew = null;
                    }
                    else
                    {
                        qrCodeEncoder.QRCodeScale--; //新尺寸未采用，恢复最终使用的尺寸
                        imageNew.Dispose();
                        imageNew = null;
                        break;
                    }
                }
                #endregion

                //当设定目标图片尺寸小于生成的尺寸时，逐步减小方格尺寸
                #region 当设定目标图片尺寸小于生成的尺寸时，逐步减小方格尺寸
                while (image.Width > size && qrCodeEncoder.QRCodeScale > 1)
                {
                    qrCodeEncoder.QRCodeScale--;
                    System.Drawing.Image imageNew = qrCodeEncoder.Encode(Content, Encoding.UTF8);
                    image = new System.Drawing.Bitmap(imageNew);
                    imageNew.Dispose();
                    imageNew = null;
                    if (image.Width < size)
                    {
                        break;
                    }
                }
                #endregion

                //如果目标尺寸大于生成的图片尺寸，则为图片增加白边
                #region 如果目标尺寸大于生成的图片尺寸，则为图片增加白边
                if (image.Width <= size)
                {
                    //根据参数设置二维码图片白边的最小宽度
                    #region 根据参数设置二维码图片白边的最小宽度
                    if (border > 0)
                    {
                        while (image.Width <= size && size - image.Width < border * 2 && qrCodeEncoder.QRCodeScale > 1)
                        {
                            qrCodeEncoder.QRCodeScale--;
                            System.Drawing.Image imageNew = qrCodeEncoder.Encode(Content, Encoding.UTF8);
                            image = new System.Drawing.Bitmap(imageNew);
                            imageNew.Dispose();
                            imageNew = null;
                        }
                    }
                    #endregion

                    //当目标图片尺寸大于二维码尺寸时，将二维码绘制在目标尺寸白色画布的中心位置
                    if (image.Width < size)
                    {
                        //新建空白绘图
                        System.Drawing.Bitmap panel = new System.Drawing.Bitmap(size, size);

                        System.Drawing.Graphics graphic0 = System.Drawing.Graphics.FromImage(panel);
                        graphic0.Clear(Color.White);
                        int p_left = 0;
                        int p_top = 0;
                        if (image.Width <= size) //如果原图比目标形状宽
                        {
                            p_left = (size - image.Width) / 2;
                        }
                        if (image.Height <= size)
                        {
                            p_top = (size - image.Height) / 2;
                        }

                        //将生成的二维码图像粘贴至绘图的中心位置
                        graphic0.DrawImage(image, p_left, p_top, image.Width, image.Height);
                        image = new System.Drawing.Bitmap(panel);
                        panel.Dispose();
                        panel = null;
                        graphic0.Dispose();
                        graphic0 = null;
                    }
                }
                #endregion
            }
            #endregion
            return image;
        }
        private static bool IsTrue() // 在Image类别对图片进行缩放的时候,需要一个返回bool类别的委托 
        {
            return true;
        }
        private static Image CombinImage(Image imgBack, string destImg, int size)
        {

            Image img = Image.FromFile(destImg);        //照片图片      
            img = KiResizeImage(img, size, size, 0);

            Graphics g = Graphics.FromImage(imgBack);

            g.DrawImage(imgBack, 0, 0, imgBack.Width, imgBack.Height);      //g.DrawImage(imgBack, 0, 0, 相框宽, 相框高);     

            g.DrawImage(img, imgBack.Width / 2 - img.Width / 2, imgBack.Width / 2 - img.Width / 2, img.Width, img.Height);
            GC.Collect();
            return imgBack;
        }
        /// <summary>    
        /// Resize图片    
        /// </summary>    
        /// <param name="bmp">原始Bitmap</param>    
        /// <param name="newW">新的宽度</param>    
        /// <param name="newH">新的高度</param>    
        /// <param name="Mode">保留着，暂时未用</param>    
        /// <returns>处理以后的图片</returns>    
        private static Image KiResizeImage(Image bmp, int newW, int newH, int Mode)
        {
            try
            {
                int size = newW + GetLogoImageBorberSize(newW);
                //新建空白绘图
                Image panel = new System.Drawing.Bitmap(size, size);

                System.Drawing.Graphics graphic0 = System.Drawing.Graphics.FromImage(panel);
                graphic0.Clear(Color.White);
                int p_left = 0;
                int p_top = 0;
                if (newW <= size) //如果原图比目标形状宽
                {
                    p_left = (size - newW) / 2;
                }
                if (newH <= size)
                {
                    p_top = (size - newH) / 2;
                }

                //将生成的二维码图像粘贴至绘图的中心位置
                graphic0.DrawImage(bmp, p_left, p_top, newW, newH);
                //image = new System.Drawing.Bitmap(panel);

                //Image b = new Bitmap(newW, newH);
                //Graphics g = Graphics.FromImage(b);
                //// 插值算法的质量    
                //g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                //g.DrawImage(bmp, new Rectangle(10, 10, newW, newH), new Rectangle(5, 5, bmp.Width, bmp.Height), GraphicsUnit.Pixel);
                //g.Dispose();
                return panel;
            }
            catch
            {
                return null;
            }
        }
        private static int GetLogoImageBorberSize(int size)
        {
            switch (size)
            {
                case 60:
                    {
                        //0.5M
                        return 8;
                    }
                case 76:
                    {
                        //0.8M
                        return 8;
                    }
                case 95:
                    {
                        //1M
                        return 10;
                    }
                case 180:
                    {
                        //1.5M
                        return 16;
                    }
                case 200:
                    {
                        //2M
                        return 20;
                    }
                default: return 10;
            }
        }
    }
}

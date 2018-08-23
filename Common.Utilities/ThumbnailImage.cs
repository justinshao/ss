using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Encoder = System.Text.Encoder;

namespace Common.Utilities
{
    public class ThumbnailImage
    {
        /// <summary>
        /// 保存图片，不对图片做任何处理
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="relativelyPath">图片保存的相对路径</param>
        /// <returns></returns>
        public static string Save(HttpPostedFileBase fileData, string relativelyPath)
        {
            CheckFile(fileData);

            string fileName = GetSaveFileName(fileData.FileName);
            string savefileName = GetFileSavePath(relativelyPath, fileName);

            fileData.SaveAs(savefileName);
            return string.Format("{0}/{1}", relativelyPath.TrimEnd('/'), fileName);
        }

        /// <summary>
        /// 压缩保存图片
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="relativelyPath">图片保存的相对路径</param>
        /// <returns></returns>
        public static string CompressionSave(HttpPostedFileBase fileData, string relativelyPath)
        {
            CheckFile(fileData);

            string fileName = GetSaveFileName(fileData.FileName);
            string savefileName = GetFileSavePath(relativelyPath, fileName);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fileData.InputStream, true))
            {

                Compress(image, savefileName, 50);
            }
            return string.Format("{0}/{1}", relativelyPath.TrimEnd('/'), fileName);
        }
        private static string GetSaveFileName(string fileName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + Path.GetExtension(fileName);
        }
        private static string GetSaveFileNameByCustmExtension(string extensionName)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfffffff") + "." + extensionName;
        }

        public static string GetFileSavePath(string relativelyPath, string savefileName)
        {
            string absolutePath = System.Web.HttpContext.Current.Server.MapPath(relativelyPath);

            if (!Directory.Exists(absolutePath))
                Directory.CreateDirectory(absolutePath);

            return Path.Combine(absolutePath, savefileName);
        }

        private static void CheckFile(HttpPostedFileBase fileData)
        {
            if (fileData == null || fileData.ContentLength == 0)
                throw new MyException("请选择需要上传的图片");

            var extension = Path.GetExtension(fileData.FileName);
            if (extension != null && (extension.ToUpper() != ".JPG" &&
                                                                 extension.ToUpper() != ".GIF"
                                                                 && extension.ToUpper() != ".PNG" &&
                                                                 extension.ToUpper() != ".JPEG"))
            {
                throw new MyException("选择的文件类型不正确（必须为JPG,GIF,PNG,JPEG文件）");
            }
        }
        /// <summary>
        /// 生成缩略图，根据指定的宽和高缩量
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="relativelyPath">保存文件的相对路径</param>
        /// <param name="width">宽</param>
        /// <param name="height">高</param>
        /// <returns></returns>
        public static string ThumbnailImageProcess(HttpPostedFileBase fileData, string relativelyPath, int width, int height)
        {
            return ThumbnailImageProcess(fileData, relativelyPath, width, height, false, false);
        }
        /// <summary>
        /// 生成缩略图
        /// </summary>
        /// <param name="fileData">文件</param>
        /// <param name="relativelyPath">相对路径</param>
        /// <param name="width">图片宽度</param>
        /// <param name="height">图片高度</param>
        /// <param name="needCompression">是否需要压缩图片</param>
        /// <param name="needProportional">是否需要根据宽度等比例压缩</param>
        /// <returns></returns>
        public static string ThumbnailImageProcess(HttpPostedFileBase fileData, string relativelyPath, int width, int height, bool needCompression, bool needProportional)
        {
            if (width < 1 || height < 1) throw new MyException("文件指定宽或者高不正确");

            CheckFile(fileData);

            string savefileName = GetSaveFileNameByCustmExtension("jpeg");
            string filePath = GetFileSavePath(relativelyPath, savefileName);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fileData.InputStream, true))
            {
                int newWidth = width;
                int newHeight = height;
                if (needProportional)
                {
                    int owidth = image.Width;
                    int oheight = image.Height;
                    if (width > owidth)
                    {
                        newWidth = owidth;
                        newHeight = oheight;
                    }
                    else
                    {
                        newWidth = width;
                        newHeight = (int)oheight * width / owidth;
                    }
                }
                ThumbnailImageProcess(image, filePath, newWidth, newHeight, needCompression);
            }
            return string.Format("{0}/{1}", relativelyPath.TrimEnd('/'), savefileName);
        }

        public static string ThumbnailImageProcess(HttpPostedFileBase fileData, string relativelyPath, double proportion, bool needCompression)
        {
            if (proportion <= 0 || proportion > 1) throw new MyException("比例值必须大于0并且小于等于1");

            CheckFile(fileData);

            string savefileName = GetSaveFileNameByCustmExtension("jpeg");
            string filePath = GetFileSavePath(relativelyPath, savefileName);

            using (System.Drawing.Image image = System.Drawing.Image.FromStream(fileData.InputStream, true))
            {
                int newWidth = (int)(image.Width * proportion);
                int newHeight = (int)(image.Height * proportion);

                ThumbnailImageProcess(image, filePath, newWidth, newHeight, needCompression);
            }
            return string.Format("{0}/{1}", relativelyPath.TrimEnd('/'), savefileName);
        }

        /// <summary>
        /// 生成缩略图，根据比例来缩略
        /// </summary>
        /// <param name="fileData"></param>
        /// <param name="relativelyPath">保存文件的相对路径</param>
        /// <param name="proportion">计算的比例，值大于0并且小于等于1</param>
        /// <returns></returns>
        public static string ThumbnailImageProcess(HttpPostedFileBase fileData, string relativelyPath, double proportion)
        {
            return ThumbnailImageProcess(fileData, relativelyPath, proportion, false);
        }
        private static void ThumbnailImageProcess(System.Drawing.Image image, string imageSavePath, int newWidth, int newHeight, bool needCompression)
        {
            System.Drawing.Size size = new System.Drawing.Size((int)newWidth, (int)newHeight); //设置图片的宽度和高度
            using (System.Drawing.Image bitmap = new System.Drawing.Bitmap(size.Width, size.Height))  //新建bmp图片
            {
                using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(bitmap))  //新建画板
                {
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;  //制定高质量插值法
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;   //设置高质量、低速度呈现平滑程度
                    g.Clear(System.Drawing.Color.FromArgb(0, 0, 0, 0));    //清空画布
                    //在制定位置画图
                    g.DrawImage(image, new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), new System.Drawing.Rectangle(0, 0, image.Width, image.Height), System.Drawing.GraphicsUnit.Pixel);
                    //文字水印
                    using (System.Drawing.Graphics testGrahpics = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        using (System.Drawing.Font font = new System.Drawing.Font("宋体", 10))
                        {
                            using (System.Drawing.Brush brush = new System.Drawing.SolidBrush(System.Drawing.Color.Black))
                            {
                                //分行
                                string sInput = "";    //获取输入的水印文字
                                int coloum = 0;    //获取每行的字符数
                                //利用循环，来依次输出
                                for (int i = 0, j = 0; i < sInput.Length; i += coloum, j++)
                                {
                                    //若要修改水印文字在照片上的位置，可将20修改成你想要的任何值
                                    if (j != sInput.Length / coloum)
                                    {
                                        string s = sInput.Substring(i, coloum);
                                        testGrahpics.DrawString(s, font, brush, 20, 20 * (i / coloum + 1));
                                    }
                                    else
                                    {
                                        string s = sInput.Substring(i, sInput.Length % coloum);
                                        testGrahpics.DrawString(s, font, brush, 20, 20 * (j + 1));
                                    }
                                }
                            }
                        }
                    }
                    if (needCompression)
                    {
                        Compress(bitmap, imageSavePath, 50);
                    }
                    else
                    {
                        bitmap.Save(imageSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                }
            }
        }

        /// <summary>
        /// 图片剪裁
        /// </summary>
        /// <param name="originalPath">需要剪裁的图片路径</param>
        /// <param name="width">剪裁的宽度</param>
        /// <param name="height">剪裁的高度</param>
        /// <param name="x">剪裁的x坐标</param>
        /// <param name="y">剪裁的y坐标</param>
        /// <param name="relativelyPath"></param>
        /// <param name="needCompression">是否需要压缩图片</param>
        /// <returns></returns>
        public static string Crop(string originalPath, int width, int height, int x, int y, string relativelyPath, bool needCompression)
        {
            return Crop(originalPath, width, height, x, y, relativelyPath, needCompression, "jpeg", false);
        }


        /// <summary>
        /// 图片剪裁(Png格式)
        /// </summary>
        /// <param name="originalPath">需要剪裁的图片路径</param>
        /// <param name="width">剪裁的宽度</param>
        /// <param name="height">剪裁的高度</param>
        /// <param name="x">剪裁的x坐标</param>
        /// <param name="y">剪裁的y坐标</param>
        /// <param name="relativelyPath"></param>
        /// <param name="needCompression">是否需要压缩图片</param>
        /// <returns></returns>
        public static string CropPng(string originalPath, int width, int height, int x, int y, string relativelyPath, bool needCompression)
        {
            return Crop(originalPath, width, height, x, y, relativelyPath, needCompression, "png", true);
        }
        /// <summary>
        /// 图片剪裁(Png格式)
        /// </summary>
        /// <param name="originalPath">需要剪裁的图片路径</param>
        /// <param name="width">剪裁的宽度</param>
        /// <param name="height">剪裁的高度</param>
        /// <param name="x">剪裁的x坐标</param>
        /// <param name="y">剪裁的y坐标</param>
        /// <param name="relativelyPath"></param>
        /// <param name="needCompression">是否需要压缩图片</param>
        /// <returns></returns>
        private static string Crop(string originalPath, int width, int height, int x, int y, string relativelyPath, bool needCompression, string extensionName, bool isPngImage)
        {
            if (width == 0 || height == 0) return originalPath;

            string savePath = System.Web.HttpContext.Current.Server.MapPath(relativelyPath);
            string originalImagePath = System.Web.HttpContext.Current.Server.MapPath(originalPath);

            string savefileName = GetSaveFileNameByCustmExtension(extensionName);

            if (!File.Exists(originalImagePath)) throw new MyException("图片路径不正确");

            CropImage(originalImagePath, width, height, x, y, Path.Combine(savePath, savefileName), needCompression, isPngImage);

            return string.Format("{0}/{1}", relativelyPath.TrimEnd('/'), savefileName);
        }
        /// <summary>
        /// 图片剪裁
        /// </summary>
        /// <param name="originalPath">需要剪裁的图片路径</param>
        /// <param name="width">剪裁的宽度</param>
        /// <param name="height">剪裁的高度</param>
        /// <param name="x">剪裁的x坐标</param>
        /// <param name="y">剪裁的y坐标</param>
        /// <param name="relativelyPath"></param>
        /// <returns></returns>
        public static string Crop(string originalPath, int width, int height, int x, int y, string relativelyPath)
        {
            return Crop(originalPath, width, height, x, y, relativelyPath, false);
        }

        /// <summary>
        /// 剪裁图像
        /// </summary>
        /// <param name="img"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="imageSavePath"></param>
        /// <param name="needCompression">是否需要压缩图片</param>
        /// <returns></returns>
        private static string CropImage(string img, int width, int height, int x, int y, string imageSavePath, bool needCompression, bool isPngImage)
        {

            using (var originalImage = new Bitmap(img))
            {
                using (var bmp = new Bitmap(width, height, originalImage.PixelFormat))
                {
                    bmp.SetResolution(originalImage.HorizontalResolution, originalImage.VerticalResolution);
                    using (Graphics graphic = Graphics.FromImage(bmp))
                    {
                        graphic.SmoothingMode = SmoothingMode.AntiAlias;
                        graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphic.DrawImage(originalImage, new Rectangle(0, 0, width, height), x, y, width, height,
                                            GraphicsUnit.Pixel);
                        if (needCompression)
                        {
                            Compress(bmp, imageSavePath, 50);
                        }
                        else if (isPngImage)
                        {
                            bmp.Save(imageSavePath, System.Drawing.Imaging.ImageFormat.Png);
                        }
                        else
                        {
                            bmp.Save(imageSavePath, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }

                        return imageSavePath;
                    }
                }
            }

        }
        #region 图片压缩(降低质量)Compress
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitmap">传入的Bitmap对象</param>
        /// <param name="destStream">压缩后的Stream对象</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitmap, Stream destStream, long level)
        {
            ImageCodecInfo myImageCodecInfo;
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;

            myImageCodecInfo = GetEncoderInfo("image/jpeg");

            myEncoder = System.Drawing.Imaging.Encoder.Quality;

            myEncoderParameters = new EncoderParameters(1);

            myEncoderParameter = new EncoderParameter(myEncoder, level);
            myEncoderParameters.Param[0] = myEncoderParameter;
            srcBitmap.Save(destStream, myImageCodecInfo, myEncoderParameters);
        }
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcBitMap">传入的Bitmap对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Bitmap srcBitMap, string destFile, long level)
        {
            Stream s = new FileStream(destFile, FileMode.Create);
            Compress(srcBitMap, s, level);
            s.Close();
        }
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcFile">传入的Stream对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(Stream srcStream, string destFile, long level)
        {
            Bitmap bm = new Bitmap(srcStream);
            Compress(bm, destFile, level);
            bm.Dispose();
        }
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcFile">传入的Image对象</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(System.Drawing.Image srcImg, string destFile, long level)
        {
            Bitmap bm = new Bitmap(srcImg);
            Compress(bm, destFile, level);
            bm.Dispose();
        }
        /// <summary>
        /// 图片压缩(降低质量以减小文件的大小)
        /// </summary>
        /// <param name="srcFile">待压缩的BMP文件名</param>
        /// <param name="destFile">压缩后的图片保存路径</param>
        /// <param name="level">压缩等级，0到100，0 最差质量，100 最佳</param>
        public static void Compress(string srcFile, string destFile, long level)
        {

            Bitmap bm = new Bitmap(srcFile);
            Compress(bm, destFile, level);
            bm.Dispose();
        }

        #endregion 图片压缩(降低质量)
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;

namespace Common.Utilities.Helpers
{
    public class FileHelper
    {
        public static bool CheckFileExtension(string fileName, List<string> extension)
        {
            if (string.IsNullOrWhiteSpace(fileName) || extension == null || !extension.Any())
                return false;
            var ext = Path.GetExtension(fileName).ToUpper();
            return !string.IsNullOrWhiteSpace(ext) && extension.Select(o => o.ToUpper()).Contains(ext);
        }

        public static bool HasFile(HttpPostedFileBase fileBase)
        {
            return fileBase != null && fileBase.ContentLength > 0;
        }

        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        /// <summary>
        /// 根据完整文件路径获取FileStream
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static FileStream GetFileStream(string fileName)
        {
            FileStream fileStream = null;
            if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
            {
                fileStream = new FileStream(fileName, FileMode.Open);
            }
            return fileStream;
        }
    }
}
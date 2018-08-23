using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Base.Common.Xml;


namespace SmartSystem.WeiXinBase.RedPack
{
    public class WxRedPackPayServices
    {
        public string Token { get; set; }
        public string CertFilePath { get; set; }
        public string CertPassword { get; set; }
        public string Key { get; set; }


        public WxRedPackPayServices(string certFilePath, string certPassword, string key)
        {
            CertFilePath = certFilePath;
            CertPassword = certPassword;
            Key = key;
        }

        /// <summary>
        /// 发送现金裂变红包
        /// </summary>
        public RspSendRedPack SendGroupRedPack(ReqSendGroupRedPack req)
        {
            req.SetSign(Key);
            const string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendgroupredpack";
            var data = XmlHelper.Serializer<ReqSendGroupRedPack>(req);
            var result = PostDataByCert(url, data);
            return XmlHelper.Deserialize<RspSendRedPack>(result);
        }

        /// <summary>
        /// 发送现金红包
        /// </summary>
        public RspSendRedPack SendRedPack(ReqSendRedPack req)
        {
            req.SetSign(Key);
            const string url = "https://api.mch.weixin.qq.com/mmpaymkttransfers/sendredpack";
            var data = XmlHelper.Serializer<ReqSendRedPack>(req);
            var result = PostDataByCert(url, data);
            return XmlHelper.Deserialize<RspSendRedPack>(result);
        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain,SslPolicyErrors errors)
        {
            if (errors == SslPolicyErrors.None)
                return true;
            return false;
        }

        private string PostDataByCert(string purl, string strdata)
        {
            var data = Encoding.UTF8.GetBytes(strdata);
            ServicePointManager.ServerCertificateValidationCallback = CheckValidationResult;
            //var cer = new X509Certificate(CertFilePath, CertPassword);    //只能本地使用，改成下句后，服务端能用了
            var cer = new X509Certificate2(CertFilePath, CertPassword, X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.MachineKeySet);
            var req = (HttpWebRequest)WebRequest.Create(purl);
            req.ClientCertificates.Add(cer);
            req.Method = "POST";
            req.ContentLength = data.Length;
            var stream = req.GetRequestStream();
            stream.Write(data, 0, data.Length);
            stream.Close();
            var rep = (HttpWebResponse)req.GetResponse();
            var receiveStream = rep.GetResponseStream();
            var encode = Encoding.UTF8;
            if (receiveStream == null) return string.Empty;
            var readStream = new StreamReader(receiveStream, encode);
            var result = readStream.ReadToEnd();
            rep.Close();
            readStream.Close();
            return result;
        }
    }
}

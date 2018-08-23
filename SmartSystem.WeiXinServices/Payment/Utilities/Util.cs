﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace SmartSystem.WeiXinServices.Payment
{
    public class Util
    {
          public static string CreateNoncestr(int length)
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < length; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

          public static string CreateNoncestr()
        {
            String chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            String res = "";
            Random rd = new Random();
            for (int i = 0; i < 16; i++)
            {
                res += chars[rd.Next(chars.Length - 1)];
            }
            return res;
        }

        public static string FormatQueryParaMap(Dictionary<string, string> parameters)
        {
            string buff = "";
            var result = from pair in parameters orderby pair.Key select pair;
            foreach (KeyValuePair<string, string> pair in result)
            {
                if (pair.Key != "")
                {
                    buff += pair.Key + "=" + HttpUtility.UrlEncode(pair.Value) + "&";
                }
            }
            if (buff.Length == 0 == false)
            {
                buff = buff.Substring(0, (buff.Length - 1) - (0));
            }
            return buff;
        }

        public static string FormatBizQueryParaMap(Dictionary<string, string> paraMap, bool urlencode)
        {

            string buff = "";
            var result = from pair in paraMap orderby pair.Key select pair;
            foreach (KeyValuePair<string, string> pair in result)
            {
                if (pair.Key != "")
                {

                    string key = pair.Key;
                    string val = pair.Value;
                    if (urlencode)
                    {
                        val = System.Web.HttpUtility.UrlEncode(val);
                    }
                    buff += key.ToLower() + "=" + val + "&";

                }
            }

            if (buff.Length == 0 == false)
            {
                buff = buff.Substring(0, (buff.Length - 1) - (0));
            }
            return buff;
        }

        /// <summary>
        /// 统一支付  （参数组合）
        /// </summary>
        /// <param name="paraMap"></param>
        /// <param name="urlencode"></param>
        /// <returns></returns>
        public static string FormatBizQueryParaMapForUnifiedPay(Dictionary<string, string> paraMap)
        {
            string buff = "";
            var result = from pair in paraMap orderby pair.Key select pair;
            foreach (KeyValuePair<string, string> pair in result)
            {
                if (pair.Key != "")
                {
                    string key = pair.Key;
                    string val = pair.Value;
                    buff += key + "=" + val + "&";
                }
            }

            if (buff.Length == 0 == false)
            {
                buff = buff.Substring(0, (buff.Length - 1) - (0));
            }
            return buff;
        }

        public static bool IsNumeric(String str)
        {
            try
            {
                int.Parse(str);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ArrayToXml(Dictionary<string, string> arr)
        {
            String xml = "<xml>";

            foreach (KeyValuePair<string, string> pair in arr)
            {
                String key = pair.Key;
                String val = pair.Value;
                if (IsNumeric(val))
                {
                    xml += "<" + key + ">" + val + "</" + key + ">";

                }
                else
                    xml += "<" + key + "><![CDATA[" + val + "]]></" + key + ">";
            }

            xml += "</xml>";
            return xml;
        }
    }
}

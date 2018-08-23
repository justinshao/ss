using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Common.Core.Helpers
{

    public class OptionHandler
    {

        string _filename = "Option.config";
        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        public OptionHandler(string filename)
        {
            _filename = filename;
        }
        /// <summary>
        /// 写配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="value">值</param>
        /// <returns>true为成功，false为失败</returns>
        public bool Write<T>(string section, string name, T value)
        {
            string fileName = AppDomain.CurrentDomain.BaseDirectory + _filename;
            XDocument doc = null;
            try
            {
                if (!File.Exists(fileName))
                {
                    doc = new XDocument(new XElement("Option"));
                }
                else
                {
                    try
                    {
                        doc = XDocument.Load(fileName);
                    }
                    catch (Exception e)
                    {
                        if (e is System.Xml.XmlException)
                        {
                            doc = new XDocument(new XElement("Option"));
                        }
                    }
                }

                if (doc.Root.Element(section) == null)
                {
                    doc.Root.Add(new XElement(section, new XElement(name, value)));
                }
                else
                {
                    if (doc.Root.Element(section).Element(name) == null)
                    {
                        doc.Root.Element(section).Add(new XElement(name, value));
                    }
                    else
                    {
                        doc.Root.Element(section).Element(name).SetValue(value);
                    }
                }
                doc.Save(fileName);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 写字符串配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="value">值</param>
        /// <returns>true为成功，false为失败</returns>
        public bool WriteString(string section, string name, string value)
        {
            return Write<string>(section, name, value);
        }

        /// <summary>
        /// 写整数配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="value">值</param>
        /// <returns>true为成功，false为失败</returns>
        public bool WriteInt(string section, string name, int value)
        {
            return Write<int>(section, name, value);
        }

        /// <summary>
        /// 写布尔配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="value">值</param>
        /// <returns>true为成功，false为失败</returns>
        public bool WriteBool(string section, string name, bool value)
        {
            return Write<bool>(section, name, value);
        }

        static object ReadWriteLock = new object();
        /// <summary>
        /// 读字符串配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public string ReadString(string section, string name, string defaultValue)
        {
            lock (ReadWriteLock)
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + _filename;
                if (!File.Exists(fileName))
                {
                    return defaultValue;
                }
                try
                {
                    XDocument doc = XDocument.Load(fileName);
                    if (doc.Root.Element(section) == null)
                    {
                        return defaultValue;
                    }
                    else
                    {
                        return doc.Root.Element(section).Element(name).Value; 
                    }
                }
                catch (Exception)
                {
                    return defaultValue;
                }
            }
        }

        /// <summary>
        /// 读整形配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public int ReadInt(string section, string name, int defaultValue)
        {
            return int.Parse(ReadString(section, name, defaultValue.ToString()));
        }

        /// <summary>
        /// 读整形配置项
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="section">节</param>
        /// <param name="name">项</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>值</returns>
        public bool ReadBool(string section, string name, bool defaultValue)
        {
            return bool.Parse(ReadString(section, name, defaultValue.ToString()));
        }
    }
}

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Base.Common.Xml
{
    /// <summary>
    /// XML操作类(打开，创建XML文档,删除，获取节点，路径等)
    /// </summary>
    public class XmlHelper:IDisposable {
        string m_rootName = "Doc";
        XmlDocument xmlDoc;
        string m_fileName = string.Empty;

        public string FileName
        {
            get { return m_fileName; }            
        }
        bool m_new = false;

        string encoding ="gb2312";// "utf-8";

        public string Encoding
        {
            get { return encoding; }
            set { encoding = value; }
        }

        /// <summary>
        /// 根节点名称
        /// </summary>
        public string RootName {
            get { return m_rootName; }
            set { m_rootName = value; }
        }
        /// <summary>
        /// 获取XmlDocument，如果没有创建，则自动创建
        /// </summary>
        public XmlDocument XmlDoc
        {
            get
            {
                if (this.xmlDoc == null) this.Open();
                return this.xmlDoc;
            }
            set { this.xmlDoc = value; }
        }

        #region 初始化实例
        public XmlHelper(string fileName)
        {
            m_fileName = fileName;
        }

        public XmlHelper(string rootName, string fileName)
        {
            m_rootName = rootName;
            m_fileName = fileName;
        }
        /// <summary>
        /// 初始化XmlHelper的实例
        /// </summary>
        /// <param name="rootName">跟节点名称</param>
        /// <param name="fileName">xml文档名称，包括路径</param>
        /// <param name="created">新建而不是加载原来文件，即无论原来文件是否存在，如果为true时都新建</param>
        public XmlHelper(string rootName, string fileName, bool created)
        {
            m_rootName = rootName;
            m_fileName = fileName;
            m_new = created;  //新建而不是加载原来文件
        }
        #endregion

        #region 打开和创建XML文档
        /// <summary>
        /// 更改XML文档
        /// </summary>
        /// <param name="fileName">XML文档名称，包括路劲</param>
        /// <param name="created">新建而不是加载原来文件，即无论原来文件是否存在，如果为true时都新建</param>
        public void ChangeFile(string fileName, bool created)
        {
            this.m_fileName = fileName;
            this.m_new = created;
            this.Open();
        }

        public void LoadFromString(string content)
        {
            XmlDoc.LoadXml(content);
        }

        public void LoadFromFile(string fileName,Encoding encoding)
        {
            if (!File.Exists(fileName))
                return;

            string content = File.ReadAllText(fileName, encoding);
            this.LoadFromString(content);
        }

        public void Create()
        {
            CreateXml();
            Dispose();
        }

        private void Open()
        {
            if (m_new)
                this.CreateXml();
            else
                this.SetXmlDoc();
        }

        public XmlDocument CreateXml(string rootName)
        {
            XmlDocument xml = new XmlDocument();
            XmlDeclaration xd = xml.CreateXmlDeclaration("1.0", encoding, null);
            xml.AppendChild(xd);
            XmlElement node = xml.CreateElement(rootName);//"ReportWindows"
            xml.AppendChild(node);
            return xml;
        }

        private void CreateXml()
        {
            xmlDoc = new XmlDocument();
            XmlDeclaration xd = xmlDoc.CreateXmlDeclaration("1.0", encoding, null);
            xmlDoc.AppendChild(xd);
            XmlElement node = xmlDoc.CreateElement(this.m_rootName);//"ReportWindows"
            xmlDoc.AppendChild(node);
        }

        private void SetXmlDoc()
        {
            if (File.Exists(m_fileName))
            {
                if (xmlDoc == null)
                    xmlDoc = new XmlDocument();
                try
                {
                    xmlDoc.Load(m_fileName);
                }
                catch 
                {
                    CreateXml();
                }                
            }
            else
                CreateXml();
        }
        
        public void SaveAs(string fileName,Encoding encoding)
        {            
            System.Xml.XmlWriterSettings setting = new System.Xml.XmlWriterSettings();
            setting.Encoding = encoding;
            setting.Indent = true;
            setting.IndentChars = "   ";
            setting.NewLineChars = "\r\n";

            System.Xml.XmlWriter writer = System.Xml.XmlWriter.Create(fileName, setting);
            this.XmlDoc.WriteTo(writer);
            writer.Close();
        }

        public void Dispose()
        {
            if (xmlDoc != null) xmlDoc = null;
        }
        #endregion

        #region 获取节点文本
        public string GetNodeAttributeText(XmlNode node, string attributeName)
        {
            return GetNodeAttributeText(node, attributeName, string.Empty);
        }

        public string GetNodeAttributeText(XmlNode node, string attributeName, string defaultValue)
        {
            if(node == null) return string.Empty;
            string text = string.Empty;
            XmlNode nodeAtt = node.Attributes[attributeName];
            if (nodeAtt != null)
                text = nodeAtt.InnerText;
            else
                text = defaultValue;
            return text;
        }

        public string GetNodeInnerText(XmlNode node)
        {
            return GetNodeTextEx(node, string.Empty);
        }

        public string GetNodeText(XmlNode parentNode, string xPath)
        {
            return GetNodeInnerText(parentNode, xPath);
        }

        public string GetNodeTextEx(XmlNode node, string defaultValue)
        {
            string text = string.Empty;
            if (node != null)
                text = node.InnerText;
            else
                text = defaultValue;
            return text;
        }

        public string GetNodeInnerText(string xPath)
        {
            return GetNodeInnerText(xPath, string.Empty);
        }

        public string GetNodeInnerText(string xPath, string defaultValue)
        {
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            return GetNodeTextEx(node, defaultValue);
        }

        //public string GetNodeInnerText(string fileName, string xPath)
        //{
        //    XmlDocument xmlDoc = GetXmlDoc(fileName, false);
        //    return GetNodeInnerText(xmlDoc, xPath);
        //}

        public string GetNodeInnerText(XmlNode parentNode, string xPath)
        {
            if (parentNode == null) return string.Empty;
            if (xPath.StartsWith("//")) xPath = xPath.Substring(2);
            xPath = "child::" + xPath;
            XmlNode node = parentNode.SelectSingleNode(xPath);
            return GetNodeInnerText(node);
        }        

        public string GetNodeInnerXml(string xPath)
        {
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            string text = string.Empty;
            if (node != null)
                text = node.InnerXml;
            return text;
        }

        public string GetNodeOuterXml(string xPath)
        {
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            string text = string.Empty;
            if (node != null)
                text = node.OuterXml;
            return text;
        }
        /// <summary>
        /// 根据xPath获取子节点,没有子节点时，返回NULL
        /// </summary>
        /// <param name="xmlDoc">XmlDocument</param>
        /// <param name="xPath">要选择节点的xPath</param>
        /// <returns>存在节点则返回节点的XmlNodeList，否则返回NULL</returns>
        public XmlNodeList GetNodeList(string xPath)
        {
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            if (node != null)
                return node.ChildNodes;
            return null;
        }

        public XmlNodeList GetNodeList(XmlNode parentNode, string xPath)
        {
            XmlNode node = parentNode.SelectSingleNode(xPath);
            if (node != null)
                return node.ChildNodes;
            return null;
        }
        #endregion

        #region 创建节点
        public void CreateNode(XmlNode node)
        {
            this.XmlDoc.DocumentElement.AppendChild(node);
        }

        //public void CreateNodeWithNoAttribute(string fileName, string name, string text)
        //{
        //    XmlDocument xmlDoc = this.GetXmlDoc(fileName, false);
        //    CreateNodeWithNoAttribute(xmlDoc, name, text);
        //    SaveXmlDoc(xmlDoc,fileName);
        //}

        public XmlNode CreateNodeWithNoAttribute(string name, string text)
        {
            return CreateNodeWithNoAttribute(XmlDoc.DocumentElement, name, text);
        }

        /// <summary>
        /// 创建XML节点，如果已经存在，则只更新文本
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="name">要创建的节点名称</param>
        /// <param name="text">要创建的节点文本</param>
        public XmlNode CreateNodeWithNoAttribute(XmlNode parentNode, string name, string text)
        {
            XmlNode node = parentNode.SelectSingleNode("child::" + name);
            if (node == null)
            {
                node = XmlDoc.CreateElement(name);
                parentNode.AppendChild(node);
            }
            node.InnerText = text;
            return node;
        }

        public XmlNode CreateNodeWithNoAttribute(string parentName, string name, string text)
        {
            return CreateNodeWithNoAttribute(parentName, name, text, string.Empty);
        }

        public XmlNode CreateNodeWithNoAttribute(string parentName, string name, string text, string commentData)
        {
            if (parentName.StartsWith("//")) parentName = parentName.Substring(1);
            XmlNode parentNode = XmlDoc.DocumentElement;
            string xPath = "//" + parentName;
            parentNode = parentNode.SelectSingleNode(xPath);
            if (parentNode == null)
            {
                parentNode = XmlDoc.CreateElement(parentName);
                XmlDoc.DocumentElement.AppendChild(parentNode);
            }
            xPath = "child::" + name;
            XmlNode node = parentNode.SelectSingleNode(xPath);
            if (node != null)
            {
                node.InnerText = text;
                return node;
            }

            node = XmlDoc.CreateElement(name);
            node.InnerText = text;
            if (commentData.Length > 0)
            {
                XmlComment comment = XmlDoc.CreateComment(commentData);
                node.AppendChild(comment);
            }
            parentNode.AppendChild(node);
            return node;
        }

        public XmlNode CreateNodeWithAttribute(XmlNode parentNode, string name, string[] attributeNames, string[] attributeValues)
        {            
            return CreateNodeWithAttribute(parentNode, name, "@@~~!!**", attributeNames, attributeValues);
        }

        public XmlNode CreateNodeWithAttribute(string name, string text, string[] attributeNames, string[] attributeValues)
        {            
            XmlNode parentNode = XmlDoc.DocumentElement;
            return CreateNodeWithAttribute(parentNode, name, text, attributeNames, attributeValues);            
        }

        public XmlNode CreateNodeWithAttribute(string name, string text, string attributeName, string attributeValue)
        {
            XmlNode parentNode = XmlDoc.DocumentElement;
            return CreateNodeWithAttribute(parentNode, name, text, new string[] { attributeName }, new string[] { attributeValue });
        }

        public XmlNode CreateNodeWithAttribute(XmlNode parentNode, string name, string attributeName, string attributeValue)
        {
            return CreateNodeWithAttribute(parentNode, name, "@@~~!!**", new string[] { attributeName }, new string[] { attributeValue });
        }

        public XmlNode CreateNodeWithAttribute(XmlNode parentNode, string name, string text, string attributeName, string attributeValue)
        {
            return CreateNodeWithAttribute(parentNode, name, text, new string[] { attributeName }, new string[] { attributeValue });
        }

        public XmlNode CreateNodeWithAttribute(XmlNode parentNode, string name, string text, string[] attributeNames, string[] attributeValues)
        {
            if (parentNode == null) parentNode = XmlDoc.DocumentElement;
            string xPath ="child::"+ GetXpath(name, attributeNames, attributeValues).Substring(2);
            
            XmlNode node = parentNode.SelectSingleNode(xPath);
            if (node != null)
            {
                SetAttributes(node, attributeNames, attributeValues);
                return node;
            }

            node = XmlDoc.CreateElement(name);
            if(text!="@@~~!!**")
                node.InnerText = text;
            if (attributeNames != null)
            {
                int count = attributeNames.Length;
                for (int i = 0; i < count; i++)
                {
                    XmlAttribute xmlAtt = XmlDoc.CreateAttribute(attributeNames[i]);
                    xmlAtt.InnerText = (i < attributeValues.Length ? attributeValues[i] : string.Empty);
                    node.Attributes.Append(xmlAtt);
                }
            }
            parentNode.AppendChild(node);
            return node;
        }

        public void CreateXmlNode(XmlNodeAttributeCollection nodeList)
        {
            XmlNodeAttribute nodeAttribute;
            XmlNode parentNode = XmlDoc.DocumentElement;
            string xPath = string.Empty;
            string name = string.Empty;
            string value = string.Empty;
            string[] attributeName = null;
            string[] attributeValue = null;
            string commentData = string.Empty;
            XmlNode node = parentNode;
            for (int i = 0; i < nodeList.Count; i++)
            {
                nodeAttribute = nodeList.Item(i);
                if (node != parentNode)
                    node = parentNode;
                for (int z = 0; z < nodeAttribute.Names.Count; z++)
                {
                    commentData = string.Empty;
                    value = string.Empty;
                    attributeName = null;
                    attributeValue = null;
                    name = nodeAttribute.Names[z] + z.ToString();
                    if (nodeAttribute.Values != null && nodeAttribute.Values.ContainsKey(name))
                        value = nodeAttribute.Values[name];
                    if (nodeAttribute.AttributeNames != null && nodeAttribute.AttributeNames.ContainsKey(name))
                        attributeName = nodeAttribute.AttributeNames[name];
                    if (nodeAttribute.AttributeValuess != null && nodeAttribute.AttributeValuess.ContainsKey(name))
                        attributeValue = nodeAttribute.AttributeValuess[name];
                    if (nodeAttribute.CommentData != null && nodeAttribute.CommentData.ContainsKey(name))
                        commentData = nodeAttribute.CommentData[name];
                    CreaeXmlNodeWithXPath(ref parentNode, ref xPath, name.Substring(0, name.Length - z.ToString().Length), value, attributeName, attributeValue, commentData);
                    if (z < nodeAttribute.Names.Count - 1)
                        parentNode = node;
                }
            }
        }

        private void CreaeXmlNodeWithXPath(ref XmlNode parentNode, ref string xPath, string name, string value, string[] attributeNames, string[] attributeValues, string commentData)
        {
            xPath += GetXpath(name, attributeNames, attributeValues);
            XmlNode node = parentNode.SelectSingleNode(xPath);
            if (node == null)
            {   //如果不存在节点，则创建
                node = XmlDoc.CreateElement(name);
                parentNode.AppendChild(node);
            }
            //if (value.Length > 0)
            if (node.FirstChild == null || node.FirstChild.GetType() != typeof(XmlText))
                node.AppendChild(XmlDoc.CreateTextNode(value));
            else
                node.FirstChild.InnerText = value;
            parentNode = node;
            XmlAttribute xmlAtt;
            if (attributeNames != null && attributeValues != null)
            {
                for (int j = 0; j < attributeNames.Length; j++)
                {
                    xmlAtt = node.Attributes[attributeNames[j]];
                    if (xmlAtt == null)
                    {   //创建属性
                        xmlAtt = XmlDoc.CreateAttribute(attributeNames[j]);
                        node.Attributes.Append(xmlAtt);
                    }
                    xmlAtt.InnerText = (j < attributeValues.Length ? attributeValues[j] : "");
                }
            }
            else
                node.Attributes.RemoveAll();

            if (node.LastChild != null && node.LastChild.GetType() == typeof(XmlComment))
            {
                node = node.LastChild;
                node.InnerText = commentData;
            }
            else if (commentData != null && commentData.Length > 0)
                parentNode.AppendChild(XmlDoc.CreateComment(commentData));


            //xPath += GetXpath(nodeAttribute);
            //node = root.SelectSingleNode(xPath);
            //if (node == null)
            //{   //如果不存在节点，则创建
            //    node = XmlDoc.CreateElement(nodeAttribute.Name);
            //    parentNode.AppendChild(node);
            //}
            //parentNode = node;
            //node.InnerText = nodeAttribute.Value;

            //if (nodeAttribute.HasAttributes)
            //{
            //    for (int j = 0; j < nodeAttribute.AttributeNames.Length; j++)
            //    {
            //        xmlAtt = node.Attributes[nodeAttribute.AttributeNames[j]];
            //        if (xmlAtt == null)
            //        {   //创建属性
            //            xmlAtt = XmlDoc.CreateAttribute(nodeAttribute.AttributeNames[j]);
            //            node.Attributes.Append(xmlAtt);
            //        }
            //        xmlAtt.InnerText = (j < nodeAttribute.AttributeValuess.Length ? nodeAttribute.AttributeValuess[j] : "");
            //    }

            //}
            //else
            //    node.Attributes.RemoveAll();

            //if (node.LastChild.GetType() == typeof(XmlComment))
            //{
            //    node = node.LastChild;
            //    node.InnerText = nodeAttribute.CommentData;
            //}
            //else if (nodeAttribute.CommentData.Length > 0)
            //    parentNode.AppendChild(XmlDoc.CreateComment(nodeAttribute.CommentData)); 
        }

        #endregion

        #region 删除节点的各种方法

        //public void RemoveNode(string fileName, string xPath) {
        //    XmlDocument xmlDoc = this.GetXmlDoc(fileName, false);
        //    if (RemoveNode(xmlDoc, xPath))
        //        SaveXmlDoc(xmlDoc, fileName);
        //}

        public bool RemoveNode(string xPath)
        {
            bool success = false;
            if (XmlDoc.DocumentElement == null) return success;
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            if (node != null)
            {
                node.ParentNode.RemoveChild(node);
                success = true;
            }
            return success;
        }

        #endregion

        #region 获取XPATH的各种方法

        private string GetXpath(string name, string[] attributeNames, string[] attributeValues) {
            string xPath = "//" + name;
            if (attributeNames != null && attributeValues != null) {   //name[@aa="aa",@bb="bb"] 
                //xPath += "[";
                //for (int i = 0; i < attributeNames.Length; i++)
                //{
                //    xPath += "@"+attributeNames[i] + "=\"" + (i < attributeValues.Length ? attributeValues[i] : "") + "\",";
                //}
                //xPath = xPath.Substring(0,xPath.Length - 1) + "]";
                for (int i = 0; i < attributeNames.Length; i++) {
                    xPath += "[@" + attributeNames[i] + "=\"" + (i < attributeValues.Length ? attributeValues[i] : "") + "\"]";
                }
            }
            return xPath;
        }

        private string GetXpath(XmlNodeAttribute node) {
            //string xPath = "//" + node.Name;
            //if (node.HasAttributes)
            //{   //name[@aa="aa",@bb="bb"] 
            //    xPath += "[";
            //    for (int i = 0; i < node.AttributeNames.Length; i++)
            //    {
            //        xPath += "@" + node.AttributeNames[i] + "=\"" + (i < node.AttributeValuess.Length ? node.AttributeValuess[i] : "") + "\",";
            //    }
            //    xPath = xPath.Substring(0,xPath.Length - 1) + "]";
            //}
            //return xPath;
            return "";
        }

        #endregion

        /// <summary>
        /// 重置XML文档，只保留根节点
        /// </summary>
        public void Reset()
        {
            this.XmlDoc.DocumentElement.InnerXml = string.Empty;
        }

        public XmlNode GetXmlNode(string xPath)
        {
            return XmlDoc.DocumentElement.SelectSingleNode(xPath);
        }

        public XmlNode GetXmlNode(XmlNode parentNode, string xPath)
        {
            return parentNode.SelectSingleNode(xPath);
        }


        public void SetAttribute(string xPath, string attributeName, string attributeText)
        {
            XmlNode node = XmlDoc.DocumentElement.SelectSingleNode(xPath);
            if (node == null) return;
            SetAttribute(node, attributeName, attributeText);
        }

        public void SetAttribute(XmlNode node, string attributeName, string attributeText)
        {
            XmlAttribute xmlAtt = node.Attributes[attributeName];
            if (xmlAtt == null)
            {
                xmlAtt = XmlDoc.CreateAttribute(attributeName);
                node.Attributes.Append(xmlAtt);
            }
            xmlAtt.InnerText = attributeText;
        }

        public void SetAttributes(XmlNode node, string[] attributeNames, string[] attributeTexts)
        {
            int count = attributeNames.Length;
            for (int i = 0; i < count; i++)
            {
                XmlAttribute xmlAtt = node.Attributes[attributeNames[i]];
                if (xmlAtt != null && i < attributeTexts.Length)
                    xmlAtt.InnerText = attributeTexts[i];
            }
        }
        /// <summary>
        /// 反序列化Xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static T Deserialize<T>(string xmlString)
            where T : class,new()
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                using (StringReader reader = new StringReader(xmlString))
                {
                    return (T)ser.Deserialize(reader);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("XmlDeserialize发生异常：xmlString:" + xmlString + "异常信息：" + ex.Message);
            }

        }
        /// <summary>
        /// 序列化对象为字符串
        /// </summary> 
        public static string Serializer<T>(object obj)
        {
            using (var ms = new MemoryStream())
            {
                var xws = new XmlWriterSettings
                {
                    Indent = true,
                    OmitXmlDeclaration = true //,Encoding = Encoding.UTF8
                };
                using (var xtw = XmlWriter.Create(ms, xws))
                {
                    var ns = new XmlSerializerNamespaces(
                        new XmlQualifiedName[]
                        {
                            new XmlQualifiedName(string.Empty, "")
                        });
                    var xml = new XmlSerializer(typeof(T));
                    //序列化对象  
                    xml.Serialize(xtw, obj, ns);
                    ms.Position = 0;
                    var sr = new StreamReader(ms);
                    return sr.ReadToEnd();
                }
            }
        }
    }
}

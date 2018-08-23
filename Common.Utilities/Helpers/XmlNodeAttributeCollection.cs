using System.Collections.Generic;

namespace Base.Common.Xml
{
    /// <summary>
    /// Xml�ڵ�����б�
    /// </summary>
    public class XmlNodeAttributeCollection {
        List<XmlNodeAttribute> nodeList = new List<XmlNodeAttribute>();

        public int Count {
            get { return this.nodeList.Count; }
        }

        public XmlNodeAttribute Item(int index) {
            return this.nodeList[index];
        }

        public List<XmlNodeAttribute> List {
            get { return this.nodeList; }
        }

        public void Add(XmlNodeAttribute xmlNode) {
            this.nodeList.Add(xmlNode);
        }

        public void Add(List<string> names, Dictionary<string, string> values, Dictionary<string, string[]> attributeNames, Dictionary<string, string[]> attributeValues, Dictionary<string, string> commentDatas) {
            this.nodeList.Add(new XmlNodeAttribute(names, values, attributeNames, attributeValues, commentDatas));
        }

        public void Add(List<string> names, Dictionary<string, string> commentDatas) {
            this.nodeList.Add(new XmlNodeAttribute(names, commentDatas));
        }

        public void Clear() {
            this.nodeList.Clear();
        }

        public void Remove(XmlNodeAttribute xmlNode) {
            if (this.nodeList.Contains(xmlNode))
                this.nodeList.Remove(xmlNode);
        }
    }

    /// <summary>
    /// Xml�ڵ�����ƣ����ԣ�ֵ��˵���ķ�װ��
    /// </summary>
    public class XmlNodeAttribute {
        List<string> m_names = new List<string>();
        Dictionary<string, string> m_values = new Dictionary<string, string>();
        //string[] m_attributeNames;
        //string[] m_attributeValues;
        Dictionary<string, string[]> m_attributeNames = new Dictionary<string, string[]>();
        Dictionary<string, string[]> m_attributeValues = new Dictionary<string, string[]>();
        Dictionary<string, string> m_commentDatas = new Dictionary<string, string>();

        /// <summary>
        /// ����Xml�ڵ�
        /// </summary>
        /// <param name="names">�ڵ�����</param>
        /// <param name="values">�ڵ�ֵ</param>
        /// <param name="attributeNames">�ڵ�����</param>
        /// <param name="attributeValues">����ֵ</param>
        /// <param name="commentDatas">���˵��</param>
        public XmlNodeAttribute(List<string> names, Dictionary<string, string> values, Dictionary<string, string[]> attributeNames, Dictionary<string, string[]> attributeValues, Dictionary<string, string> commentDatas) {
            this.m_names = names;
            this.m_values = values;
            this.m_attributeNames = attributeNames;
            this.m_attributeValues = attributeValues;
            this.m_commentDatas = commentDatas;
        }

        public XmlNodeAttribute(List<string> names, Dictionary<string, string[]> attributeNames, Dictionary<string, string[]> attributeValues) {
            this.m_names = names;
            this.m_attributeNames = attributeNames;
            this.m_attributeValues = attributeValues;
        }

        public XmlNodeAttribute(List<string> names, Dictionary<string, string> values, Dictionary<string, string> commentDatas) {
            this.m_names = names;
            if (commentDatas != null)
                this.m_commentDatas = commentDatas;
            this.m_values = values;
        }

        public XmlNodeAttribute(List<string> names, Dictionary<string, string> commentDatas) {
            this.m_names = names;
            this.m_commentDatas = commentDatas;
        }

        public List<string> Names {
            get { return this.m_names; }
            set { this.m_names = value; }
        }

        public Dictionary<string, string> Values {
            get { return this.m_values; }
            set { this.m_values = value; }
        }

        public Dictionary<string, string[]> AttributeNames {
            get { return this.m_attributeNames; }
            set { this.m_attributeNames = value; }
        }

        public Dictionary<string, string[]> AttributeValuess {
            get { return this.m_attributeValues; }
            set { this.m_attributeValues = value; }
        }

        public Dictionary<string, string> CommentData {
            get { return this.m_commentDatas; }
            set { this.m_commentDatas = value; }
        }

        public bool HasValues {
            get {
                return (this.m_values != null && this.m_values.Count > 0);
            }
        }

        public bool HasCommentDatas {
            get {
                return (this.m_commentDatas != null && this.m_commentDatas.Count > 0);
            }
        }

        public bool HasAttributes {
            get {
                return (this.m_attributeNames != null && this.m_attributeValues != null && this.m_attributeNames.Count > 0
                && this.m_attributeValues.Count > 0);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1002파파고
{
    [Serializable]
    class Dictionary
    {
        #region 프로퍼티
        public string Title { get; private set; }
        public string Link { get; private set; }
        public string Description { get; private set; }
        public string Thumbnail { get; private set; }
        #endregion
        #region 메서드
        //사진 파서
        static public Dictionary MakeDictionary(XmlNode xn)
        {
            string title = string.Empty;
            string link = string.Empty;
            string description = string.Empty;
            string thumbnail = string.Empty;

            XmlNode title_node = xn.SelectSingleNode("title");
            title = ConvertString(title_node.InnerText);
            XmlNode link_node = xn.SelectSingleNode("link");
            link = ConvertString(link_node.InnerText);
            XmlNode description_node = xn.SelectSingleNode("description");
            description = ConvertString(description_node.InnerText);
            XmlNode thumbnail_node = xn.SelectSingleNode("thumbnail");
            thumbnail = ConvertString(thumbnail_node.InnerText);

            return new Dictionary(title, link, description, thumbnail);
        }
        //생성자
        public Dictionary(string title, string link, string description, string thumbnail)
        {
            Title = title;
            Link = link;
            Description = description;
            Thumbnail = thumbnail;
        }
        static private string ConvertString(string str)
        {
            return str;
        }
        #endregion
    }
}

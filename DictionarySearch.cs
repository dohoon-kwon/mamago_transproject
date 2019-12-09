using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace _1002파파고
{
    class DictionarySearch
    {
        #region 객체생성
        public List<Dictionary> list = new List<Dictionary>();
        #endregion
        #region 선언
        public string XmlString { get; private set; }
        XmlDocument doc;
        #endregion
        #region 메서드
        //xml파일 Document & 노드찾기
        public void SearchLanguage(string str)
        {
            list.Clear();

            XmlString = Find(str);
            doc = new XmlDocument();
            doc.LoadXml(XmlString);
            //doc.Save("DicResult.xml");
            //====================================
            XmlNode node = doc.SelectSingleNode("rss");
            XmlNode n = node.SelectSingleNode("channel");
            Dictionary dic = null;
            foreach (XmlNode el in n.SelectNodes("item"))
            {
                dic = Dictionary.MakeDictionary(el);
                list.Add(dic);
            }

        }
        //백과사전 연결
        public string Find(string str)
        {
            string uni = HttpUtility.UrlEncode(str);
            string url = "https://openapi.naver.com/v1/search/encyc.xml?query=" + uni;  // 결과가 XML 포맷
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "eQ1Du224I3tVP24ZdpAW"); // 클라이언트 아이디
            request.Headers.Add("X-Naver-Client-Secret", "13XzEXhr7V");       // 클라이언트 시크릿
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string status = response.StatusCode.ToString();
            if (status == "OK")
            {
                Stream stream = response.GetResponseStream();
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);
                string text = reader.ReadToEnd();
                return text;
            }
            else
            {
                return string.Format("Error 발생={0}", status);
            }
        }
        #endregion
    }
}

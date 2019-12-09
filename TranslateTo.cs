using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace _1002파파고
{
    class TranslateTo
    {
        #region 선언
        public string TransText;
        #endregion
        #region 프로퍼티
        public string ResultLanguage { get; private set; }
        public string Viewtext { get; private set; }
        public string Lang { get; private set; }
        XmlDocument doc;
        #endregion
        #region 메서드
        //파일변환
        public void TranslateLanguage(string str)
        {
            //파싱 JSON >> XML 변환
            string json = Find(str);
            JObject j = JObject.Parse(json);
            ResultLanguage = Trans(str, j["langCode"].ToString());
            doc = new XmlDocument();
            doc = JsonConvert.DeserializeXmlNode(ResultLanguage);
            //doc.Save("result.xml");
            //--------------------------------------------------------------
            //노드찾기
            XmlNode node = doc.SelectSingleNode("message");
            XmlNode node2 = node.SelectSingleNode("result");
            Translate t = Translate.MakeTranslate(node2);
            TransText = t.Result;
        }
        //언어감지
        public string Find(string str)
        {
            string url = "https://openapi.naver.com/v1/papago/detectLangs";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "eQ1Du224I3tVP24ZdpAW");
            request.Headers.Add("X-Naver-Client-Secret", "13XzEXhr7V");
            request.Method = "POST";
            //string query = str;
            byte[] byteDataParams = Encoding.UTF8.GetBytes("query=" + str);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            //Console.WriteLine(text);
            return text;
        }
        //언어번역
        public string Trans(string str,string t)
        {
            string url = "https://openapi.naver.com/v1/papago/n2mt";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", "9TA168rSdZxXhKCoNpVf");
            request.Headers.Add("X-Naver-Client-Secret", "8ndXb9ZqPD");
            request.Method = "POST";
            byte[] byteDataParams = Encoding.UTF8.GetBytes("source="+t+"&target="+Lang+"&text="+str);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            string text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();
            return text;
        }
        //콤보박스 언어값 설정
        public void ComboLanguage(string str)
        {
            if(str.Equals("한국어")==true || str.Equals("Korean")==true)
            {
                Lang = "ko";
            }
            else if (str.Equals("일본어") == true || str.Equals("Japanese") == true)
            {
                Lang = "ja";
            }
            else if (str.Equals("영어") == true || str.Equals("English") == true)
            {
                Lang = "en";
            }
            else if (str.Equals("독일어") == true || str.Equals("German") == true)
            {
                Lang = "de";
            }
            else if (str.Equals("중국어간체") == true || str.Equals("Chinese") == true)
            {
                Lang = "zh-CN";
            }
            else if (str.Equals("중국어번체") == true)
            {
                Lang = "zh-TW";
            }
            else if (str.Equals("스페인어") == true || str.Equals("Spanish") == true)
            {
                Lang = "es";
            }
            else if (str.Equals("프랑스어") == true || str.Equals("French") == true)
            {
                Lang = "fr";
            }
            else if (str.Equals("러시아어") == true || str.Equals("Russian") == true)
            {
                Lang = "ru";
            }
            else if (str.Equals("이탈리아어") == true || str.Equals("Italian") == true)
            {
                Lang = "it";
            }
        }
        #endregion
        #region 주의문!
        //한국어(ko)-영어(en), 한국어(ko)-일본어(ja), 한국어(ko)-중국어 간체(zh-CN),
        //한국어(ko)-중국어 번체(zh-TW), 한국어(ko)-스페인어(es), 한국어(ko)-프랑스어(fr),
        //한국어(ko)-러시아어(ru), 한국어(ko)-베트남어(vi), 한국어(ko)-태국어(th),
        //한국어(ko)-인도네시아어(id), 한국어(ko)-독일어(de), 한국어(ko)-이탈리아어(it),
        //중국어 간체(zh-CN) - 중국어 번체(zh-TW), 중국어 간체(zh-CN) - 일본어(ja),
        //중국어 번체(zh-TW) - 일본어(ja), 영어(en)-일본어(ja), 영어(en)-중국어 간체(zh-CN),
        //영어(en)-중국어 번체(zh-TW), 영어(en)-프랑스어(fr)를 지원합니다.
        #endregion
    }
}

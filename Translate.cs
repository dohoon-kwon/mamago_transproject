using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace _1002파파고
{
    class Translate
    {
        #region 프로퍼티
        public string Result { get; private set; }
        #endregion
        #region 파서메서드
        //파서
        static public Translate MakeTranslate(XmlNode xn)
        {
            string result = string.Empty;

            XmlNode n1 = xn.SelectSingleNode("srcLangType");
            XmlNode n2 = xn.SelectSingleNode("tarLangType");
            XmlNode result_node = xn.SelectSingleNode("translatedText");
            result = ConvertString(result_node.InnerText);

            return new Translate(result);
        }

        static string ConvertString(string str)
        {
            return str;
        }

        public Translate(string result)
        {
            Result = result;
        }
        #endregion
    }
}

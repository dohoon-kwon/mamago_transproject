using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace _1002파파고
{
    class wbList
    {
        #region 메서드
        //파일저장
        public static void filesersave(object[] arr, int size)
        {
            Stream ws = new FileStream("list.txt", FileMode.Create);
            BinaryFormatter serializer = new BinaryFormatter();

            int max = arr.Length;
            serializer.Serialize(ws, max);
            serializer.Serialize(ws, size);

            for (int i = 0; i < size; i++)
            {
                Dictionary dic = (Dictionary)arr[i];
                serializer.Serialize(ws, dic);
            }
            ws.Close();
        }
        //파일로드
        public static Dictionary[] fileserload(out int max)
        {
            Stream rs = new FileStream("list.txt", FileMode.Open);
            BinaryFormatter deserializer = new BinaryFormatter();

            max = (int)deserializer.Deserialize(rs);
            int size = (int)deserializer.Deserialize(rs);

            Dictionary[] list = new Dictionary[size];
            for (int i = 0; i < size; i++)
            {
                list[i] = (Dictionary)deserializer.Deserialize(rs);
            }
            rs.Close();
            return list;
        }
        #endregion
    }
}
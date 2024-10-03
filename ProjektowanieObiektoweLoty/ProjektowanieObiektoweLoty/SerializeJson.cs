using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public static class SerializeJson
    {
        private static string SerializeList<T>(List<T> ObjectList)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var Object in ObjectList)
            {
                sb.Append(Newtonsoft.Json.JsonConvert.SerializeObject(Object));
                sb.Append("\n");
            }
            return sb.ToString();
        }
        public static void Serialize<T>(List<T> ObjectList,string fileName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(SerializeList(ObjectList));
            string jsonString = sb.ToString();
            using (StreamWriter outputFile = new StreamWriter(fileName))
            {
                outputFile.Write(jsonString);
            }
        }
    }
    

}

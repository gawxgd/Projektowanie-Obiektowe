using Avalonia.Media.TextFormatting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.Tools
{
    public static class UpdateLog
    {
        private static List<string> updates = new List<string>();
        public static void AddUpdate(string name, ulong oldID, ulong newID)
        {
            updates.Add($"{name} updated ID from {oldID} to {newID}");  
        }
        
        public static void AddUpdate(string name, ulong ID, Single oLon, Single oLat, Single nLon, Single nLat,Single oAmsl, Single nAmsl)
        {
            updates.Add($"{name} {ID} updated longtitude from {oLon} to {nLon} lattitiude from {oLat} to {nLon} amsl from {oAmsl} to {nAmsl}");
        }
        
        public static void AddUpdate(ulong ID,string name, string oldPhone,string newPhone, string oldEmail,string newEmail)
        {
            updates.Add($"{name} {ID} updated Phone from {oldPhone} to {newPhone}, Email from {oldEmail} to {newEmail}");
        }
        public static void AddTakenIDLog(ulong ID)
        {
            updates.Add($"{ID} taken");
        }
        private static string CreateLogName()
        {
            var date = DateTime.Now;
            return $"UpdateLog_{date.Hour}_{date.Minute}_{date.Second}.txt";
        }
        public static void PrintToFile()
        {
            string FileName = CreateLogName();
            var sb = new StringBuilder();
            foreach(var log in updates) 
            {
                sb.Append(log);
                sb.Append('\n');    
            }
            using(StreamWriter logFile = new StreamWriter(FileName)) 
            {
                logFile.Write(sb.ToString());   
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NetworkSourceSimulator;

namespace ProjektowanieObiektoweLoty
{
    public static class ReadMessage
    {
        public static void ReadFromMessage(Message NewMessage)
        {
            byte[] byteTab = NewMessage.MessageBytes;
            char[] classID = Encoding.ASCII.GetChars(byteTab[0..3]);
            string classShorName = new string(classID);
            lock (ConsoleUser._serializeLock)
            {
                CreateFtrObject.CreateObjectFromDictionaryNetwork(classShorName, byteTab, CreateFactories.MatchClassNameWithFactoryDictNetwork);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty
{
    public static class ReadFromFile
    {
        public static void ReadFromFtrFile(string FilePathArg)
        {
            String line;
            try
            {
                StreamReader sr = new StreamReader(FilePathArg);
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] ObjectParameters = line.Split(',');
                    string ClassShortName = ObjectParameters[0];
                    try
                    {
                        CreateFtrObject.CreateObjectFromDictionaryFtr(ClassShortName, ObjectParameters,CreateFactories.MatchClassNameWithFactoryDict);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception: " + e.Message);
                    }
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
    }
}

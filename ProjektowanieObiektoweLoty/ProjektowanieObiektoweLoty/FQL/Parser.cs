using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.FQL
{
    public class Parser
    {
        public void ParseFQL(string userInput)
        {
            var spaceDelimited = userInput.Split(' ');
            switch (spaceDelimited[0].ToLower())
            {
                case "display":
                    var outputD = ParseDisplay(userInput);
                    string conditionsD;
                    if (outputD.conditions == null)
                        conditionsD = "";
                    else
                        conditionsD = outputD.conditions;
                    PerformQuery queryD = new PerformQuery();
                    queryD.Display(outputD.tables, outputD.source, conditionsD);
                    break;
                case "update":
                    var outputU = ParseUpdate(userInput);
                    string conditionsU;
                    if (outputU.conditions == null)
                        conditionsU = "";
                    else
                        conditionsU = outputU.conditions;
                    PerformQuery queryU = new PerformQuery();
                    queryU.Update(outputU.toSet, outputU.source, conditionsU);
                    break;
                case "delete":
                    var outputDel = ParseDelete(userInput);
                    string conditionsDel;
                    if (outputDel.conditions == null)
                        conditionsDel = "";
                    else
                        conditionsDel = outputDel.conditions;
                    PerformQuery queryDel = new PerformQuery();
                    queryDel.Delete(outputDel.Item1, outputDel.conditions);
                    break;
                case "add":
                    var outputA = ParseAdd(userInput);
                    PerformQuery queryAdd = new PerformQuery();
                    queryAdd.Add(outputA.toAdd, outputA.dest);
                    break;
            }
        }
        static string[] SplitCaseInsensitive(string input, string delimiter)
        {
            string pattern = Regex.Escape(delimiter);
            return Regex.Split(input, pattern, RegexOptions.IgnoreCase);
        }
        public (string[] tables, string source, string conditions) ParseDisplay(string display)
        {
            var displayQuery = display.Split(" ");
            var tab = SplitCaseInsensitive(display, "from");
            var table = tab[0];
            var tempQuery = tab[1];
            var splitedTempQuery = SplitCaseInsensitive(tempQuery, "where");
            var from = splitedTempQuery[0];
            string where = null;
            if (splitedTempQuery.Length > 1)
                where = splitedTempQuery[1];
            
            table = table.Replace("display", "");
            table = table.Trim();
            if (table is null)
                throw new Exception("not specifed query request output");
            if (from == "")
                throw new Exception("not specified query source");
            table = table.Replace(",", "");
            var tables = table.Split(" ");
            from = from.Replace(" ", "");
            if(where != null)
                where = where.Trim();
            foreach(var item in tables)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine(from);
            return (tables, from, where);
        }
        public ((string, string)[] toSet, string source, string conditions) ParseUpdate(string update)
        {
            var Query = update.Split(" ");
            var tab = SplitCaseInsensitive(update, "set");
            var table = tab[0];
            var tempQuery = tab[1];
            var splitedTempQuery = SplitCaseInsensitive(tempQuery, "where");
            var toSet = splitedTempQuery[0];
            string where = null;
            if (splitedTempQuery.Length > 1)
                where = splitedTempQuery[1];

            table = table.Replace("update", "");
            table = table.Trim();
        
            if (table is null)
                throw new Exception("not specifed query request output");
            if (toSet == "")
                throw new Exception("not specified query to set");
            List<(string, string)> resultList = new List<(string, string)>();

            string[] pairs = toSet.Split(',');

            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Trim().Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    resultList.Add((key, value));
                }
                else if (keyValue.Length == 1) // Handles the case when no '=' is found
                {
                    string value = keyValue[0].Trim();
                    resultList.Add(("NoKey", value));
                }
            }
            foreach(var item in resultList)
            {
                Console.WriteLine(item);
            }

            if (where != null)
                where = where.Trim();
            Console.WriteLine(table);
            Console.WriteLine(where);
            return (resultList.ToArray(), table, where);
        }
        public (string, string conditions) ParseDelete(string update)
        {
            var tab = SplitCaseInsensitive(update, "where");
            var toDelete = tab[0];
            string where = null;
            if (tab.Length > 1)
                where = tab[1];
            toDelete = toDelete.Replace("delete", "");
            toDelete = toDelete.Trim();

            if (toDelete is null)
                throw new Exception("not specifed query request output");
            if (where != null)
                where = where.Trim();
            return (toDelete, where);
        }
        public ((string, string)[] toAdd,string dest) ParseAdd(string update)
        {
            var tab = SplitCaseInsensitive(update, "new");
            var dest = tab[0];
            var toAdd = tab[1];
            dest = dest.Replace("add","");
            dest.Trim();
            if (dest == "")
                throw new Exception("not specifed query request output");
            if (toAdd == "")
                throw new Exception("not specified query to set");
            List<(string, string)> resultList = new List<(string, string)>();

            string[] pairs = toAdd.Split(',');

            foreach (var pair in pairs)
            {
                string[] keyValue = pair.Trim().Split('=');
                if (keyValue.Length == 2)
                {
                    string key = keyValue[0].Trim();
                    string value = keyValue[1].Trim();
                    resultList.Add((key, value));
                }
                else if (keyValue.Length == 1) // Handles the case when no '=' is found
                {
                    string value = keyValue[0].Trim();
                    resultList.Add(("NoKey", value));
                }
            }
            Console.WriteLine(dest);
            foreach (var item in resultList)
            {
                Console.WriteLine(item);
            }
            return (resultList.ToArray(), dest);
        }
        
    }
}

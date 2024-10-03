using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.FQL
{
    public class PerformQuery
    {
        public void Display(string[] tables, string sourceTable, string conditions)
        {
            try
            {
                var objectDict = CreateFtrObject.TypeNameToDict[sourceTable];
                List<List<string>> table = new List<List<string>>();
                foreach (var item in objectDict)
                {
                    bool result;
                    if (conditions == "")
                        result = true;
                    result = item.Value.EvaluateFQL(conditions);
                    if (result)
                    {
                        if (tables[0] == "*")
                        {
                            List<string> row = new List<string>();
                            foreach (var field in item.Value.GetAllFields())
                            {
                                row.Add(field);
                            }
                            table.Add(row);
                        }
                        else
                        {
                            List<string> row = new List<string>();
                            foreach (var field in tables)
                            {
                                row.Add(item.Value.GetField(field));
                            }
                            table.Add(row);
                        }
                    }
                }
                if (tables[0] == "*")
                {
                    tables = objectDict.First().Value.GetFieldNames();
                }
                foreach (var field in tables)
                {
                    Console.WriteLine(field);
                }

                DisplayQuery.PrintTable(tables.ToList(), table);

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public void Update((string key, string value)[] toUpdate, string dest, string conditions)
        {
            try
            {
                var objectDict = CreateFtrObject.TypeNameToDict[dest];

                foreach (var item in objectDict)
                {
                    bool result;
                    if (conditions == "")
                        result = true;
                    result = item.Value.EvaluateFQL(conditions);
                    if (result)
                    {
                        foreach (var keyValue in toUpdate)
                        {
                            item.Value.SetField(keyValue.value, keyValue.key);
                        }
                    }
                }


            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public void Delete(string toDelete, string conditions)
        {
            try
            {
                var objectDict = CreateFtrObject.TypeNameToDict[toDelete];

                foreach (var item in objectDict)
                {
                    bool result;
                    if (conditions == "")
                        result = true;
                    result = item.Value.EvaluateFQL(conditions);
                    if (result)
                    {
                        objectDict.TryRemove(item);
                    }
                }


            }
            catch (Exception e) { Console.WriteLine(e.Message); }
        }
        public void Add((string key, string value)[] toAdd, string dest)
        {
            dest = dest.Trim();

            var factory = CreateFactories.MatchClassNameWithFactoryDict[CreateFactories.MatchClassNameWithShortClassName[dest]];
            var createdObject = factory.Create();
            foreach (var item in toAdd)
            {
                createdObject.SetField(item.value, item.key);
            }
            createdObject.AddToList();
            CreateFtrObject.TypeNameToDict[dest].TryAdd(createdObject.ID, createdObject);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProjektowanieObiektoweLoty.FQL.CreateQuery;

namespace ProjektowanieObiektoweLoty.FQL
{
    public static class FilterTables
    {
        public delegate bool CheckCondition(IComparable a, IComparable b);
        public static readonly Dictionary<string, CheckCondition> EvalCondition = new Dictionary<string, CheckCondition>()
        {
            {"=",(a,b) => a.CompareTo(b) == 0 },
            {"!=",(a,b) => a.CompareTo(b) != 0 },
            {"<", (a,b) => a.CompareTo(b) < 0},
            {">",(a,b) => a.CompareTo(b) > 0 },
            {"<=",(a,b) => a.CompareTo(b) <= 0 },
            {">=",(a,b) => a.CompareTo(b) >= 0 }
        };
        public static bool EvaluateCondition<T>(string conditionString, T iftrObject, Dictionary<string, Get<T>> GetField) where T : IFtr
        {
            var operandList = ConditionParser.ParseToOperands(conditionString);
            Stack<bool> conditionStack = new Stack<bool>();
            int i = 0;
            while (i < operandList.Count)
            {
                (string a, string b,string operand, int temp ) = ConditionParser.GetExppresion(operandList, i);
                i = temp;
                if (a == null && b == null && operand == null)
                    continue;
                if (GetField.ContainsKey(a))
                {
                    string stringValueA = GetField[a].Invoke(iftrObject);
                    var aValue = FtrObjectGetFieldValue[a].Invoke(stringValueA);
                    var bValue = FtrObjectGetFieldValue[a].Invoke(b);
                    bool evaluation = EvalCondition[operand].Invoke(aValue, bValue);
                    conditionStack.Push(evaluation);
                }
                else if (GetField.ContainsKey(b))
                {
                    string stringValueB = GetField[b].Invoke(iftrObject);
                    var bValue = FtrObjectGetFieldValue[b].Invoke(stringValueB);
                    var aValue = FtrObjectGetFieldValue[b].Invoke(a);
                    bool evaluation = EvalCondition[operand].Invoke(aValue, bValue);
                    conditionStack.Push(evaluation);
                }
            }
            foreach (var item in operandList)
            {
                if (item.ToLower() == "and")
                {
                    var a = conditionStack.Pop();
                    var b = conditionStack.Pop();
                    conditionStack.Push(a && b);
                }
                else if (item.ToLower() == "or")
                {
                    var a = conditionStack.Pop();
                    var b = conditionStack.Pop();
                    conditionStack.Push(a || b);
                }
            }
            bool cond = true;
            foreach (var item in conditionStack)
            {
                cond = item && cond;
            }
            return cond;
        }
    }
}

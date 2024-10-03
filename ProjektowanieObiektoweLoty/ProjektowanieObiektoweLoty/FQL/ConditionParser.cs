using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.FQL
{
    public static class ConditionParser
    {
        private static string[] SplitCaseInsensitive(string input, string delimiter)
        {
            string pattern = string.Format(@"(?<=[\s=<>!]=?|!=)(?={0})", delimiter);
            string[] parts = Regex.Split(input, pattern, RegexOptions.IgnoreCase);

            // Filter out empty strings
            parts = parts.Where(part => !string.IsNullOrEmpty(part.Trim())).ToArray();

            return parts;
        }
        private static string[] SplitWithDelimiter(string input)
        {
            string pattern = @"(?<=[\s=<>!]=?|!=)";
            var parts = Regex.Split(input, pattern);
            parts = parts.Where(part => !string.IsNullOrEmpty(part.Trim())).ToArray();
            return parts;
        }
        public static List<string> ParseToOperands(string conditionString)
        {
            var condition = SplitCaseInsensitive(conditionString, "or|and");
            List<string> operandList = new List<string>();
            foreach (var expression in condition)
            {
                var exp = expression.Trim();
                var operands = SplitWithDelimiter(exp);
                foreach (var operand in operands)
                {
                    operandList.Add(operand.Trim());
                }
            }
            return operandList;
        }
        public static (string,string,string, int) GetExppresion(List<string> operandList, int j)
        {
            string a = "";
            string b = "";
            int i = j;
            if (operandList[i].ToLower() == "and" || operandList[i].ToLower() == "or")
            {
                i++;
                return (null, null,null, i);
            }
            a = operandList[i];
            if (a.Contains('['))
            {
                i++;
                StringBuilder sb = new StringBuilder();
                sb.Append(a.Replace("[", ""));
                sb.Append(" ");
                while (!operandList[i].Contains(']') && i < operandList.Count)
                {
                    sb.Append(operandList[i]);
                    sb.Append(" ");
                    i++;
                }
                sb.Append(operandList[i].Replace("]", ""));
                a = sb.ToString();
            }
            string operand = operandList[i + 1];
            string tempOperand = operand + operandList[i + 2];
            if (tempOperand == ">=" || tempOperand == "<=")
            {
                b = operandList[i + 3];
                operand = tempOperand;
                i += 4;
            }
            else
            {
                b = operandList[i + 2];
                i += 3;
            }
            if (b.Contains('['))
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(b.Replace("[", ""));
                sb.Append(" ");
                while (!operandList[i].Contains(']') && i < operandList.Count)
                {
                    sb.Append(operandList[i]);
                    sb.Append(" ");
                    i++;
                }
                sb.Append(operandList[i].Replace("]", ""));
                b = sb.ToString();
                i++;
            }
            return (a, b,operand, i);
        }
    }
}

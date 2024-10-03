using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektowanieObiektoweLoty.FQL
{
    public static class DisplayQuery
    {
        public static void PrintTable(List<string> headers, List<List<string>> rows)
        {
            // Calculate column widths
            var columnWidths = new int[headers.Count];
            for (int i = 0; i < headers.Count; i++)
            {
                columnWidths[i] = rows.Max(row => row[i].Length) > headers[i].Length
                    ? rows.Max(row => row[i].Length) + 2
                    : headers[i].Length + 2;
            }

            // Print header
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write(headers[i].PadRight(columnWidths[i]));
                if (i < headers.Count - 1)
                    Console.Write(" | "); // Add separator between columns
            }
            Console.WriteLine();

            // Print separator
            Console.WriteLine(new string('-', columnWidths.Sum()));

            // Print rows
            foreach (var row in rows)
            {
                for (int i = 0; i < row.Count; i++)
                {
                    var alignment = i == 0 ? PadDirection.Left : PadDirection.Right;
                    Console.Write(PadString(row[i], columnWidths[i], alignment));
                    if (i < row.Count - 1)
                        Console.Write(" | "); // Add separator between columns
                }
                Console.WriteLine();
            }
        }

        private static string PadString(string text, int length, PadDirection direction)
        {
            return direction == PadDirection.Left
                ? text.PadRight(length - 1) + " "
                : " " + text.PadLeft(length - 1);
        }

        private enum PadDirection
        {
            Left,
            Right
        }
    }
}





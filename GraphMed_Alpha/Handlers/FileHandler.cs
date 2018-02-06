using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers
{
    class FileHandler
    {
        /* ---PUBLICS--- */
        public static void ValidateCSV(string filepath)
        {
            if (filepath != null || File.Exists(filepath))
            {
                var allLines = File.ReadAllLines(filepath);
                var row = new string[0];

                for (int i = 0; i < allLines.Length; i++) // for each line 
                {
                    var tmp = "";
                    row = allLines[i].Split('\t'); // split the row at tabs
                    for (int j = 0; j < row.Length; j++) // for each cell in row
                    {
                        var val = row[j]; // value at cell j
                        if (val.Contains("\"")) // if the cell contains a quotation mark
                        {
                            var line = new StringBuilder(val);
                            var r = FindAllIndexesOf(val, "\"");
                            var tick = 0;
                            foreach (var a in FindAllIndexesOf(val, "\""))
                            {
                                line.Insert(a + tick, "\"");
                                tick++;
                            }
                            row[j] = "\"" + line.ToString() + "\"";
                        }
                        tmp += row[j] + "\t";
                    }
                    allLines[i] = tmp;
                }
                File.WriteAllLines(filepath, allLines);
            }
            else
                Console.WriteLine("File not found");
        }

        /* ---PRIVATES--- */
        private static int[] FindAllIndexesOf(string source, string match)
        {
            var indexes = new List<int>();
            int index = 0;

            while ((index = source.IndexOf(match, index)) != -1)
            {
                indexes.Add(index++);
            }
            return indexes.ToArray();
        }
    }
}


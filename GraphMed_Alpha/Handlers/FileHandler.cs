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
        public static void ValidateCSV(string filepath)
        {
            if (File.Exists(filepath))
            {
                string[] tmp1 = File.ReadAllLines(filepath);
                string[] tmp = new string[0];

                for (int j = 0; j < tmp1.Length; j++)
                {
                    string abc = "";
                    tmp = tmp1[j].Split('\t');
                    for (int i = 0; i < tmp.Length; i++)
                    {
                        var val = tmp[i];
                        if (val.Contains("\""))
                        {
                            var a = val.Insert(val.IndexOf("\""), "\"");
                            a = a.Insert(a.LastIndexOf("\""), "\"");
                            var line = "\"" + a + "\"";
                            tmp[i] = line;
                        }
                        abc += tmp[i] + "\t";
                    }
                    tmp1[j] = abc;
                }

                File.WriteAllLines(filepath, tmp1);
            }
        }
    }
}

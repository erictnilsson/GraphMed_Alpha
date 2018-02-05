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
                        if (val.Contains("\"") && val.IndexOf("\"") != val.LastIndexOf("\"")) // if there are multiple citation-marks
                        {
                            var a = val.Insert(val.IndexOf("\""), "\"");
                            a = a.Insert(a.LastIndexOf("\""), "\"");
                            var line = "\"" + a + "\"";
                            tmp[i] = line;

                        }
                        else if (val.Contains("\"") && val.IndexOf("\"") == val.LastIndexOf("\""))
                        {
                            var a = val.Insert(val.IndexOf("\""), "\"");
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

        public static void SplitCSV(string filepath, int no_files)
        {
            var file = filepath + ".txt";
            if (File.Exists(file))
            {
                var val = File.ReadAllLines(file);
                var index = val.Count();
                var step = index / no_files;
                var skip = 0;
                var j = step; 
                for (int i = 0; i < no_files; i++)
                {
                    skip = i * step;
                    File.WriteAllLines(filepath + i.ToString() + ".txt", val.Take(j).Skip(skip).ToArray());
                    j += step;
                }

                //File.WriteAllLines(filepath + i.ToString() + ".txt", val.Take(no).ToArray());
            }
        }
    }
}


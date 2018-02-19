using GraphMed_Alpha.Handlers.CypherHandler;
using System;
using System.Diagnostics;
using GraphMed_Alpha.DisplayHandler;
using GraphMed_Alpha.Model;
using GraphMed_Alpha.Handlers;
using System.Configuration;

namespace GraphMed_Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            /* <Diagnostics purposes> */
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var uri = ConfigurationManager.AppSettings["relationship_snapshot"];
            /* <Testing purposes> */
            FileHandler.WriteToFile(FileHandler.SplitCSV("C:/Users/Eric Nilsson/Documents/Neo4j/default.graphdb/import/parsedRelationship-IS_A.txt")); 



            /* <Diagnostics purposes> */
            stopwatch.Stop();
            System.Diagnostics.Process.Start("http://127.0.0.1:7474/browser/");
            Console.WriteLine("Process completed in " + stopwatch.ElapsedMilliseconds + "ms");

        }
    }
}

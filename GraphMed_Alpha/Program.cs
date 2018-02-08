using GraphMed_Alpha.Handlers.CypherHandler;
using System;
using System.Diagnostics;
using GraphMed_Alpha.DisplayHandler;
using GraphMed_Alpha.Model;

namespace GraphMed_Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            /* <Diagnostics purposes> */
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            /* <Testing purposes> */
            var a = CypherHandler.Match(null).ByTerm("Duckbill flathead"); 
           //var a = CypherHandler.Match(null).DoStuff<Description, Concept>(searchTerm: "Duckbill flathead", searchBy: "Term", relationship: "refers_to"); 
            foreach (var tmp in a)
            {
                tmp.Print();
                Console.Write("\n"); 
            }
                
            /* <Diagnostics purposes> */
            stopwatch.Stop();
            //System.Diagnostics.Process.Start("http://127.0.0.1:7474/browser/");
            Console.WriteLine("Process completed in " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadLine(); 
        }
    }
}

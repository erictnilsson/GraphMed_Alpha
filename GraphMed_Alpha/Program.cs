using GraphMed_Alpha.Handlers.CypherHandler;
using System;
using System.Diagnostics;
using GraphMed_Alpha.DisplayHandler;
using GraphMed_Alpha.Model;
using GraphMed_Alpha.Handlers;

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
            Console.WriteLine("Search: ");
            string search = Console.ReadLine();

            var a = CypherHandler.Match(null).ByTerm(true, search);
            foreach (var i in a)
            {
                i.Print();
                Console.WriteLine(); 
            }
            Console.Read();
           

            /* <Diagnostics purposes> */
            stopwatch.Stop();
            //System.Diagnostics.Process.Start("http://127.0.0.1:7474/browser/");
            Console.WriteLine("Process completed in " + stopwatch.ElapsedMilliseconds + "ms");

        }
    }
}

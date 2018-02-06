using GraphMed_Alpha.Handlers;
using GraphMed_Alpha.Handlers.CypherHandler;
using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha
{
    class Program
    {
        static void Main(string[] args)
        {
            // Diagnostics purposes
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            // Testing

            // Diagnostics purposes
            stopwatch.Stop();
            System.Diagnostics.Process.Start("http://127.0.0.1:7474/browser/");
            Console.WriteLine("Process completed in " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadLine(); 
        }
    }
}

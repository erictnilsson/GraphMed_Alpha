using GraphMed_Alpha.Handlers;
using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
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
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var c = new Concept(id: "01", effectiveTime: "02", active: "03", moduleId: "04", definitionStatusId: "05");
            var d = new Description(id: "001", effectiveTime: "002", active: "003", moduleId: "004", conceptId: "01", languageCode: "006", typeId: "007", term: "008", caseSignificanceId: "009");
            var d1 = new Description(id: "010", effectiveTime: "020", active: "030", moduleId: "040", conceptId: "01", languageCode: "060", typeId: "070", term: "080", caseSignificanceId: "090");
            var d2 = new Description(id: "100", effectiveTime: "200", active: "300", moduleId: "400", conceptId: "00", languageCode: "600", typeId: "700", term: "800", caseSignificanceId: "900");

            Cypher.LoadConcepts();
            Cypher.LoadDescriptions(demandAnchorNode: false);

            Cypher.DeleteEverything();
            stopwatch.Stop();
            System.Diagnostics.Process.Start("http://127.0.0.1:7474/browser/");
            Console.WriteLine("Process completed in " + stopwatch.ElapsedMilliseconds + "ms");
            Console.ReadLine();
        }
    }
}

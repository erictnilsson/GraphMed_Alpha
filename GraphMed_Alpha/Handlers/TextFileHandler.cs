using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers
{
    public static class TextFileHandler
    {
        //string typeId = "";


        public static void RelationShipSplit(string filepath)
        {
            StreamReader fileI = new StreamReader(filepath);
            string line;
            string relationshipTerm = "";
            List<Relationship> relationships = new List<Relationship>();
            Dictionary<string, string> terms = new Dictionary<string, string>();


            //Count nbr of rows
            int counter = File.ReadAllLines(filepath).Length;
            //Exclude headers by reading extracting the first line
            string headers = fileI.ReadLine();
            //Loop through file
            for (int i = 0; i<= counter; i++)
            {
                //Get all values in columns
                while ((line = fileI.ReadLine()) != null)
                {
                    string[] values = line.Split('\t');
                    //Add rows into object relationship
                    relationships.Add(new Relationship(values[0], values[1], values[2], values[3], values[4], values[5], values[6], values[7], values[8], values[9]));
                }
            }
            //First row getTerm avoid outOfRangeException -- ugly as fuck
            terms.Add(relationships[0].TypeId, GetTerm(relationships[0].TypeId as string).ToUpper());
            relationshipTerm = terms[relationships[0].TypeId];
            //Cypher


            //loop through Relationship objects
            for (int i = 1; i < relationships.Count; i++)
            {
                //If Relationship typeId == Previous typeId
                if (relationships[i].TypeId.Equals(relationships[i - 1].TypeId))
                {
                    relationshipTerm = terms[relationships[i].TypeId];
                    //Cypher with relationshipTerm
                }
                //If not
                else
                {
                    //Check dictionary if exists run cypher
                    if (terms.ContainsKey(relationships[i].TypeId))
                    {
                        relationshipTerm = terms[relationships[i].TypeId];
                        //Cypher with relationTerm
                    }
                    //else add to dictionary run cypher
                    else
                    {
                        terms.Add(relationships[i].TypeId, GetTerm(relationships[i].TypeId as string).ToUpper());
                    }

                }

                //Console.WriteLine(objects[i].typeId);
            }


            Console.Read();
            fileI.Close();



            //MATCH (c:Concept),(cc:Concept)
            //WHERE c.Id = "100000000" AND cc.Id = "102272007"
            //CREATE(c) -[r: IS_A { id: "100022	", effectiveTime: "20090731", active: "0", moduleId: "900000000000207008",sourceId: "100000000", destinationId: "102272007", relationshipGroup: "0",typeId: "116680003",characteristicTypeId: "900000000000011006", modifierId: "900000000000451002"}]->(cc)



        }

        public static string GetTerm(string conceptId)
        {
            string termOutput = "";
            using (var client = new ConnectionHandler().Connect())
            {
                var term = client.Cypher.Match("(c:Concept)<-[:REFERS_TO]-(d:Description)")
                        .Where("d.TypeId = '900000000000013009'")
                        .AndWhere("c.Id = '" + conceptId + "'")
                        .Return(d => d.As<Description>())
                        .Results;
                termOutput = term.First().Term as string;
                //Cypher -- MATCH(c: Concept { Id: "116680003"}) - [:REFERS_TO] - (d:Description) WHERE d.TypeId = "900000000000013009" RETURN d.Term -  
            }
            return termOutput;
        }

        //Not used atm, only if we wanna go pro
        private static T GetPrevious<T>(IEnumerable<T> list, T current)
        {
            try
            {
                return list.TakeWhile(x => !x.Equals(current)).Last();
            }
            catch
            {
                return default(T);
            }
        }
    }
}
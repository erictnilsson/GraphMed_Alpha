using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandler
{
    class LoadCypher
    {
        /*---PUBLICS---*/
        public void Descriptions(bool forceConceptRelation)
        {
            var uri = ConfigurationManager.AppSettings["description_snapshot_deluxe"];
            var descriptId = "conceptId";
            var conceptId = "Id";
            var relationship = "refers_to";

            if (forceConceptRelation)
                BulkLoadCSVWithRelations(uri: uri, targetNode: new Description(), anchorNode: new Concept().GetType(), parentId: conceptId, childId: descriptId, relationship: relationship);

            else
                BulkLoadCSV(uri, new Description());

            IndexDescription();
        }

        public void Concepts()
        {
            var uri = ConfigurationManager.AppSettings["concept_snapshot_deluxe"];

            BulkLoadCSV(uri, new Concept());
            IndexConcept();
        }



        /*---PRIVATES---*/
        private static void BulkLoadCSV(string uri, Node targetNode)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                try
                {
                    client.Cypher
                   .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: 200)
                   .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})")
                   .ExecuteWithoutResults();
                }
                catch (TaskCanceledException te)
                {
                    Console.WriteLine("Stacktrace:" + te.StackTrace);
                    Console.WriteLine("Source: " + te.Source);
                    Console.WriteLine("Message:" + te.Message);
                    Console.WriteLine("CancellationToken: " + te.CancellationToken);
                    Console.WriteLine("Data:" + te.Data);

                    throw;
                }
            }
        }

        private static void BulkLoadCSVWithRelations(string uri, Node targetNode, Type anchorNode, string parentId, string childId, string relationship)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher
                      .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: 200)
                      .Match("(parent: " + anchorNode.Name + ")")
                      .Where("parent." + parentId + " = csvLine." + childId)
                      .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})-[:" + relationship.ToUpper() + "]->(parent)")
                      .ExecuteWithoutResults();
            }
        }

        private static string GetBuildString(Node target)
        {
            var targetLine = "";
            var targetLength = target.GetType().GetProperties().Length;

            for (int i = 0; i < targetLength; i++)
            {
                var propName = target.GetType().GetProperties()[i].Name;

                var propLine = propName + ": csvLine." + propName.First().ToString().ToLower() + propName.Substring(1);
                if (i != targetLength - 1)
                    propLine += ", ";

                targetLine += propLine;
            }
            return targetLine;
        }

        private void IndexConcept()
        {
            CreateIndex(new Concept(), "id");
        }

        private void IndexDescription()
        {
            CreateIndex(new Description(), "conceptId");
        }

        private void CreateIndex(Node target, string index_on)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.Create("INDEX ON: " + target.GetType().Name + "(" + index_on + ")")
                             .ExecuteWithoutResults();
            }
        }
    }
}

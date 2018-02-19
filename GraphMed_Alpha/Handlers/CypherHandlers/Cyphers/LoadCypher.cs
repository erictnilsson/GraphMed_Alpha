using GraphMed_Alpha.Handlers.CypherHandler.Cyphers;
using GraphMed_Alpha.Handlers.CypherHandlers.Cyphers;
using GraphMed_Alpha.Model;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandlers
{
    class LoadCypher : Cypher
    {
        private int? CommitSize { get; set; }

        public LoadCypher(int? limit, int? commitSize) : base(limit)
        {
            this.CommitSize = commitSize;
        }

        /*---PUBLICS---*/
        public void Descriptions(bool forceConceptRelation)
        {
            var uri = ConfigurationManager.AppSettings["description_snapshot_deluxe"];
            var relationship = "refers_to";

            if (forceConceptRelation)
                BulkLoadCSVWithRelations<Description, Concept>(uri: uri, targetNode: new Description(), relationship: relationship, anchorNode: new Concept());

            else
                BulkLoadCSV<Description>(uri, new Description());

            Console.WriteLine("Waldo successfully loaded the Descriptions!");

            IndexDescription();
            SetConstraintOnDescription();
        }

        public void Concepts()
        {
            var uri = ConfigurationManager.AppSettings["concept_snapshot_deluxe"];

            BulkLoadCSV<Concept>(uri, new Concept());
            Console.WriteLine("Waldo successfully loaded the Concepts!");
            //IndexConcept();
            SetConstraintOnConcept();
        }

        public void Relationships()
        {
            List<string> uris = new List<string>();
            foreach (var u in uris)
                BulkLoadRelations<Concept>(u); 
        }

        /*---PRIVATES---*/
        private void BulkLoadCSV<T>(string uri, Node targetNode)
        {
            try
            {
                Client.Cypher
                      .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: CommitSize)
                      .With("csvLine")
                      .Limit(Limit)
                      .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString<T>() + "})")
                      .ExecuteWithoutResults();
            }
            catch (NeoException)
            {
                throw;
            }
            finally
            {
                Client.Dispose();
            }
        }

        private void BulkLoadRelations<T1>(string fileUri)
        {
            var nodeType = typeof(T1).Name;
            var relationship = fileUri.Substring(fileUri.IndexOf('-') + 1, fileUri.IndexOf('.') - fileUri.IndexOf('-') - 1).ToUpper();
            try
            {
                Client.Cypher
                      .LoadCsv(new Uri(fileUri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: CommitSize)
                      .With("csvLine")
                      .Limit(Limit)
                      .Match("(c:" + nodeType + ")", "(cc:" + nodeType + ")")
                      .Where("c.Id = csvLine.sourceId")
                      .AndWhere("cc.Id = csvLine.destinationId")
                      .Create("(c)-[:" + relationship + " {" + GetBuildString<Model.Relationship>() + "} ]->(cc)")
                      .ExecuteWithoutResults();
            }
            catch (NeoException)
            {
                throw;
            }
            finally
            {
                Client.Dispose();
            }
        }

        private void BulkLoadCSVWithRelations<T1, T2>(string uri, Node targetNode, string relationship, Node anchorNode)
        {
            string anchorType = anchorNode.GetType().Name;
            string anchorLinkProp = anchorNode.LinkProp;

            string targetType = targetNode.GetType().Name;
            string targetLinkProp = targetNode.LinkProp.First().ToString().ToLower() + targetNode.LinkProp.Substring(1);

            try
            {
                Client.Cypher
                  .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: CommitSize)
                  .With("csvLine")
                  .Limit(Limit)
                  .Match("(anchor: " + anchorType + ")")
                  .Where("anchor." + anchorLinkProp + " = csvLine." + targetLinkProp)
                  .Create("(target: " + targetType + " {" + GetBuildString<T1>() + "})-[:" + relationship.ToUpper() + "]->(anchor)")
                  .ExecuteWithoutResults();
            }
            catch (NeoException)
            {
                throw;
            }
            finally
            {
                Client.Dispose();
            }
        }

        private void SetConstraintOnConcept()
        {
            SetConstraint(new Concept(), "Id");
            Console.WriteLine("Waldo successfully constrained Concept at Id!");
        }

        private void SetConstraintOnDescription()
        {
            SetConstraint(new Description(), "Id");
            Console.WriteLine("Waldo successfully constrained Description at Id!");
        }

        private void SetConstraint(Node target, string constraint_on)
        {
            try
            {
                Client.Cypher.CreateUniqueConstraint("n:" + target.GetType().Name, "n." + constraint_on)
                         .ExecuteWithoutResults();
            }
            catch (NeoException)
            {
                throw;
            }
            finally
            {
                Client.Dispose();
            }
        }

        private void IndexConcept()
        {
            CreateIndex(new Concept(), "Id");
        }

        private void IndexDescription()
        {
            CreateIndex(new Description(), "ConceptId");
            Console.WriteLine("Waldo successfully indexed the descriptions at ConceptId!");
        }

        private void CreateIndex(Node target, string index_on)
        {
            try
            {
                Client.Cypher.Create("INDEX ON: " + target.GetType().Name + "(" + index_on + ")")
                         .ExecuteWithoutResults();
            }
            catch (NeoException)
            {
                throw;
            }
            finally
            {
                Client.Dispose();
            }
        }

        private static string GetBuildString<T>()
        {
            var targetLine = "";
            var target = typeof(T);
            var targetLength = target.GetProperties().Length;

            for (int i = 0; i < targetLength; i++)
            {
                var propName = target.GetProperties()[i].Name;
                var propLine = "";

                if (!propName.Equals("LinkProp"))
                {
                    propLine = propName + ": csvLine." + propName.First().ToString().ToLower() + propName.Substring(1);
                    if (i != targetLength - 2)
                        propLine += ", ";

                    targetLine += propLine;
                }
            }
            return targetLine;
        }
    }
}

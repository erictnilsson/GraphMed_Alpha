using GraphMed_Alpha.Handlers.CypherHandler.Cyphers;
using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandler
{
    class LoadCypher : Cypher
    {
        private int? CommitSize { get; set; }

        public LoadCypher(int? limit, int? commitSize)
        {
            this.Limit = limit;
            this.CommitSize = commitSize;
        }

        /*---PUBLICS---*/
        public void Descriptions(bool forceConceptRelation)
        {
            var uri = ConfigurationManager.AppSettings["description_snapshot_deluxe"];
            var relationship = "refers_to";

            if (forceConceptRelation)
                BulkLoadCSVWithRelations(uri: uri, targetNode: new Description(), relationship: relationship, anchorNode: new Concept());

            else
                BulkLoadCSV(uri, new Description());

            Console.WriteLine("Waldo successfully loaded the Descriptions!");
            IndexDescription();
            SetConstraintOnDescription();
        }

        public void Concepts()
        {
            var uri = ConfigurationManager.AppSettings["concept_snapshot_deluxe"];

            BulkLoadCSV(uri, new Concept());
            Console.WriteLine("Waldo successfully loaded the Concepts!");
            //IndexConcept();
            SetConstraintOnConcept();
        }

        /*---PRIVATES---*/
        private void BulkLoadCSV(string uri, Node targetNode)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                try
                {
                    client.Cypher
                          .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: CommitSize)
                          .With("csvLine")
                          .Limit(Limit)
                          .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})")
                          .ExecuteWithoutResults();
                }
                catch (Neo4jClient.NeoException)
                {
                    throw;
                }

            }
        }

        private void BulkLoadCSVWithRelations(string uri, Node targetNode, string relationship, Node anchorNode)
        {
            string anchorType = anchorNode.GetType().Name;
            string anchorLinkProp = anchorNode.LinkProp;
            string targetType = targetNode.GetType().Name;
            string targetLinkProp = targetNode.LinkProp.First().ToString().ToLower() + targetNode.LinkProp.Substring(1); 

            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher
                      .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: CommitSize)
                      .With("csvLine")
                      .Limit(Limit)
                      .Match("(anchor: " + anchorType + ")")
                      .Where("anchor." + anchorLinkProp + " = csvLine." + targetLinkProp)
                      .Create("(target: " + targetType + " {" + GetBuildString(targetNode) + "})-[:" + relationship.ToUpper() + "]->(anchor)")
                      .ExecuteWithoutResults();
            }
        }

        private string GetBuildString(Node target)
        {
            var targetLine = "";
            var targetLength = target.GetType().GetProperties().Length;

            for (int i = 0; i < targetLength; i++)
            {
                var propName = target.GetType().GetProperties()[i].Name;
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
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.CreateUniqueConstraint("n:" + target.GetType().Name, "n." + constraint_on)
                             .ExecuteWithoutResults();
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
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.Create("INDEX ON: " + target.GetType().Name + "(" + index_on + ")")
                             .ExecuteWithoutResults();
            }
        }
    }
}

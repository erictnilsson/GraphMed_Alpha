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
        int? limit { get; set; }
        int? commitSize { get; set; }

        public LoadCypher(int? limit, int? commitSize)
        {
            this.limit = limit;
            this.commitSize = commitSize;
        }

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
                client.Cypher
               .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: commitSize)
               .With("csvLine")
               .Limit(limit)
               .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})")
               .ExecuteWithoutResults();
            }
        }

        private void BulkLoadCSVWithRelations(string uri, Node targetNode, Type anchorNode, string parentId, string childId, string relationship)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher
                      .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: commitSize)
                      .With("csvLine")
                      .Limit(limit)
                      .Match("(parent: " + anchorNode.Name + ")")
                      .Where("parent." + parentId + " = csvLine." + childId)
                      .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})-[:" + relationship.ToUpper() + "]->(parent)")
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

                var propLine = propName + ": csvLine." + propName.First().ToString().ToLower() + propName.Substring(1);
                if (i != targetLength - 1)
                    propLine += ", ";

                targetLine += propLine;
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

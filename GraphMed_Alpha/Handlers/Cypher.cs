using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers
{
    class Cypher
    {
        public static void CreateDescription(Description description, bool unique)
        {
            if (description != null)
                if (unique)
                    CreateUniqueNode(description, description.Id);
                else
                    CreateNode(description);
        }

        public static void CreateConcept(Concept concept, bool unique)
        {
            if (concept != null)
                if (unique)
                    CreateUniqueNode(concept, concept.Id);
                else
                    CreateNode(concept);
        }

        private static void CreateNode(Node newNode)
        {
            using (var client = new ConnectionHandler().Connect())
            {

                var nodeType = newNode.GetType().Name;
                client.Cypher.Create("(node:" + nodeType + " {newNode})")
                             .WithParam("newNode", newNode)
                             .ExecuteWithoutResults();
            }
        }

        private static void CreateUniqueNode(Node newNode, string merge_on)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                var nodeType = newNode.GetType().Name;
                client.Cypher.Merge("(node:" + nodeType + " {Id:{id} })")
                             .OnCreate().Set("node = {newNode}")
                             .WithParams(new
                             {
                                 id = merge_on,
                                 newNode
                             })
                             .ExecuteWithoutResults();
            }
        }

        public static void DescribeConcept(Concept InConcept, string relation)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.Match("(concept:Concept)", "(description:Description)")
                             .Where((Description description) => description.ConceptId == InConcept.Id)
                             .Create("(description)-[:" + relation.ToUpper() + "]->(concept)")
                             .ExecuteWithoutResults();
            }
        }

        public static void MatchConceptDescription(Concept concept, Description description)
        {
            if (concept != null || description != null)
            {
                var descriptionId = "ConceptId";
                var conceptId = "Id";
                var relation = "refers_to";
                Match(parent: description, child: concept, parentId: descriptionId, childId: conceptId, relation: relation);
            }
        }

        private static void Match(Node parent, Node child, string parentId, string childId, string relation)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                var pNode = "(parent: " + parent.GetType().Name + ")";
                var cNode = "(child: " + child.GetType().Name + ")";

                client.Cypher.Match(pNode, cNode)
                             .Where("parent." + parentId + " = child." + childId)
                             .Create("(parent)-[:" + relation.ToUpper() + "]->(child)")
                             .ExecuteWithoutResults();
            }
        }

        public static void CreateDescriptionWithConcept(Description description)
        {
            var descriptionId = "ConceptId";
            var conceptId = "Id";
            var relation = "refers_to";
            CreateRelationalNode(new Concept().GetType(), description, conceptId, descriptionId, relation);
        }

        private static void CreateRelationalNode(Type parentType, Node childNode, string parentId, string childId, string relation)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                var pNode = parentType.Name;
                var cNode = childNode.GetType().Name;
                var a = childNode.GetField(childId).GetValue(childNode);

                client.Cypher.Match("(parent: " + pNode + ")")
                             .Where("parent." + parentId + " = '" + childNode.GetField(childId).GetValue(childNode) + "'")
                             .Create("(child: " + cNode + " {childNode})-[:" + relation.ToUpper() + "]->(parent)")
                             .WithParam("childNode", childNode)
                             .ExecuteWithoutResults();
            }
        }

        private static string GetBuildString(Node target)
        {
            string targetLine = "";
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

        public static void LoadDescriptions(bool demandAnchorNode)
        {
            string uri = ConfigurationManager.AppSettings["description_snapshot_deluxe"];
            var descriptId = "conceptId";
            var conceptId = "Id";
            var relationship = "refers_to";

            BulkLoadCSVWithRelations(uri: uri, targetNode: new Description(), anchorNode: new Concept().GetType(), parentId: conceptId, childId: descriptId, relationship: relationship, forced: demandAnchorNode);
        }

        public static void LoadConcepts()
        {
            string uri = ConfigurationManager.AppSettings["concept_snapshot_deluxe"];
            BulkLoadCSV(uri, new Concept());
        }

        private static void BulkLoadCSV(string uri, Node targetNode)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher
                    .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: 200)
                    .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})")
                    .ExecuteWithoutResults();
            }
        }

        private static void BulkLoadCSVWithRelations(string uri, Node targetNode, Type anchorNode, string parentId, string childId, string relationship, bool forced)
        {
            using (var client = new ConnectionHandler().Connect())
            {
                if (forced)
                    client.Cypher
                          .LoadCsv(new Uri(uri), "csvLine", withHeaders: true, fieldTerminator: "\t", periodicCommit: 200)
                          .Match("(parent: " + anchorNode.Name + ")")
                          .Where("parent." + parentId + " = csvLine." + childId)
                          .Create("(n: " + targetNode.GetType().Name + " {" + GetBuildString(targetNode) + "})-[:" + relationship.ToUpper() + "]->(parent)")
                          .ExecuteWithoutResults();
            }
        }

        public static void DeleteEverything()
        {
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.Match("n").DetachDelete("n");
            }
        }
    }
}

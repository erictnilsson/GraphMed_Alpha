using GraphMed_Alpha.Handlers.CypherHandler.Cyphers;
using GraphMed_Alpha.Model;
using System;

namespace GraphMed_Alpha.Handlers.CypherHandler
{
    public class CreateCypher : Cypher
    {

        /*---PUBLICS---*/
        public void Relationships(Concept concept, Description description)
        {
            var anchorId = "ConceptId";
            var relationship = "refers_to";
            var targetAnchorId = "Id"; 

            CreateRelationships(anchor: description, anchorId: anchorId, relationship: relationship, target: concept, targetAnchorId: targetAnchorId);
        }

       /*---PRIVATES---*/
        private void CreateRelationships (Node anchor, string anchorId, string relationship, Node target, string targetAnchorId)
        {
            /*
             * MATCH (a:Anchor), (t:Target) 
             * WHERE (a.Id = t.AnchorId)
             * CREATE (a)-[:RELATIONSHIP]->(t)
             */
            var tick = 0; 
            using (var client = new ConnectionHandler().Connect())
            {
                client.Cypher.Match("(a:" + anchor.GetType().Name + ")", "(t:" + target.GetType().Name + ")")
                             .Where("a." + anchorId + " = " + "t." + targetAnchorId)
                             .Create("(a)-[:"+relationship.ToUpper()+"]->(t)")
                             .ExecuteWithoutResults();

                tick++; 
                Console.WriteLine("Created " + tick + " Relation"); 
            }
        }
    }
}
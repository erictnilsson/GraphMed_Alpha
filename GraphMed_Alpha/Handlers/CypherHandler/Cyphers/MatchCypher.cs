using GraphMed_Alpha.DisplayHandler;
using GraphMed_Alpha.Model;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandler.Cyphers
{
    class MatchCypher : Cypher
    {
        public MatchCypher(int? limit) : base(limit) { }
        
        public IEnumerable<Display> ByTerm(string searchTerm)
        {
            try
            {
                return Client.Cypher
                             .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                             .Where("description.Term = '"+searchTerm+"'")
                             .Return<Display>("description")
                             .Results; 
            } finally
            {
                Client.Dispose(); 
            }
        }

        public IEnumerable<Display> ByConceptId(string searchTerm)
        {
            try
            {
                return Client.Cypher
                             .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                             .Where("concept.Id = '" + searchTerm + "'")
                             .Return<Display>("description")
                             .Results;

            }
            finally
            {
                Client.Dispose();
            }
        }

        public IEnumerable<Display> DoStuff<T1, T2>(string searchTerm, string searchBy, string relationship)
        {
            var anchorType = typeof(T1).Name;
            var targetType = typeof(T2).Name;

            try
            {
                var result = Client.Cypher
                                   .Match("(anchor:" + anchorType + ")-[:" + relationship.ToUpper() + "]-(target:" + targetType + ")")
                                   .Where("anchor." + searchBy + " = '" + searchTerm + "'")
                                   .Return<Display>("target")
                                   .Results;

                if (result != null)
                    return result;
                else
                    return null;
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
    }
}

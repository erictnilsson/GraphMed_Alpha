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


        public Concept Concept(string id)
        {
            return GetNode<Concept>(id, "Id").First();
        }

        public Description Description(string id)
        {
            return GetNode<Description>(id, "Id").First();
        }


        public Dictionary<Description, Concept> ByTerm(string searchTerm)
        {
            return GetNodes<Description, Concept>(searchTerm: searchTerm, searchBy: "Term", relationship: "refers_to"); 
        }

        private Dictionary<T1, T2> GetNodes<T1, T2>(string searchTerm, string searchBy, string relationship)
        {
            /* 
             * MATCH(d:Description)-[:REFERS_TO]->(c:Concept)
             * WHERE d.Term = "Duckbill flathead"
             * RETURN d, c 
             */
            var anchorType = typeof(T1).Name;
            var targetType = typeof(T2).Name;
            try
            {
                var result = Client.Cypher
                                   .Match("(a:" + anchorType + ")-[:" + relationship.ToUpper() + "]->(t:" + targetType + ")")
                                   .Where("a." + searchBy + " = '" + searchTerm + "'")
                                   .Return((a, t) => new
                                   {
                                       a = a.As<T1>(),
                                       t = t.As<T2>()
                                   })
                                   .Results;

                return new Dictionary<T1, T2>
                {
                    {
                      result.First().a,
                      result.First().t
                    }
                };
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

        private IEnumerable<T> GetNode<T>(string searchTerm, string searchBy)
        {
            var type = typeof(T);

            try
            {
                var result = Client.Cypher
                                   .Match("(node:" + type.Name + ")")
                                   .Where("node." + searchBy + " = '" + searchTerm + "'")
                                   .With("node")
                                   .Limit(Limit)
                                   .Return(node => node.As<T>())
                                   .Results;

                return result;
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

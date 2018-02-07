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
            return GetNode<Concept>(id, "Id").Single();
        }

        public Description Description(string id)
        {
            return GetNode<Description>(id, "Id").Single();
        }


        public Dictionary<Description, Concept> NodesByTerm(string searchTerm)
        {
            return GetLinkedNodes<Description, Concept>(searchTerm: searchTerm, searchBy: "Term", relationship: "refers_to"); 
        }

        public void NodesByConceptId(string searchTerm)
        {
            var a = GetLinkedNodes<Concept, Description>(searchTerm: searchTerm, searchBy: "Id", relationship: "refers_to");
            Console.WriteLine(); 
        }

        private Dictionary<T1, T2> GetLinkedNodes<T1, T2>(string searchTerm, string searchBy, string relationship)
        {
            var anchorType = typeof(T1).Name;
            var targetType = typeof(T2).Name;
            try
            {
                var result = Client.Cypher
                                   .Match("(a:" + anchorType + ")-[:" + relationship.ToUpper() + "]-(t:" + targetType + ")")
                                   .Where("a." + searchBy + " = '" + searchTerm + "'")
                                   .Return((a, t) => new
                                   {
                                       a = a.As<T1>(),
                                       t = t.As<T2>()
                                   })
                                   .Results;

                Console.WriteLine(); 
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

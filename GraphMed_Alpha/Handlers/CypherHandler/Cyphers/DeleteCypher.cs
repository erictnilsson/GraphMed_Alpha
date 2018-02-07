using GraphMed_Alpha.Model;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandler.Cyphers
{
    class DeleteCypher : Cypher
    {
        public DeleteCypher(int? limit) : base(limit) { }

        /* ---PUBLICS--- */
        public void DeleteAllConcepts(bool detach)
        {
            if (detach)
                DetachDeleteEveryNode(new Concept());
            else
                DeleteEveryNode(new Concept());
        }

        public void DeleteAllDescriptions(bool detach)
        {
            if (detach)
                DetachDeleteEveryNode(new Description());
            else
                DeleteEveryNode(new Description());
        }

        /* ---PRIVATES--- */
        private void DetachDeleteEveryNode(Node n)
        {
            try
            {
                Client.Cypher.Match("(n:" + n.GetType().Name + ")")
                         .With("n")
                         .Limit(Limit)
                         .DetachDelete("n")
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

        private void DeleteEveryNode(Node n)
        {
            try
            {
                Client.Cypher.Match("(n:" + n.GetType().Name + ")")
                         .With("n")
                         .Limit(Limit)
                         .Delete("n")
                         .ExecuteWithoutResults();

            } catch(NeoException)
            {
                throw; 
            } finally
            {
                Client.Dispose();
            }
        }
    }
}

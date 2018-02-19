using GraphMed_Alpha.Model;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandlers.Cyphers
{
    class DropCypher : Cypher
    {

        public DropCypher() : base() { }

        public void IndexOnConcept()
        {
            DropIndex(new Concept(), "Id");
        }

        public void IndexOnDescription()
        {
            DropIndex(new Description(), "ConceptId");
        }

        public void ConceptUniqueConstraint()
        {
            DropConstraint(new Concept(), "IS UNIQUE", "Id");
        }

        public void DescriptionUniqueConstraint()
        {
            DropConstraint(new Description(), "IS UNIQUE", "Id");
        }

        private void DropConstraint(Node target, string constraint, string constraint_on)
        {
            try
            {
                Client.Cypher.Drop("CONSTRAINT ON (n:" + target.GetType().Name + ") ASSERT n." + constraint_on + " " + constraint)
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

        private void DropIndex(Node target, string index_on)
        {
            try
            {
                Client.Cypher.Drop("INDEX ON :" + target.GetType().Name + " (" + index_on + ")")
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
    }
}

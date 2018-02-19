using Neo4jClient;

namespace GraphMed_Alpha.Handlers.CypherHandlers.Cyphers
{
    public abstract class Cypher
    {
        protected int? Limit { get; set; }
        protected GraphClient Client { get; set; }

        public Cypher()
        {
            this.Client = new ConnectionHandler().Connect(); 
        }

        public Cypher(int? limit)
        {
            this.Limit = limit;
            this.Client = new ConnectionHandler().Connect();
        }
    }
}

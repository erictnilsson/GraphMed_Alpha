using GraphMed_Alpha.Handlers.CypherHandlers.Cyphers;
using GraphMed_Alpha.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandlers
{
    class CypherHandler
    {
        public static LoadCypher Load(int? limit, int? commit)
        {
            return new LoadCypher(limit, commit);
        }

        public static CreateCypher Create()
        {
            return new CreateCypher();
        }

        public static MatchCypher Match(int? limit)
        {
            return new MatchCypher(limit); 
        }

        public static DeleteCypher Delete(int? limit)
        {
            return new DeleteCypher(limit); 
        }

        public static DropCypher Drop()
        {
            return new DropCypher(); 
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers.CypherHandler
{
    class CypherHandler
    {
        public static LoadCypher Load()
        {
            return new LoadCypher();
        }

        public static CreateCypher Create()
        {
            return new CreateCypher(); 
        } 
    }
}

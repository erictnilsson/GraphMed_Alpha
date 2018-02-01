using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers
{
    class ConnectionHandler : IDisposable
    {
        private static string user = ConfigurationManager.AppSettings["GraphDBUser"];
        private static string pass = ConfigurationManager.AppSettings["GraphDBPassword"];
        private static string uri = ConfigurationManager.AppSettings["ClientUri"];

        public ConnectionHandler()
        {
            //this.Connect(); 
        }

        public GraphClient Connect()
        {
            var client = new GraphClient(new Uri(uri), username: user, password: pass);
            client.Connect();

            return client;
        }

        public void Dispose()
        {
            this?.Dispose();
        }
    }
}

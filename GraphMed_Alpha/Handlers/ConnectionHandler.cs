using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GraphMed_Alpha.Handlers
{
    class ConnectionHandler : IDisposable
    {
        private static string user { get; set; }
        private static string pass { get; set; }
        private static string uri { get; set; }
        private static double timeout { get; set; }
        private static HttpClient httpClient { get; set; }
        public ConnectionHandler()
        {
            user = ConfigurationManager.AppSettings["GraphDBUser"];
            pass = ConfigurationManager.AppSettings["GraphDBPassword"];
            uri = ConfigurationManager.AppSettings["ClientUri"];
            timeout = 2D;
            httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(timeout) };
            //this.Connect(); 
        }

        public GraphClient Connect()
        {
            var client = new GraphClient(new Uri(uri), new HttpClientWrapper(user, pass, httpClient));
            client.Connect();

            // var client = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
            //GraphClient gc = new GraphClient(new Uri("http://localhost:7474/db/data"), new HttpClientWrapper("user", "pass", client));
            return client;
        }

        public void Dispose()
        {
            this?.Dispose();
        }
    }
}

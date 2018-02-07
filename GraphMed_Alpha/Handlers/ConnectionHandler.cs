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
        private static string User { get; set; }
        private static string Pass { get; set; }
        private static string Uri { get; set; }
        private static HttpClient HttpClient { get; set; }
        public ConnectionHandler()
        {
            User = ConfigurationManager.AppSettings["GraphDBUser"];
            Pass = ConfigurationManager.AppSettings["GraphDBPassword"];
            Uri = ConfigurationManager.AppSettings["ClientUri"];
            HttpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(2D) };
            //this.Connect(); 
        }

        public GraphClient Connect()
        {
            var client = new GraphClient(new Uri(Uri), new HttpClientWrapper(User, Pass, HttpClient));
            client.Connect();

            return client;
        }

        public void Dispose()
        {
            HttpClient?.Dispose();
            GC.SuppressFinalize(this); 
        }
    }
}

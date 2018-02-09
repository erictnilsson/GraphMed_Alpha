﻿using GraphMed_Alpha.DisplayHandler;
using GraphMed_Alpha.Model;
using Neo4jClient;
using System.Collections.Generic;

namespace GraphMed_Alpha.Handlers.CypherHandler.Cyphers
{
    class MatchCypher : Cypher
    {
        public MatchCypher(int? limit) : base(limit) { }

        public IEnumerable<Display> ByTypeId(bool fullySpecified)
        {
            var specified = "900000000000003001";
            var compOp = "<>";

            if (fullySpecified)
                compOp = "=";
            try
            {
                return Client.Cypher
                             .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                             .Where("description.TypeId " + compOp + " '" + specified + "'")
                             .With("description")
                             .Limit(Limit)
                             .Return<Display>("description")
                             .Results;
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
        public IEnumerable<Display> ByTerm(bool withSynonyms, string term)
        {
            try
            {
                if (withSynonyms)
                    return Client.Cypher
                                 .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                                 .Where("description.Term = '" + term + "'")
                                 .With("description")
                                 .Limit(Limit)
                                 .OptionalMatch("(concept)-[:REFERS_TO]-(synonym:Description)")
                                 .Return<Display>("synonym")
                                 .Results;
                else
                    return Client.Cypher
                            .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                            .Where("description.Term = '" + term + "'")
                            .With("description")
                            .Limit(Limit)
                            .Return<Display>("description")
                            .Results;
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

        public IEnumerable<Display> ByConceptId(string Id)
        {
            try
            {
                return Client.Cypher
                             .Match("(concept:Concept)-[:REFERS_TO]-(description:Description)")
                             .Where("concept.Id = '" + Id + "'")
                             .With("concept")
                             .Limit(Limit)
                             .Return<Display>("description")
                             .Results;
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

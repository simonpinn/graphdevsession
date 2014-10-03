using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bauer.Data.Neo4j;
using DomainModel.Entities;
using DomainModel.Relationships;
using Neo4jClient;
using Newtonsoft.Json.Serialization;

namespace PumaGraph
{
    class Program
    {
        static void Main(string[] args)
        {
            //setup 
            var client = new GraphClient(new Uri("http://localhost:7477/db/data"));
            client.JsonContractResolver = new CamelCasePropertyNamesContractResolver();
            client.Connect();

            //step 1 - query something 
            var query = client.Cypher.MatchEntity(new Application {Name = "Oryx"}, "app").Return(app => app.As<Application>());
            Console.WriteLine(query.GetFormattedDebugText());
            var result = query.Results;

            //step 2 - achieve manual
            var query2 = client.Cypher
                .MergeEntity(new Application {Name = "Oryx"}, "app")
                .MergeEntity(new Role {Name = "Finance"}, "finance")
                .MergeEntity(new Role {Name = "Editor"}, "editor")
                .MergeEntity(new Function {Name = "Access System"}, "access")
                .MergeEntity(new Function {Name = "Provider"}, "provider")
                .MergeEntity(new Function {Name = "View Provider"}, "vprovider")
                .MergeEntity(new Function {Name = "Edit Provider"}, "eprovider")
                .MergeEntity(new Function {Name = "Search Provider"}, "sprovider")
                .MergeRelationship(new HasRole("app", "finance"))
                .MergeRelationship(new HasRole("app", "editor"))
                .MergeRelationship(new Includes("provider", "access"))
                .MergeRelationship(new Includes("sprovider", "provider"))
                .MergeRelationship(new Includes("vprovider", "sprovider"))
                .MergeRelationship(new Includes("eprovider", "vprovider"))
                .MergeRelationship(new Includes("finance", "vprovider"))
                .MergeRelationship(new Includes("editor", "eprovider"));

            Console.WriteLine(query2.GetFormattedDebugText());
            query2.ExecuteWithoutResults();

            // Add a user and a profile
            var query3 = client.Cypher
                .MatchEntity(new Role {Name = "Finance"}, "finance")
                .MatchEntity(new Role {Name = "Editor"}, "editor")
                .MergeEntity(new User {Name = "Simon"}, "simon")
                .MergeEntity(new User {Name = "Ben"}, "ben")
                .MergeEntity(new Profile {Name = "Super"}, "profile")
                .MergeRelationship(new IsInRole("simon", "finance"))
                .MergeRelationship(new HasProfile("ben", "profile"))
                .MergeRelationship(new IsInRole("profile", "editor"));

            Console.WriteLine(query3.GetFormattedDebugText());
            query3.ExecuteWithoutResults();

            Console.ReadLine();
        }

        /*
         * 
MATCH (n) OPTIONAL MATCH (n)-[r]-() DELETE n,r
         
MERGE (app:Application{name:'Oryx'})
MERGE (fRole:Role{name:'Finance'})
MERGE (eRole:Role{name:'Editor'})
MERGE (aFun:Function{name:'Access System'})
MERGE (pFun:Function{name:'Provider'})
MERGE (vFun:Function{name:'View Provider'})
MERGE (eFun:Function{name:'Edit Provider'})
MERGE (sFun:Function{name:'Search Provider'})
MERGE (app)-[:HAS_ROLE]->(fRole)
MERGE (app)-[:HAS_ROLE]->(eRole)
MERGE (vFun)-[:INCLUDES]->(sFun)-[:INCLUDES]->(pFun)-[:INCLUDES]->(aFun)
MERGE (eFun)-[:INCLUDES]->(vFun)

match (f:Role{name:'Editor'})
match (fun:Function{name:'Edit Provider'})
merge (f)-[:INCLUDES]->(fun)

match (f:Role{name:'Finance'})
match (fun:Function{name:'View Provider'})
merge (f)-[:INCLUDES]->(fun)

match (r:Role{name:'Editor'})-[:INCLUDES*..]->(f:Function) return r,f

MERGE (u:User{name:'Ben'})
MERGE (r:Role{name:'Editor'})
MERGE (u)-[:IS_In_ROLE]->(r)

MATCH (u:User{name:'Simon'})
MATCH (u)-[*]->(r:Role)-[:INCLUDES*..]->(f:Function)
RETURN u,f

         * 
         * */
    }
}

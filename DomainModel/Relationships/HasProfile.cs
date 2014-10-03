using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bauer.Data.Neo4j;

namespace DomainModel.Relationships
{
    [CypherLabel(Name = "HAS_PROFILE")]
    public class HasProfile:BaseRelationship
    {
        public HasProfile(string from, string to)
            : base(from, to)
        {
            
        }
    }
}

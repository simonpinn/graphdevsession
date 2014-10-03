using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bauer.Data.Neo4j;

namespace DomainModel.Relationships
{
    [CypherLabel(Name = "IS_IN_ROLE")]
    public class IsInRole : BaseRelationship
    {
        public IsInRole(string from, string to)
            : base(from, to)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bauer.Data.Neo4j;

namespace DomainModel.Entities
{
    public abstract class BaseEntity
    {
        protected BaseEntity()
        {
            Id = Guid.NewGuid();
        }
        [CypherMergeOnCreate]
        public Guid Id { get; set; }
        [CypherMerge]
        [CypherMatch]
        public string Name { get; set; }
    }
}

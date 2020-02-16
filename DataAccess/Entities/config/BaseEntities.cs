using Dapper.Contrib.Extensions;
using System;

namespace DataAccess.Entities
{
    public class BaseEntities
    {
        [ExplicitKey]
        public string id { get; set; }
        public DateTime? createdAt { get; set; }
        public DateTime? updatedAt { get; set; }
        public DateTime? deletedAt { get; set; }
    }
}

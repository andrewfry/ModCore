using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.Models.BaseEntities
{
    public abstract class Entity<T>
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public T Id { get; set; }
    }
}

using ModCore.Models.BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DataAccess.MongoDb.Test
{
    public class TestObject : BaseEntity
    {

        public string Name { get; set; }

        public double Price { get; set; }
    }
}

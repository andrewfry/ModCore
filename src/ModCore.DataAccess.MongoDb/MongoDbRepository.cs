using Microsoft.Extensions.Options;
using ModCore.Abstraction.DataAccess;
using ModCore.Models.BaseEntities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DataAccess.MongoDb
{
    public class MongoDbRepository<T> : IDataRepository<T> where T : BaseEntity
    {
        protected internal IMongoCollection<T> collection;

        public MongoDbRepository(IOptions<MongoDbSettings> dbSettings)
        {
            this.collection = Util.GetCollectionFromConnectionString<T>(dbSettings.Value.ConnectionString);
        }

        public MongoDbRepository(string connectionString)
        {
            this.collection = Util.GetCollectionFromConnectionString<T>(connectionString);
        }

        public MongoDbRepository(string connectionString, string collectionName)
        {
            this.collection = Util.GetCollectionFromConnectionString<T>(connectionString, collectionName);
        }

        public MongoDbRepository(MongoUrl url)
        {
            this.collection = Util.GetCollectionFromUrl<T>(url);
        }

        public MongoDbRepository(MongoUrl url, string collectionName)
        {
            this.collection = Util.GetCollectionFromUrl<T>(url, collectionName);
        }

        public IMongoCollection<T> Collection
        {
            get { return this.collection; }
        }

        public string CollectionName
        {
            get { return this.collection.CollectionNamespace.CollectionName; }
        }

        public virtual void Insert(T entity)
        {
            this.collection.InsertOneAsync(entity);
        }

        public virtual void Insert(ICollection<T> entities)
        {
            this.collection.InsertManyAsync(entities);
        }

        public virtual T Update(T entity)
        {
            if (entity.Id == null)
                this.Insert(entity);
            else
                this.collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            return entity;
        }

        public virtual void Update(ICollection<T> entities)
        {

            foreach (var entity in entities)
            {
                this.collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            }
        }

        public virtual void DeleteAll(ISpecification<T> specification)
        {
            this.collection.DeleteManyAsync<T>(specification.GetExpression());
        }

        public virtual void Delete(ISpecification<T> specification)
        {
            this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToList();


            var entity = Find(specification);
            if (entity != null)
            {
                this.collection.DeleteOneAsync(specification.GetExpression());
            }
        }

        public virtual ICollection<T> FindAll(ISpecification<T> specification)
        {
            return this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToList();
        }

        public virtual T Find(ISpecification<T> specification)
        {
            return this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).SingleOrDefault();
        }


        private static FilterDefinition<T> GetIDFilter(ObjectId id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }

        private static FilterDefinition<T> GetIDFilter(string id)
        {
            return Builders<T>.Filter.Eq("_id", id);
        }
    }
}

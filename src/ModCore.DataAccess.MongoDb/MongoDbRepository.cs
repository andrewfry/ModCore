using Microsoft.Extensions.Options;
using ModCore.Abstraction.DataAccess;
using ModCore.Core.DataAccess;
using ModCore.DataAccess.MongoDb;
using ModCore.Models.BaseEntities;
using ModCore.Specifications.Base;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModCore.DataAccess.MongoDb
{
    public class MongoDbRepository<T> : IDataRepository<T>, IDataRepositoryAsync<T> where T : BaseEntity
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
            this.collection.InsertOne(entity);
        }

        public virtual void Insert(ICollection<T> entities)
        {
            this.collection.InsertMany(entities);
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
                this.collection.ReplaceOne(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            }
        }

        public virtual void DeleteAll(ISpecification<T> specification)
        {
            this.collection.DeleteMany<T>(specification.GetExpression());
        }

        public virtual void Delete(ISpecification<T> specification)
        {

            this.collection.DeleteOne(specification.GetExpression());
        }

        public virtual void DeleteById(string id)
        {
            Delete(new GetById<T>(id));
        }

        public virtual ICollection<T> FindAll(ISpecification<T> specification)
        {
            return this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToList();
        }

        public virtual ICollection<T> FindAll()
        {
            return this.collection.AsQueryable<T>().ToList();
        }
        public virtual T Find(ISpecification<T> specification)
        {
            return this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).SingleOrDefault();
        }


        public virtual T FindById(string id)
        {
            return Find(new GetById<T>(id));
        }

        //Async Methods

        public virtual async Task InsertAsync(T entity)
        {
            await this.collection.InsertOneAsync(entity);
        }

        public virtual async Task InsertAsync(ICollection<T> entities)
        {
            await this.collection.InsertManyAsync(entities);
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity.Id == null)
                await this.InsertAsync(entity);
            else
                await this.collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            return entity;
        }

        public virtual async Task UpdateAsync(ICollection<T> entities)
        {

            foreach (var entity in entities)
            {
                await this.collection.ReplaceOneAsync(GetIDFilter(entity.Id), entity, new UpdateOptions { IsUpsert = true });
            }
        }

        public virtual async Task DeleteAllAsync(ISpecification<T> specification)
        {
            await this.collection.DeleteManyAsync<T>(specification.GetExpression());
        }

        public virtual async Task DeleteAsync(ISpecification<T> specification)
        {
            await this.collection.DeleteOneAsync(specification.GetExpression());
        }

        public virtual async Task DeleteByIdAsync(string id)
        {
             await DeleteAsync(new GetById<T>(id));
        }

        public virtual async Task<IPagedResult<T>> FindAllByPageAsync(ISpecification<T> specification, IPagedRequest request)
        {
            if (request.PageSize == 0)
                throw new Exception("request.PageSize can not be zero.");

            var result = new PagedResult<T>();

            if (request.TotalResults == null || request.TotalResults == 0)
            {
                result.TotalResults = await Task.Run<int>(() => this.collection.AsQueryable<T>().Count<T>(specification.Predicate()));
            }

            result.PageSize = request.PageSize;
            result.CurrentPage = request.CurrentPage;

            result.CurrentPageResults = await Task.Run<IList<T>>(() => this.collection.AsQueryable<T>().Skip<T>((request.CurrentPage - 1) * request.PageSize).Take<T>(request.PageSize).ToList());

            return result;
        }

        public virtual async Task<ICollection<T>> FindAllAsync(ISpecification<T> specification)
        {
            //return await this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToListAsync();
            return await Task.Run<ICollection<T>>(() => this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToList());

        }
        public virtual async Task<ICollection<T>> FindAllAsync()
        {
            //return await this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).ToListAsync();
            return await Task.Run<ICollection<T>>(() => this.collection.AsQueryable<T>().ToList());

        }
        public virtual async Task<T> FindAsync(ISpecification<T> specification)
        {
            //TODO: No SingleOrDefaultAsync() in .net core?
            return await Task.Run<T>(() => this.collection.AsQueryable<T>().Where<T>(specification.Predicate()).SingleOrDefault());

            //await this.Collection.FindAsync<T>(specification.GetExpression());
        }

        public virtual async Task<T> FindByIdAsync(string id)
        {
            return await FindAsync(new GetById<T>(id));
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

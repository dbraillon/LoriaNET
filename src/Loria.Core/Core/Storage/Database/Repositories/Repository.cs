using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace LoriaNET.Storage.Database
{
    public abstract class Repository<TEntity>
        where TEntity : Entity
    {
        public TEntity Create(TEntity entity)
        {
            using (var db = new LoriaDbContext())
            {
                db.Set<TEntity>().Add(entity);
                db.SaveChanges();
            }

            return entity;
        }

        public TEntity Read(params object[] keyValues)
        {
            using (var db = new LoriaDbContext())
            {
                return db.Set<TEntity>().Find(keyValues);
            }
        }

        public IEnumerable<TEntity> ReadAll()
        {
            using (var db = new LoriaDbContext())
            {
                return db.Set<TEntity>().ToList();
            }
        }
        public IEnumerable<TEntity> ReadAll(Expression<Func<TEntity, bool>> predicate)
        {
            using (var db = new LoriaDbContext())
            {
                return db.Set<TEntity>().Where(predicate).ToList();
            }
        }

        public TEntity Update(TEntity entity)
        {
            using (var db = new LoriaDbContext())
            {
                db.Set<TEntity>().Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }

            return entity;
        }

        public TEntity Delete(TEntity entity)
        {
            using (var db = new LoriaDbContext())
            {
                db.Set<TEntity>().Attach(entity);
                db.Entry(entity).State = EntityState.Deleted;
                db.SaveChanges();
            }

            return entity;
        }
    }
}

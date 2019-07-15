using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace XPW.Utilities.BaseContext {
     public class BaseService<T> : IDisposable where T : class, new() {
          T _repository;
          protected T Repository() {
               if (_repository == null) {
                    _repository = new T();
                    return _repository;
               } else {
                    return _repository;
               }
          }
          public virtual void Dispose() {
               _repository = null;
          }
     }

     public abstract class BaseService<T, TR> where TR : IBaseRepository<T>, new() where T : class, new() {
          #region Repository
          private TR _Repository;
          protected TR Repository {
               get {
                    if (_Repository == null)
                         _Repository = new TR();

                    return _Repository;
               }
          }
          private TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(15);

          public virtual void Save(T entity) {
               if (entity == null)
                    throw new ArgumentNullException("entity");

               Repository.Add(entity);
               Repository.Save();
          }

          public virtual void Save(IEnumerable<T> entities) {
               if (entities == null)
                    throw new ArgumentNullException("entities");

               Repository.Add(entities);
               Repository.Save();
          }

          public virtual void Update(T entity) {
               if (entity == null)
                    throw new ArgumentNullException("entity");

               Repository.Edit(entity);
               Repository.Save();
          }

          public virtual void Update(IEnumerable<T> entity) {
               if (entity == null)
                    throw new ArgumentNullException("entity");

               Repository.Edit(entity);
               Repository.Save();
          }

          public virtual void Delete(Guid id) {
               Repository.Delete(id);
               Repository.Save();
          }

          public virtual void Delete(IEnumerable<Guid> ids) {
               foreach (var id in ids)
                    Repository.Delete(id);
               Repository.Save();
          }

          public virtual T Get(Guid id) {
               return Repository.Find(id);
          }

          public virtual T Get(int id) {
               return Repository.Find(id);
          }

          public virtual T Get(Expression<Func<T, bool>> where) {
               return Repository.All()
                               .Where(where)
                               .FirstOrDefault();
          }

          public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where) {
               return Repository.All()
                                .Where(where);
          }

          public virtual IEnumerable<T> GetAll() {
               return Repository.All();
          }

          public virtual bool Check(int id) {
               var entity = Repository.Find(id);
               if (entity != null)
                    return true;
               else
                    return false;
          }

          public virtual bool Check(Guid id) {
               var entity = Repository.Find(id);
               if (entity != null)
                    return true;
               else
                    return false;
          }

          public virtual bool Check(Expression<Func<T, bool>> where) {
               return Repository.All().Any(where);
          }

          public virtual void Dispose() {
               Repository.Dispose();
          }

          public virtual List<T> StoredProcedureList(string storedProcName, List<StoredProcedureParam> parameters) {
               return Repository.StoredProcedureList(storedProcName, parameters);
          }

          public virtual T StoredProcedure(string storedProcName, List<StoredProcedureParam> parameters) {
               return Repository.StoredProcedure(storedProcName, parameters);
          }

          #endregion
     }
}

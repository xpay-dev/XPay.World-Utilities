using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContext {
     [Serializable]
     public abstract class BaseService<T> : IDisposable where T : class, new() {
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

     [Serializable]
     public abstract class BaseService<T, TR> where TR : IBaseRepository<T>, IDisposable, new() where T : class, new() {
          #region Repository
          private TR _Repository;
          protected TR Repository {
               get {
                    if (_Repository == null)
                         _Repository = new TR();

                    return _Repository;
               }
          }

          public virtual async Task SaveAsync(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    Repository.Add(entity);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    RevisionLog<T> revision = new RevisionLog<T> {
                         Context = contextName,
                         Entity = new T().GetType().Name,
                         Revisions = entity,
                         RevisionType = Enums.RevisionType.Create
                    };
                    await RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task<T> SaveReturnAsync(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    entity = Repository.AddReturn(entity);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    RevisionLog<T> revision = new RevisionLog<T> {
                         Context = contextName,
                         Entity = new T().GetType().Name,
                         Revisions = entity,
                         RevisionType = Enums.RevisionType.Create
                    };
                    await RevisionLogs<T>.Write(revision, contextName, fileName);
                    return entity;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Save(IEnumerable<T> entities) {
               try {
                    if (entities == null) {
                         throw new ArgumentNullException("entities");
                    }
                    Repository.Add(entities);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Create
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual IEnumerable<T> SaveReturn(IEnumerable<T> entities) {
               try {
                    if (entities == null)
                         throw new ArgumentNullException("entities");

                    entities = Repository.AddReturn(entities);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Create
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
                    return entities;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task UpdateAsync(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    Repository.Edit(entity);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    RevisionLog<T> revision = new RevisionLog<T> {
                         Context = contextName,
                         Entity = new T().GetType().Name,
                         Revisions = entity,
                         RevisionType = Enums.RevisionType.Update
                    };
                    await RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task<T> UpdateReturnAsync(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    entity = Repository.EditReturn(entity);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    RevisionLog<T> revision = new RevisionLog<T> {
                         Context = contextName,
                         Entity = new T().GetType().Name,
                         Revisions = entity,
                         RevisionType = Enums.RevisionType.Update
                    };
                    await RevisionLogs<T>.Write(revision, contextName, fileName);
                    return entity;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Update(IEnumerable<T> entities) {
               try {
                    if (entities == null)
                         throw new ArgumentNullException("entity");

                    Repository.Edit(entities);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Update
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual IEnumerable<T> UpdateReturn(IEnumerable<T> entities) {
               try {
                    if (entities == null)
                         throw new ArgumentNullException("entity");

                    entities = Repository.EditReturn(entities);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Update
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
                    return entities;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task DeleteAsync(Guid id) {
               await Task.Run(async () => {
                    try {
                         var entity = Get(id);
                         Repository.Delete(id);
                         Repository.Save();
                         string contextName = Repository.ContextName();
                         string fileName = new T().GetType().Name + ".json";
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = await entity,
                              RevisionType = Enums.RevisionType.Delete
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }

          public virtual async Task DeleteAsync(IEnumerable<Guid> ids) {
               try {
                    var entities = new List<T>();
                    foreach (var id in ids) {
                         entities.Add(await Get(id));
                         Repository.Delete(id);
                    }
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Delete
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task DeleteAsync(int id) {
               try {
                    var entity = await Get(id);
                    Repository.Delete(id);
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    RevisionLog<T> revision = new RevisionLog<T> {
                         Context = contextName,
                         Entity = new T().GetType().Name,
                         Revisions = entity,
                         RevisionType = Enums.RevisionType.Delete
                    };
                    await RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task DeleteAsync(IEnumerable<int> ids) {
               try {
                    var entities = new List<T>();
                    foreach (var id in ids) {
                         entities.Add(await Get(id));
                         Repository.Delete(id);
                    }
                    Repository.Save();
                    string contextName = Repository.ContextName();
                    string fileName = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(async a => {
                         RevisionLog<T> revision = new RevisionLog<T> {
                              Context = contextName,
                              Entity = new T().GetType().Name,
                              Revisions = a,
                              RevisionType = Enums.RevisionType.Delete
                         };
                         await RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual async Task<T> Get(Guid id) {
               return await Task.Run(() => {
                    try {
                         return Repository.Find(id);
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }

          public virtual async Task<T> Get(int id) {
               return await Task.Run(() => {
                    try {
                         return Repository.Find(id);
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }

          public virtual async Task<T> GetAsync(Expression<Func<T, bool>> where) {
               return await Task.Run(() => {
                    try {
                         return Repository.All()
                                    .Where(where)
                                    .FirstOrDefault();
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }

          public virtual async Task<IEnumerable<T>> GetAllByAsync(Expression<Func<T, bool>> where) {
               return await Task.Run(() => {
                    try {
                         return Repository.All()
                                          .Where(where);
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }

          public virtual IEnumerable<T> GetAll() {
               try {
                    return Repository.All();
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual bool Check(int id) {
               try {
                    var entity = Repository.Find(id);
                    if (entity != null)
                         return true;
                    else
                         return false;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual bool Check(Guid id) {
               try {
                    var entity = Repository.Find(id);
                    if (entity != null)
                         return true;
                    else
                         return false;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual bool Check(Expression<Func<T, bool>> where) {
               try {
                    return Repository.All().Any(where);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Dispose() {
               try {
                    Repository.Dispose();
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual List<T> StoredProcedureList(string storedProcName, List<StoredProcedureParam> parameters) {
               try {
                    return Repository.StoredProcedureList(storedProcName, parameters);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual T StoredProcedure(string storedProcName, List<StoredProcedureParam> parameters) {
               try {
                    return Repository.StoredProcedure(storedProcName, parameters);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          #endregion
     }
}

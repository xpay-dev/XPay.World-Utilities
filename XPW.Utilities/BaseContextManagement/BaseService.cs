using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContext {
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

          public virtual void Save(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    Repository.Add(entity);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Create
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          
          public virtual T SaveReturn(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    entity = Repository.AddReturn(entity);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Create
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
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
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Create
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
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
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Create
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
                    return entities;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Update(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    Repository.Edit(entity);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Update
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual T UpdateReturn(T entity) {
               try {
                    if (entity == null)
                         throw new ArgumentNullException("entity");

                    entity = Repository.EditReturn(entity);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Update
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
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
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Update
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
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
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Update
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
                    return entities;
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Delete(Guid id) {
               try {
                    var entity = Get(id);
                    Repository.Delete(id);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Delete
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Delete(IEnumerable<Guid> ids) {
               try {
                    var entities = new List<T>();
                    foreach (var id in ids) {
                         entities.Add(Get(id));
                         Repository.Delete(id);
                    }
                    Repository.Save();
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Delete
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Delete(int id) {
               try {
                    var entity = Get(id);
                    Repository.Delete(id);
                    Repository.Save();
                    string contextName       = Repository.ContextName();
                    string fileName          = new T().GetType().Name + ".json";
                    RevisionLog<T> revision  = new RevisionLog<T> {
                         Context             = contextName,
                         Entity              = new T().GetType().Name,
                         Revisions           = entity,
                         RevisionType        = Enums.RevisionType.Delete
                    };
                    RevisionLogs<T>.Write(revision, contextName, fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual void Delete(IEnumerable<int> ids) {
               try {
                    var entities = new List<T>();
                    foreach (var id in ids) {
                         entities.Add(Get(id));
                         Repository.Delete(id);
                    }
                    Repository.Save();
                    string contextName            = Repository.ContextName();
                    string fileName               = new T().GetType().Name + ".json";
                    entities.ToList().ForEach(a   => {
                         RevisionLog<T> revision  = new RevisionLog<T> {
                              Context             = contextName,
                              Entity              = new T().GetType().Name,
                              Revisions           = a,
                              RevisionType        = Enums.RevisionType.Delete
                         };
                         RevisionLogs<T>.Write(revision, contextName, fileName);
                    });
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual T Get(Guid id) {
               try {
                    return Repository.Find(id);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual T Get(int id) {
               try {
                    return Repository.Find(id);
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual T Get(Expression<Func<T, bool>> where) {
               try {
                    return Repository.All()
                               .Where(where)
                               .FirstOrDefault();
               } catch (Exception ex) {
                    throw ex;
               }
          }

          public virtual IEnumerable<T> GetAllBy(Expression<Func<T, bool>> where) {
               try {
                    return Repository.All()
                                .Where(where);
               } catch (Exception ex) {
                    throw ex;
               }
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

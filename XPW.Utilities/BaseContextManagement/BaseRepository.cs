using RefactorThis.GraphDiff;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace XPW.Utilities.BaseContext {
     [Serializable]
     public abstract class BaseRepository<C, T> : IBaseRepository<T>, IDisposable
        where C : DbContext, new()
        where T : class, new() {
          public C Context { get; set; } = Activator.CreateInstance<C>();
          private TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(15);
          public BaseRepository() {
               try {
                    if (!Context.Database.Exists()) {
                         throw new Exception("Cannot connect to database");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual string ContextName() {
               return Context.GetType().Name;
          }
          public virtual IQueryable<T> All() {
               try {
                    IQueryable<T> queryable = Context.Set<T>();
                    Task task = Task.Run(() => {
                         queryable = Context.Set<T>();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return queryable;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties) {
               try {
                    IQueryable<T> queryable = Context.Set<T>();
                    Task task = Task.Run(() => {
                         for (int i = 0; i < includeProperties.Length; i++) {
                              Expression<Func<T, object>> expression = includeProperties[i];
                              queryable = QueryableExtensions.Include(queryable, expression);
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return queryable;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T Find(int id) {
               try {
                    T entityObject = new T();
                    Task task = Task.Run(() => {
                         entityObject = Context.Set<T>().Find(new object[] {
                              id
                         });
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObject;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T Find(Guid id) {
               try {
                    T entityObject = new T();
                    Task task = Task.Run(() => {
                         entityObject = Context.Set<T>().Find(new object[] {
                              id
                         });
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObject;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Add(T entity) {
               try {
                    Task task = Task.Run(() => {
                         Context.Set<T>().Add(entity);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Add(IEnumerable<T> entities) {
               try {
                    List<T> entityObjecTimeSpan = new List<T>();
                    Task task = Task.Run(() => {
                         foreach (T entity in entities)
                              Context.Set<T>().Add(entity);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Edit(T entity) {
               try {
                    Task task = Task.Run(() => {
                         if (Context.Set<T>().Local.Any(e => e == entity)) {
                              Context.Entry(entity).State = EntityState.Modified;
                         } else {
                              Context.UpdateGraph(entity);
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Edit(IEnumerable<T> entities) {
               try {
                    Task task = Task.Run(() => {
                         foreach (T entity in entities) {
                              if (Context.Set<T>().Local.Any(e => e == entity)) {
                                   Context.Entry(entity).State = EntityState.Modified;
                              } else {
                                   Context.UpdateGraph<T>(entity);
                              }
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Edit(T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping) {
               try {
                    Task task = Task.Run(() => {
                         Context.UpdateGraph(entity, mapping);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Edit(IEnumerable<T> entities, Expression<Func<IUpdateConfiguration<T>, object>> mapping) {
               try {
                    Task task = Task.Run(() => {
                         foreach (T entity in entities)
                              Context.UpdateGraph(entity, mapping);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T AddReturn(T entity) {
               try {
                    T entityObject = new T();
                    Task task = Task.Run(() => {
                         entityObject = Context.Set<T>().Add(entity);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObject;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual List<T> AddReturn(IEnumerable<T> entities) {
               try {
                    List<T> entityObjecTimeSpan = new List<T>();
                    Task task = Task.Run(() => {
                         foreach (T entity in entities) {
                              T entityObject = Context.Set<T>().Add(entity);
                              entityObjecTimeSpan.Add(entityObject);
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObjecTimeSpan;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T EditReturn(T entity) {
               try {
                    T entityObject = new T();
                    Task task = Task.Run(() => {
                         if (Context.Set<T>().Local.Any(e => e == entity)) {
                              Context.Entry(entity).State = EntityState.Modified;
                              entityObject = entity;
                         } else {
                              entityObject = Context.UpdateGraph(entity);
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObject;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual List<T> EditReturn(IEnumerable<T> entities) {
               try {
                    List<T> entityObjecTimeSpan = new List<T>();
                    Task task = Task.Run(() => {
                         foreach (T entity in entities) {
                              if (Context.Set<T>().Local.Any(e => e == entity)) {
                                   Context.Entry(entity).State = EntityState.Modified;
                                   entityObjecTimeSpan.Add(entity);
                              } else {
                                   T entityObject = Context.UpdateGraph(entity);
                                   entityObjecTimeSpan.Add(entityObject);
                              }
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObjecTimeSpan;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T EditReturn(T entity, Expression<Func<IUpdateConfiguration<T>, object>> mapping) {
               try {
                    T entityObject = new T();
                    Task task = Task.Run(() => {
                         entityObject = Context.UpdateGraph(entity, mapping);
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObject;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual List<T> EditReturn(IEnumerable<T> entities, Expression<Func<IUpdateConfiguration<T>, object>> mapping) {
               try {
                    List<T> entityObjecTimeSpan = new List<T>();
                    Task task = Task.Run(() => {
                         foreach (T entity in entities) {
                              Context.UpdateGraph(entity, mapping);
                              entityObjecTimeSpan.Add(entity);
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return entityObjecTimeSpan;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual void Delete(int id) {
               DbContextTransaction trans = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
               try {
                    Task task = Task.Run(() => {
                         T t = Context.Set<T>().Find(new object[] {
                              id
                         });
                         Context.Set<T>().Remove(t);
                         trans.Commit();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    trans.Rollback();
                    throw ex;
               }
          }
          public virtual void Delete(Guid id) {
               DbContextTransaction trans = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
               try {
                    Task task = Task.Run(() => {
                         T t = Context.Set<T>().Find(new object[] {
                              id
                         });
                         Context.Set<T>().Remove(t);
                         trans.Commit();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    trans.Rollback();
                    throw ex;
               }
          }
          public virtual void Save() {
               DbContextTransaction trans = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
               try {
                    Context.SaveChanges();
                    trans.Commit();
               } catch (Exception ex) {
                    trans.Rollback();
                    throw ex;
               }
          }
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
          public virtual async Task SaveAsync() {
               DbContextTransaction trans = Context.Database.BeginTransaction(IsolationLevel.ReadCommitted);
               try {
                    Task task = Task.Run(() => {
                         Context.SaveChanges();
                         Context.SaveChangesAsync();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
               } catch (Exception ex) {
                    trans.Rollback();
                    throw ex;
               }
          }
          public virtual void Dispose() {
               Context.Dispose();
          }
          public virtual T StoredProcedure(string storedProcName, List<StoredProcedureParam> parameters) {
               try {
                    T result = new T();
                    Task task = Task.Run(() => {
                         string sqlCommand = string.Format("exec {0}", storedProcName);
                         List<SqlParameter> storedProcParams = new List<SqlParameter>();
                         parameters.ForEach(a => {
                              sqlCommand = string.Format("{0} @{1}", sqlCommand, a.Param);
                              storedProcParams.Add(new SqlParameter("@" + a.Param, a.Value));
                         });
                         result = Context.Database.SqlQuery<T>(sqlCommand, storedProcParams.ToArray()).ToList().FirstOrDefault();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return result;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual List<T> StoredProcedureList(string storedProcName, List<StoredProcedureParam> parameters) {
               try {
                    List<T> results = new List<T>();
                    Task task = Task.Run(() => {
                         string sqlCommand = string.Format("exec {0}", storedProcName);
                         List<SqlParameter> storedProcParams = new List<SqlParameter>();
                         parameters.ForEach(a => {
                              sqlCommand = string.Format("{0} @{1}", sqlCommand, a.Param);
                              storedProcParams.Add(new SqlParameter("@" + a.Param, a.Value));
                         });
                         results = Context.Database.SqlQuery<T>(sqlCommand, storedProcParams.ToArray()).ToList();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return results;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual T StoredProcedure(string storedProcName) {
               try {
                    T result = new T();
                    Task task = Task.Run(() => {
                         string sqlCommand = string.Format("exec {0}", storedProcName);
                         result = Context.Database.SqlQuery<T>(sqlCommand).ToList().FirstOrDefault();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return result;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public virtual List<T> StoredProcedureList(string storedProcName) {
               try {
                    List<T> results = new List<T>();
                    Task task = Task.Run(() => {
                         string sqlCommand = string.Format("exec {0}", storedProcName);
                         results = Context.Database.SqlQuery<T>(sqlCommand).ToList();
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return results;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }

     public interface IBaseRepository<T> : IDisposable where T : class {
          string ContextName();
          IQueryable<T> All();
          IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
          T Find(int id);
          T Find(Guid id);
          void Add(T entity);
          void Add(IEnumerable<T> entity);
          void Edit(T entity);
          void Edit(IEnumerable<T> entity);
          T AddReturn(T entity);
          List<T> AddReturn(IEnumerable<T> entity);
          T EditReturn(T entity);
          List<T> EditReturn(IEnumerable<T> entity);
          void Delete(int id);
          void Delete(Guid id);
          void Save();
          Task SaveAsync();
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
          void Dispose();
#         pragma warning restore CS0108 // Member hides inherited member; missing new keyword
          T StoredProcedure(string storedProcName, List<StoredProcedureParam> param);
          List<T> StoredProcedureList(string storedProcName, List<StoredProcedureParam> param);
          T StoredProcedure(string storedProcName);
          List<T> StoredProcedureList(string storedProcName);
     }

     public class StoredProcedureParam {
          public string Param { get; set; }
          public string Value { get; set; }
     }
}

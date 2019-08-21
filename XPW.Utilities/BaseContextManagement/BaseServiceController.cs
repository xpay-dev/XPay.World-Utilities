using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Utilities.BaseContext;
using XPW.Utilities.CryptoHashingManagement;
using XPW.Utilities.Functions;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContextManagement {
     public abstract class BaseServiceController<S> : ApiController, IDisposable
          where S : class, new() {
          private readonly S BaseService = Activator.CreateInstance<S>();
          public new virtual void Dispose() => base.Dispose();
          public static readonly string key = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto = new HashUtilityManagement(key, iv);
          public string ErrorMessage = string.Empty;
          public List<string> ErrorDetails = new List<string>();
     }
     public abstract class BaseServiceController : ApiController, IDisposable {
          public new virtual void Dispose() => base.Dispose();
          public static readonly string key = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto = new HashUtilityManagement(key, iv);
          public string ErrorMessage = string.Empty;
          public List<string> ErrorDetails = new List<string>();
     }
     public abstract class BaseServiceController<E, C> : ApiController, IDisposable
          where E : class, new()
          where C : DbContext, new() {
          public class BaseRepo : BaseRepository<C, E>, IBaseRepo { }
          internal interface IBaseRepo : IBaseRepository<E> { }
          public class BaseServices : BaseService<E, BaseRepo> { }
          public BaseServices Service = new BaseServices();
          public static readonly string key = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto = new HashUtilityManagement(key, iv);
          public string ErrorCode = string.Empty;
          public string ErrorMessage = string.Empty;
          public List<string> ErrorDetails = new List<string>();
          public List<StoredProcedureParam> spParams = new List<StoredProcedureParam>();
          [Route("get-all")]
          [HttpGet]
          public virtual async Task<GenericResponseListModel<E>> GetAll() {
               return await Task.Run(async () => {
                    var data = new List<E>();
                    try {
                         ErrorCode = "800.1";
                         string entityName = new E().GetType().Name;
                         spParams.Add(new StoredProcedureParam {
                              Param = "TableName",
                              Value = Pluralized.Pluralize(new E().GetType().Name)
                         });
                         data = await Service.StoredProcedureList("spGetAll", spParams);
                    } catch (Exception ex) {
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                    }
                    return new GenericResponseListModel<E>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public virtual async Task<GenericResponseModel<E>> Get([FromUri]string id) {
               return await Task.Run(async () => {
                    var data = new E();
                    try {
                         ErrorCode = "800.2";
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         string entityName = new E().GetType().Name;
                         spParams.Add(new StoredProcedureParam {
                              Param = "TableName",
                              Value = Pluralized.Pluralize(new E().GetType().Name)
                         });
                         spParams.Add(new StoredProcedureParam {
                              Param = "Id",
                              Value = id.ToString()
                         });
                         var dataType = string.Empty;
                         var isGuid = Guid.TryParse(id, out guidId);
                         var isNumeric = int.TryParse(id, out intId);
                         if (isGuid) {
                              dataType = "uniqueidentifier".ToUpper();
                         } else if (isNumeric) {
                              dataType = "int".ToUpper();
                         } else {
                              ErrorCode = "800.21";
                              throw new Exception("Invalid data reference.");
                         }
                         spParams.Add(new StoredProcedureParam {
                              Param = "Type",
                              Value = dataType
                         });
                         data = await Service.StoredProcedure("spGetAll", spParams);
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                    }
                    return new GenericResponseModel<E>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public virtual async Task<GenericResponseModel<E>> Delete([FromUri]string id) {
               return await Task.Run(async () => {
                    var data = new E();
                    try {
                         ErrorCode = "800.3";
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              var crypto = new HashUtilityManagement(key, iv);
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         var isGuid = Guid.TryParse(id, out guidId);
                         var isNumeric = int.TryParse(id, out intId);
                         if (isGuid) {
                              data = await Service.Get(guidId);
                              if (data == null) {
                                   ErrorCode = "800.31";
                                   throw new Exception("Invalid data reference.");
                              }
                              await Service.DeleteAsync(guidId);
                         } else if (isNumeric) {
                              data = await Service.Get(intId);
                              if (data == null) {
                                   ErrorCode = "800.31";
                                   throw new Exception("Invalid data reference.");
                              }
                              await Service.DeleteAsync(intId);
                         } else {
                              ErrorCode = "800.32";
                              throw new Exception("Invalid data reference.");
                         }
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                    }
                    return new GenericResponseModel<E>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          public new virtual void Dispose() => base.Dispose();
     }
}


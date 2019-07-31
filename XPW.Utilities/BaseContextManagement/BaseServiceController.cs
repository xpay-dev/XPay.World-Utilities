using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Utilities.BaseContext;
using XPW.Utilities.CryptoHashingManagement;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContextManagement {
     public abstract class BaseServiceController<S> : ApiController, IDisposable 
          where S : class, new() {
          private S BaseService = Activator.CreateInstance<S>();
          public new virtual void Dispose()       => base.Dispose();
          public static readonly string key       = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv        = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto     = new HashUtilityManagement(key, iv);
          public string ErrorMessage              = string.Empty;
          public List<string> ErrorDetails        = new List<string>();
     }
     public abstract class BaseServiceController : ApiController, IDisposable {
          public new virtual void Dispose()       => base.Dispose();
          public static readonly string key       = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv        = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          public HashUtilityManagement crypto     = new HashUtilityManagement(key, iv);
          public string ErrorMessage              = string.Empty;
          public List<string> ErrorDetails        = new List<string>();
     }
     public abstract class BaseServiceController<E, C> : ApiController, IDisposable 
          where E : class, new() 
          where C : DbContext, new() {
          public class BaseRepo         : BaseRepository<C, E>, IBaseRepo { }
          internal interface IBaseRepo  : IBaseRepository<E> { }
          public class BaseServices     : BaseService<E, BaseRepo> { }
          public BaseServices Service             = new BaseServices();
          public HashUtilityManagement crypto     = new HashUtilityManagement(key, iv);
          public string ErrorCode                 = string.Empty;
          public string ErrorMessage              = string.Empty;
          public List<string> ErrorDetails        = new List<string>();
          public static readonly string key       = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          public static readonly string iv        = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          private static readonly bool SaveLog    = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"].ToString());
          [Route("get-all")]
          [HttpGet]
          public virtual async Task<GenericResponseListModel<E>> GetAll() {
               return await Task.Run(() => {
                    var data = new List<E>();
                    try {
                         data = Service.GetAll().ToList();
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
                    }
                    return new GenericResponseListModel<E>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = "800.1",
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public virtual async Task<GenericResponseModel<E>> Get([FromUri]string id) {
               return await Task.Run(() => {
                    var data = new E();
                    try {
                         ErrorCode = "800.2";
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         var isGuid     = Guid.TryParse(id, out guidId);
                         var isNumeric  = int.TryParse(id, out intId);
                         if (isGuid) {
                              data = Service.Get(guidId);
                         } else if (isNumeric) {
                              data = Service.Get(intId);
                         } else {
                              ErrorCode = "800.21";
                              throw new Exception("Invalid data reference.");
                         }
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
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
               return await Task.Run(() => {
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
                              data = Service.Get(guidId);
                                   if (data == null) {
                                   ErrorCode = "800.31";
                                   throw new Exception("Invalid data reference.");
                              }
                              Service.Delete(guidId);
                         } else if (isNumeric) {
                              data = Service.Get(intId);
                              if (data == null) {
                                   ErrorCode = "800.31";
                                   throw new Exception("Invalid data reference.");
                              }
                              Service.Delete(intId);
                         } else {
                              ErrorCode = "800.32";
                              throw new Exception("Invalid data reference.");
                         }
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
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

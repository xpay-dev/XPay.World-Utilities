using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Utilities.BaseContext;
using XPW.Utilities.CryptoHashingManagement;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContextManagement {
     public abstract class BaseServiceController<S> : ApiController, IDisposable where S : class, new() {
          private S BaseService = Activator.CreateInstance<S>();
          public new virtual void Dispose() => base.Dispose();
     }
     public abstract class BaseController : ApiController, IDisposable {
          public new virtual void Dispose() => base.Dispose();
     }
     public abstract class BaseServiceController<E, C> : ApiController, IDisposable where E : class, new() where C : DbContext, new() {
          internal class BaseRepo : BaseRepository<C, E>, IBaseRepo { }
          internal interface IBaseRepo : IBaseRepository<E> { }
          internal class BaseServices : BaseService<E, BaseRepo> { }
          private BaseServices BaseService = new BaseServices();
          internal string errorMessage = string.Empty;
          internal List<string> errorDetails = new List<string>();
          internal static readonly string key = ConfigurationManager.AppSettings["DefaultKey"].ToString();
          internal static readonly string iv = ConfigurationManager.AppSettings["DefaultIV"].ToString();
          [Route("get-all")]
          [HttpGet]
          public async Task<GenericResponseListModel<E>> GetAll() {
               return await Task.Run(() => {
                    var data = new List<E>();
                    try {
                         data = BaseService.GetAll().ToList();
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseListModel<E>() {
                         Code                = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details        = errorDetails,
                              ErrNumber      = "800.1",
                              Message        = errorMessage
                         }
                    };
               });
          }
          [Route("get/{id}")]
          [HttpGet]
          public async Task<GenericResponseModel<E>> Get(string id) {
               return await Task.Run(() => {
                    var data = new E();
                    try {
                         if(!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              var crypto = new HashUtilityManagement(key, iv);
                              id         = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         var isGuid     = Guid.TryParse(id, out guidId);
                         var isNumeric  = int.TryParse(id, out intId);
                         if (isGuid) {
                              data = BaseService.Get(guidId);
                         } else if (isNumeric) {
                              data = BaseService.Get(intId);
                         } else {
                              throw new Exception("Invalid data reference");
                         }
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<E>() {
                         Code                = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details        = errorDetails,
                              ErrNumber      = "800.2",
                              Message        = errorMessage
                         }
                    };
               });
          }
          [Route("delete/{id}")]
          [HttpDelete]
          public async Task<GenericResponseModel<E>> Delete(string id) {
               return await Task.Run(() => {
                    var data = new E();
                    try {
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              var crypto = new HashUtilityManagement(key, iv);
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         var isGuid = Guid.TryParse(id, out guidId);
                         var isNumeric = int.TryParse(id, out intId);
                         if (isGuid) {
                              data = BaseService.Get(guidId);
                              if (data == null) {
                                   throw new Exception("Invalid data reference");
                              }
                              BaseService.Delete(guidId);
                         } else if (isNumeric) {
                              data = BaseService.Get(intId);
                              if (data == null) {
                                   throw new Exception("Invalid data reference");
                              }
                              BaseService.Delete(intId);
                         } else {
                              throw new Exception("Invalid data reference");
                         }
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<E>() {
                         Code                = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details        = errorDetails,
                              ErrNumber      = "800.2",
                              Message        = errorMessage
                         }
                    };
               });
          }
          public new virtual void Dispose() => base.Dispose();
     }
}

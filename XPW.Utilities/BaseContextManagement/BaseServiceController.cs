using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Utilities.BaseContext;
using XPW.Utilities.CryptoHashingManagement;
using XPW.Utilities.Filtering;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.BaseContextManagement {
     public abstract class BaseServiceController<E, C> : ApiController where E : class, new() where C : DbContext, new() {
          public class BaseRepo : BaseRepository<C, E>, IBaseRepo { }
          public interface IBaseRepo : IBaseRepository<E> { }
          public class BaseServices : BaseService<E, BaseRepo> { }
          public BaseServices Service = new BaseServices();
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
                         data = Service.GetAll().ToList();
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
                              data = Service.Get(guidId);
                         } else if (isNumeric) {
                              data = Service.Get(intId);
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
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<E>> Save(E model) {
               return await Task.Run(() => {
                    var data = new E();
                    try {
                         data = Service.SaveReturn(model);
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<E>() {
                         Code = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details = errorDetails,
                              ErrNumber = "800.3",
                              Message = errorMessage
                         }
                    };
               });
          }
          [Route("update/{id}")]
          [HttpPut]
          [RequestFiltering]
          public async Task<GenericResponseModel<E>> Update(string id, E model) {
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
                              data = Service.Get(guidId);
                         } else if (isNumeric) {
                              data = Service.Get(intId);
                         } else {
                              throw new Exception("Invalid data reference");
                         }
                         if (data == null) {
                              throw new Exception("Invalid data reference, cannot be update");
                         }
                         data = Service.UpdateReturn(model);
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<E>() {
                         Code = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success : Enums.CodeStatus.Error,
                         CodeStatus = string.IsNullOrEmpty(errorMessage) ? Enums.CodeStatus.Success.ToString() : Enums.CodeStatus.Error.ToString(),
                         ReferenceObject = string.IsNullOrEmpty(errorMessage) ? data : null,
                         ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details = errorDetails,
                              ErrNumber = "800.4",
                              Message = errorMessage
                         }
                    };
               });
          }
     }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Filtering {
     [Serializable]
     public class RequestFiltering : ActionFilterAttribute {
          public override void OnActionExecuting(HttpActionContext actionContext) {
               var modelState = actionContext.ModelState;
               if (!modelState.IsValid) {
                    var modelErrors = new List<string>();
                    foreach (var ms in modelState) {
                         var key = ms.Key.Replace(string.Format("{0}.", ms.Key.Split('.')[0]), string.Empty);
                         var errors = ms.Value.Errors.ToList();
                         if (errors.Count > 0) {
                              var modelError = string.Empty;
                              foreach (var error in errors) {
                                   modelError += error.ErrorMessage + ", ";
                              }
                              modelError = modelError.TrimEnd(' ').TrimEnd(',').ToLower();
                              modelErrors.Add(string.Format("{0}: {1}", key, modelError));
                         } else {
                              modelErrors.Add(string.Format("{0}: cannot be null or empty", key));
                         }
                    }
                    var details = string.Empty;
                    modelErrors.ForEach(a => {
                         details += a;
                    });
                    var response = new GenericResponseModel {
                         Code = CodeStatus.InvalidInput,
                         CodeStatus = CodeStatus.InvalidInput.ToString(),
                         ErrorMessage = new ErrorMessage() {
                              ErrNumber = "700.1",
                              Details = modelErrors,
                              Message = CodeStatus.InvalidInput.ToString(),
                         }, ReferenceObject = null
                    };
                    RequestErrorLogs.Write(new RequestErrorLogModel {
                         ErrorCode = response.ErrorMessage.ErrNumber,
                         ErrorType = "RequestFiltering",
                         Message   = response.ErrorMessage.Message,
                         URLPath   = actionContext.Request.RequestUri.AbsoluteUri
                    });
                    var json = JsonConvert.SerializeObject(response);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, json);
                    actionContext.Response.Content = new StringContent(json, Encoding.UTF8, "application/json");
               }
          }
     }
}

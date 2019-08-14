using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HTTPSChecker {
     [Serializable]
     public class HTTPSValidation : AuthorizationFilterAttribute {
          internal bool Active = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveHTTPSValidation"] == null ? "false" : ConfigurationManager.AppSettings["ActiveHTTPSValidation"].ToString());
          public HTTPSValidation() { }
          public HTTPSValidation(bool active) { Active = active; }
          public override void OnAuthorization(HttpActionContext actionContext) {
               if (Active) {
                    var identity = actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps;
                    if (!identity) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(DefaultResponse.Error(actionContext), Encoding.UTF8, "application/json");
                         return;
                    }
                    base.OnAuthorization(actionContext);
               }
          }
          void Challenge(HttpActionContext actionContext) {
               _ = actionContext.Request.RequestUri.DnsSafeHost;
               actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
               actionContext.Response.Content = new StringContent(DefaultResponse.Error(actionContext), Encoding.UTF8, "application/json");
          }
          internal class DefaultResponse {
               internal static string Error(HttpActionContext actionContext) {
                    var details = new List<string> {
                         "Unauthorized access"
                    };
                    var response = new GenericResponseModel {
                         Code = CodeStatus.Unauthorized,
                         CodeStatus = CodeStatus.Unauthorized.ToString(),
                         ErrorMessage = new ErrorMessage {
                              ErrNumber = "700.4",
                              Details = details,
                              Message = HttpStatusCode.Unauthorized.ToString()
                         }, ReferenceObject = null
                    };
                    RequestErrorLogs.Write(new RequestErrorLogModel {
                         ErrorCode = response.ErrorMessage.ErrNumber,
                         ErrorType = "HTTPSValidation",
                         Message  = response.ErrorMessage.Message,
                         URLPath  = actionContext.Request.RequestUri.AbsoluteUri
                    });
                    return JsonConvert.SerializeObject(response);
               }
          }
     }
}

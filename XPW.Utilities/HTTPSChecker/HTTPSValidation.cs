using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Hosting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HTTPSChecker {
     public class HTTPSValidation : AuthorizationFilterAttribute {
          internal static readonly AppConfigManagement.AppConfig appConfigManager = new AppConfigManagement.AppConfig(HostingEnvironment.ApplicationPhysicalPath + "App_Settings", "appConfig.json");
          internal bool? Active = appConfigManager.AppSetting<bool>("RequiredHTTPSAuthorization");
          public HTTPSValidation() { }
          public HTTPSValidation(bool active) { Active = active; }
          public override void OnAuthorization(HttpActionContext actionContext) {
               if (Active.HasValue && Active.Value) {
                    var identity = actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps;
                    if (!identity) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(DefaultResponse.Error(), Encoding.UTF8, "application/json");
                         return;
                    }
                    base.OnAuthorization(actionContext);
               }
          }
          void Challenge(HttpActionContext actionContext) {
               _ = actionContext.Request.RequestUri.DnsSafeHost;
               actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
               actionContext.Response.Content = new StringContent(DefaultResponse.Error(), Encoding.UTF8, "application/json");
          }
          internal class DefaultResponse {
               internal static string Error() {
                    var details = new List<string> {
                    "Unauthorized access"
                };
                    return JsonConvert.SerializeObject(new GenericResponseModel {
                         Code = CodeStatus.Unauthorized,
                         CodeStatus = CodeStatus.Unauthorized.ToString(),
                         ErrorMessage = new ErrorMessage {
                              ErrNumber = "01",
                              Details = details,
                              Message = HttpStatusCode.Unauthorized.ToString()
                         }, ReferenceObject = null
                    });
               }
          }
     }
}

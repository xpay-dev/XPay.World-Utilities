using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.IPWhiteListing {
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
     public class IPAuthorization : AuthorizationFilterAttribute {
          internal static readonly AppConfigManagement.AppConfig appConfigManager = new AppConfigManagement.AppConfig(HostingEnvironment.ApplicationPhysicalPath + "App_Settings", "appConfig.json");
          internal bool? Active = appConfigManager.AppSetting<bool>("RequiredIPAuthorization");
          public IPAuthorization() { }
          public IPAuthorization(bool active) { Active = active; }
          public override void OnAuthorization(HttpActionContext actionContext) {
               if (Active.HasValue && Active.Value) {
                    var identity = ValidateRequestIP(actionContext);
                    if (!identity) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(DefaultResponse.Error(), Encoding.UTF8, "application/json");
                         return;
                    }
                    base.OnAuthorization(actionContext);
               }
          }
          protected virtual bool ValidateRequestIP(HttpActionContext actionContext) {
               var name = actionContext.ActionDescriptor.ActionName;
               var myRequest = ((HttpContextWrapper)actionContext.Request.Properties["MS_HttpContext"]).Request;
               for (int i = 0; i < myRequest.ServerVariables.Count; i++) {
                    System.Diagnostics.Debug.WriteLine(myRequest.ServerVariables.AllKeys[i] + " -:- " + myRequest.ServerVariables[i]);
               }
               var ip = myRequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
               var port = myRequest.ServerVariables["SERVER_PORT"];
               if (!string.IsNullOrEmpty(ip)) {
                    string[] ipRange = ip.Split(',');
                    int le = ipRange.Length - 1;
                    _ = ipRange[le];
               } else { ip = myRequest.ServerVariables["REMOTE_ADDR"]; }
               if (ip == null) { return false; }
               var registeredIp = GetRegisteredIP(ip, port);
               if (string.IsNullOrEmpty(registeredIp)) { return false; }
               return ip == registeredIp ? true : false;
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
          public string GetRegisteredIP(string ipAddress, string port) {
               try {
                    var registeredIps = Reader<IPWhiteListingModel>.JsonReaderList(HostingEnvironment.ApplicationPhysicalPath + "App_Settings\\ipWhiteList.json");
                    if (registeredIps.Count == 0) { return string.Empty; }
                    if (string.IsNullOrEmpty(ipAddress)) { throw new Exception("Value cannot be null"); }
                    if (appConfigManager.AppSetting<bool>("RequiredIPPortAuthorization")) {
                         if (string.IsNullOrEmpty(port)) {
                              throw new Exception("Value cannot be null");
                         }
                    }
                    List<IPWhiteListingModel> registeredIpAddresses = registeredIps.Where(a => a.IPAddress.Equals(ipAddress, StringComparison.CurrentCulture)).ToList();
                    if (registeredIpAddresses.Count == 0) { return string.Empty; }
                    if (registeredIps == null) { return string.Empty; }
                    if (appConfigManager.AppSetting<bool>("RequiredIPPortAuthorization")) {
                         var registeredIp = registeredIpAddresses.Where(a => a.Port.Equals(port, StringComparison.CurrentCulture)).FirstOrDefault();
                         if (registeredIp == null) {
                              return string.Empty;
                         }
                    }
                    if (!registeredIpAddresses.FirstOrDefault().IsActive) { return string.Empty; }
                    return registeredIpAddresses.FirstOrDefault().IPAddress;
               } catch {
                    return string.Empty;
               }
          }
     }
}

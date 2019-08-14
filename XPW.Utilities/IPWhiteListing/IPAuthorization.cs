using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.Logs;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.IPWhiteListing {
     [Serializable]
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
     public class IPAuthorization : AuthorizationFilterAttribute {
          internal bool Active = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveIPBlackListing"] == null ? "false" : ConfigurationManager.AppSettings["ActiveIPBlackListing"].ToString());
          public IPAuthorization() { }
          public IPAuthorization(bool active) { Active = active; }
          public override void OnAuthorization(HttpActionContext actionContext) {
               if (Active) {
                    var identity = ValidateRequestIP(actionContext);
                    if (!identity) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(DefaultResponse.Error(actionContext), Encoding.UTF8, "application/json");
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
                              ErrNumber = "700.5",
                              Details = details,
                              Message = HttpStatusCode.Unauthorized.ToString()
                         }, ReferenceObject = null
                    };
                    RequestErrorLogs.Write(new RequestErrorLogModel {
                         ErrorCode = response.ErrorMessage.ErrNumber,
                         ErrorType = "IPBlackListing",
                         Message = response.ErrorMessage.Message,
                         URLPath = actionContext.Request.RequestUri.AbsoluteUri
                    });
                    return JsonConvert.SerializeObject(response);
               }
          }
          public string GetRegisteredIP(string ipAddress, string port) {
               try {
                    var registeredIps = Reader<IPWhiteListingModel>.JsonReaderList(HostingEnvironment.ApplicationPhysicalPath + "App_Settings\\ipWhiteList.json");
                    if (registeredIps.Count == 0) { return string.Empty; }
                    if (string.IsNullOrEmpty(ipAddress)) { throw new Exception("Value cannot be null"); }
                    bool activePort = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveIPBlackListingPort"] == null ? "false" : ConfigurationManager.AppSettings["ActiveIPBlackListingPort"].ToString());
                    if (activePort) {
                         if (string.IsNullOrEmpty(port)) {
                              throw new Exception("Value cannot be null");
                         }
                    }
                    List<IPWhiteListingModel> registeredIpAddresses = registeredIps.Where(a => a.IPAddress.Equals(ipAddress, StringComparison.CurrentCulture)).ToList();
                    if (registeredIpAddresses.Count == 0) { return string.Empty; }
                    if (registeredIps == null) { return string.Empty; }
                    if (activePort) {
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

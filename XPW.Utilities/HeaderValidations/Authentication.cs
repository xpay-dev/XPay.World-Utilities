using System;
using System.Configuration;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HeaderValidations {
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
     public class Authentication : AuthorizationFilterAttribute {
          internal bool Active = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveAuthentication"] == null ? "false" : ConfigurationManager.AppSettings["ActiveAuthentication"].ToString());
          public Authentication() { }
          public Authentication(bool active) {
               Active = active;
          }
          public override void OnAuthorization(HttpActionContext actionContext) {
               if (Active) {
                    var identity = ParseAuthorizationHeader(actionContext);
                    if (identity == null) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return;
                    }
                    if (!OnAuthorizeUser(identity.Name, identity.Password, actionContext)) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return;
                    }
                    if (!AuthenticationBase64Filter.OnAuthorization(identity.Name, identity.Password, actionContext)) {
                         Challenge(actionContext);
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return;
                    }
                    var principal = new GenericPrincipal(identity, null);
                    Thread.CurrentPrincipal = principal;
                    base.OnAuthorization(actionContext);
               }
          }
          protected virtual bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext) {
               if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) {
                    Challenge(actionContext);
               }
               return true;
          }
          protected virtual BasicAuthenticationIdentity ParseAuthorizationHeader(HttpActionContext actionContext) {
               string authHeader = null;
               var auth = actionContext.Request.Headers.Authorization;
               if (auth != null && auth.Scheme == "Auth") {
                    authHeader = auth.Parameter;
               }
               if (string.IsNullOrEmpty(authHeader)) {
                    return null;
               }
               var authentication = new BaseAuthenticationModel();
               try {
                    authentication = new BaseAuthenticationServiceRepository().Get(authHeader);
                    if (authentication == null) { return null; }
               } catch { return null; }
               return new BasicAuthenticationIdentity(authentication.Username, authentication.Password);
          }
          void Challenge(HttpActionContext actionContext) {
               _ = actionContext.Request.RequestUri.DnsSafeHost;
               actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
               actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
          }
          public class BasicAuthenticationIdentity : GenericIdentity {
               public BasicAuthenticationIdentity(string name, string password) : base(name, "Auth") {
                    Password = password;
               }
               public string Password { get; set; }
          }
          public class AuthenticationBase64Filter : Authorization {
               public AuthenticationBase64Filter() { }
               public AuthenticationBase64Filter(bool active) : base(active) { }
               protected override bool OnAuthorizeUser(string username, string password, HttpActionContext actionContext) {
                    if (string.IsNullOrEmpty(username)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (string.IsNullOrEmpty(password)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    var authentication = new BaseAuthenticationModel();
                    if (authentication == null) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (username != authentication.Username) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (password != authentication.Password) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    return true;
               }
               internal static bool OnAuthorization(string username, string password, HttpActionContext actionContext) {
                    if (string.IsNullOrEmpty(username)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (string.IsNullOrEmpty(password)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    var authentication = new BaseAuthenticationModel();
                    if (authentication == null) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (username != authentication.Username) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    if (password != authentication.Password) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    return true;
               }
          }
     }
}

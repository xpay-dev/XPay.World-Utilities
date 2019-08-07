using System;
using System.Configuration;
using System.Data.Entity;
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
          internal bool Active = Convert.ToBoolean(ConfigurationManager.AppSettings["ActiveAuthorization"] == null ? "false" : ConfigurationManager.AppSettings["ActiveAuthorization"].ToString());
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
               if (auth != null && auth.Scheme == "Basic") {
                    authHeader = auth.Parameter;
               }
               if (string.IsNullOrEmpty(authHeader)) {
                    return null;
               }
               try {
                    byte[] bytes = Convert.FromBase64String(authHeader);
                    authHeader = Encoding.UTF8.GetString(bytes);
               } catch {
                    return null;
               }
               var tokens = authHeader.Split('|');
               if (tokens.Length < 2) {
                    return null;
               }
               return new BasicAuthenticationIdentity(tokens[0], tokens[1]);
          }
          void Challenge(HttpActionContext actionContext) {
               _ = actionContext.Request.RequestUri.DnsSafeHost;
               actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
               actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
          }
          public class BasicAuthenticationIdentity : GenericIdentity {
               public BasicAuthenticationIdentity(string name, string password) : base(name, "Basic") {
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
                    //if (username != authUsername) {
                    //     _ = actionContext.Request.RequestUri.DnsSafeHost;
                    //     actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    //     actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                    //     return false;
                    //}
                    if (string.IsNullOrEmpty(password)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    //if (password != authPassword) {
                    //     _ = actionContext.Request.RequestUri.DnsSafeHost;
                    //     actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    //     actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                    //     return false;
                    //}
                    return true;
               }
               internal static bool OnAuthorization(string username, string password, HttpActionContext actionContext) {
                    if (string.IsNullOrEmpty(username)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    //if (username != authUsername) {
                    //     _ = actionContext.Request.RequestUri.DnsSafeHost;
                    //     actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    //     actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                    //     return false;
                    //}
                    if (string.IsNullOrEmpty(password)) {
                         _ = actionContext.Request.RequestUri.DnsSafeHost;
                         actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                         actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                         return false;
                    }
                    //if (password != authPassword) {
                    //     _ = actionContext.Request.RequestUri.DnsSafeHost;
                    //     actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                    //     actionContext.Response.Content = new StringContent(HeaderValidationDefaults.ErrorResponse(actionContext.Request.RequestUri.AbsoluteUri), Encoding.UTF8, "application/json");
                    //     return false;
                    //}
                    return true;
               }
          }
     }
}

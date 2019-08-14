using System;
using System.Web.Http.Filters;

namespace XPW.Utilities.TokenValidationManagement {
     [Serializable]
     [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
     public class TokenUrlValidation : AuthorizationFilterAttribute {

     }
}

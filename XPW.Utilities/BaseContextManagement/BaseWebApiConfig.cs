using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Routing;

namespace XPW.Utilities.BaseContextManagement {
     public static class BaseWebApiConfig {
          public static void Register(HttpConfiguration config) {
               //config.MapHttpAttributeRoutes();
               config.MapHttpAttributeRoutes(new WebApiCustomDirectRouteProvider());

               config.Routes.MapHttpRoute(
                   name: "DefaultApi",
                   routeTemplate: "{controller}/{action}/{id}",
                   defaults: new { id = RouteParameter.Optional, token = RouteParameter.Optional }
               );

               var json = config.Formatters.JsonFormatter;
               json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
               config.Formatters.Remove(config.Formatters.XmlFormatter);
               json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
          }
     }
     public class WebApiCustomDirectRouteProvider : DefaultDirectRouteProvider {
          protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories(HttpActionDescriptor actionDescriptor) {
               return actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(inherit: true);
          }
     }
}

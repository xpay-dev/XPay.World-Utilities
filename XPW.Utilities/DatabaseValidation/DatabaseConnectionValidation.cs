using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using XPW.Utilities.Enums;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.DatabaseValidation {
     public partial class DatabaseTestConnection : DbContext {
          public DatabaseTestConnection(string testConnection)
              : base("name=" + testConnection) {
          }
     }
     public class DatabaseConnectionValidation : ActionFilterAttribute {
          private readonly string connection;
          public DatabaseConnectionValidation(string testConnection) { connection = testConnection; }
          private TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(7);
          internal Tuple<bool, string> CheckDatabaseConnection() {
               try {
                    var Context = new DatabaseTestConnection(connection);
                    Task task = Task.Run(() => {
                         if (!Context.Database.Exists()) {
                              throw new Exception("Data not exist or access denied, please contact your administration");
                         }
                    });
                    if (!task.Wait(TimeSpan)) {
                         throw new Exception("Connecting to database was taking to long, please try again later");
                    }
                    return new Tuple<bool, string>(true, string.Empty);
               } catch (Exception ex) {
                    return new Tuple<bool, string>(false, ex.Message);
               }
          }
          public override void OnActionExecuting(HttpActionContext actionContext) {
               var testConnection = CheckDatabaseConnection();
               var details = new List<string>();
               if (!testConnection.Item1) {
                    details.Add(testConnection.Item2);
                    var response = new GenericResponseModel {
                         Code = CodeStatus.DBError,
                         CodeStatus = CodeStatus.DBError.ToString(),
                         ErrorMessage = new ErrorMessage() {
                              ErrNumber = "700.2",
                              Details = details,
                              Message = CodeStatus.DBError.ToString(),
                         },
                         ReferenceObject = null
                    };
                    RequestErrorLogs.Write(new RequestErrorLogModel {
                         ErrorCode = response.ErrorMessage.ErrNumber,
                         ErrorType = "DatabaseConnectionValidation",
                         Message   = response.ErrorMessage.Message,
                         URLPath   = actionContext.Request.RequestUri.AbsoluteUri
                    });
                    var json = JsonConvert.SerializeObject(response);
                    actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.InternalServerError, json);
                    actionContext.Response.Content = new StringContent(json, Encoding.UTF8, "application/json");
               }
          }
     }
}

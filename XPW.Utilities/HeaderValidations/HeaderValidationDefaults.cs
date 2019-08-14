using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using XPW.Utilities.Enums;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HeaderValidations {
     [Serializable]
     public class HeaderValidationDefaults {
          internal static string ErrorResponse(string url) {
               var details = new List<string> {
                         "Unauthorized access"
                    };
               var response = new GenericResponseModel {
                    Code = CodeStatus.Unauthorized,
                    CodeStatus = CodeStatus.Unauthorized.ToString(),
                    ErrorMessage = new ErrorMessage {
                         ErrNumber = "700.3",
                         Details = details,
                         Message = HttpStatusCode.Unauthorized.ToString()
                    }, ReferenceObject = null
               };
               RequestErrorLogs.Write(new RequestErrorLogModel {
                    ErrorCode = response.ErrorMessage.ErrNumber,
                    ErrorType = "Authorization",
                    Message = response.ErrorMessage.Message,
                    URLPath = url
               });
               return JsonConvert.SerializeObject(response);
          }
     }
}

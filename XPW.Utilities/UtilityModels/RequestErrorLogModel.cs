using System;

namespace XPW.Utilities.UtilityModels {
     [Serializable]
     public class RequestErrorLogModel {
          public RequestErrorLogModel() {
               Id = Guid.NewGuid();
               DateCreated = DateTime.Now;
          }
          public Guid Id { get; set; }
          public string ErrorCode { get; set; }
          public string ErrorType { get; set; }
          public string URLPath { get; set; }
          public string Message { get; set; }
          public DateTime DateCreated { get; set; }
     }
}

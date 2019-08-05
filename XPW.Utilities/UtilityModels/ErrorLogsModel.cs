using System;
using XPW.Utilities.Enums;

namespace XPW.Utilities.UtilityModels {
     public class ErrorLogsModel {

          public ErrorLogsModel() {
               Id = Guid.NewGuid();
               DateCreated = DateTime.Now;
               Tag = ErrorResolution.ToDo;
          }
          public Guid Id { get; set; }
          public string ErrorType { get; set; }
          public string ErrorCode { get; set; }
          public string Message { get; set; }
          public string Application { get; set; }
          public string Controller { get; set; }
          public string Method { get; set; }
          public string CurrentAction { get; set; }
          public string SourceFile { get; set; }
          public int LineNumber { get; set; }
          public string StackTrace { get; set; }
          public DateTime DateCreated { get; set; }
          public ErrorResolution Tag { get; set; }
     }
}

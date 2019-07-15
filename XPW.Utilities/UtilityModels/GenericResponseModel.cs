using System.Collections.Generic;
using XPW.Utilities.Enums;

namespace XPW.Utilities.UtilityModels {
     public class GenericResponseModel<T> where T : class, new() {
          public CodeStatus Code { get; set; }
          public string CodeStatus { get; set; }
          public ErrorMessage ErrorMessage { get; set; }
          public T ReferenceObject { get; set; }
     }
     public class GenericResponseListModel<T> where T : class, new() {
          public CodeStatus Code { get; set; }
          public string CodeStatus { get; set; }
          public ErrorMessage ErrorMessage { get; set; }
          public List<T> ReferenceObject { get; set; }
     }
     public class GenericResponseModel {
          public CodeStatus Code { get; set; }
          public string CodeStatus { get; set; }
          public ErrorMessage ErrorMessage { get; set; }
          public object ReferenceObject { get; set; }
     }
     public class ErrorMessage {
          public string ErrNumber { get; set; }
          public string Message { get; set; }
          public List<string> Details { get; set; }
     }
}

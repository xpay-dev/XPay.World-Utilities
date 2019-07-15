using System.Collections.Generic;

namespace XPW.Utilities.UtilityModels {
     public class APICallModel {
          public APICallModel() {
               ContentType = "application/json";
               AdditionalHeaders = new List<APICallHeaderModel>();
          }
          public List<APICallHeaderModel> AdditionalHeaders { get; set; }
          public string ContentType { get; set; }
          public string ContentData { get; set; }
          public string Host { get; set; }
          public string Path { get; set; }
     }
     public class APICallHeaderModel {
          public string Name { get; set; }
          public string Value { get; set; }
     }
}

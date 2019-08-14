using System;
using System.Collections.Generic;

namespace XPW.Utilities.UtilityModels {
     [Serializable]
     public class WhiteLabelingModel {
          public WhiteLabelingModel() {
               Properties = new List<WhiteLabelingPropertyModel>();
          }
          public string Name { get; set; }
          public string ClassName { get; set; }
          public List<WhiteLabelingPropertyModel> Properties { get; set; }
     }
     [Serializable]
     public class WhiteLabelingPropertyModel {
          public string Property { get; set; }
          public string Value { get; set; }
     }
}

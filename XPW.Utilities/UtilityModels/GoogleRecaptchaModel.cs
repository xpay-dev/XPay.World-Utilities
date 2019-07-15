using System;
using System.Collections.Generic;

namespace XPW.Utilities.UtilityModels {
     public class GoogleRecaptchaModel {
          public bool success { get; set; }
          public DateTime challenge_ts { get; set; }
          public string hostname { get; set; }
          public List<string> errorcodes { get; set; }
     }
}

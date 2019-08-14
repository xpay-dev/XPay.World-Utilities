using System;

namespace XPW.Utilities.UtilityModels {
     [Serializable]
     public class IPWhiteListingModel {
          public string CompanyName { get; set; }
          public string IPAddress { get; set; }
          public string Port { get; set; }
          public bool IsActive { get; set; }
          public DateTime CreateAt { get; set; }
     }
}

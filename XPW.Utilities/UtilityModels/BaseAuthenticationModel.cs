using System;
using XPW.Utilities.Enums;

namespace XPW.Utilities.UtilityModels {
     public class BaseAuthenticationModel {
          public BaseAuthenticationModel() {
               Id = Guid.NewGuid();
               DateCreated = DateTime.Now;
          }
          public Guid Id { get; set; }
          public string Client { get; set; }
          public string Username { get; set; }
          public string Password { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
          public Status Tag { get; set; }
     }
}

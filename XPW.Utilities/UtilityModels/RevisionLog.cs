using System;
using XPW.Utilities.Enums;

namespace XPW.Utilities.UtilityModels {
     public class RevisionLog<T> where T : class, new() {
          public RevisionLog() {
               DateCreated = DateTime.Now;
          }
          public string Context { get; set; }
          public string Entity { get; set; }
          public T Revisions { get; set; }
          public RevisionType RevisionType { get; set; }
          public DateTime DateCreated { get; set; }
     }
}

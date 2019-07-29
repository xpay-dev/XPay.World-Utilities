using System.Collections.Generic;
using XPW.Utilities.Enums;

namespace XPW.Utilities.UtilityModels {
     public class RevisionLog<T> where T : class, new() {
          public string Context { get; set; }
          public string Entity { get; set; }
          public List<T> Revisions { get; set; }
          public RevisionType RevisionType { get; set; }
     }
}

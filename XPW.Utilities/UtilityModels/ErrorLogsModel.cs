using XPW.Utilities.BaseContext;

namespace XPW.Utilities.UtilityModels {
     public class UserErrorLogModel : BaseModelError {
     }
     public class SupportErrorLogModel : BaseModelError { 
     }
     public class SystemErrorLogModel : BaseModelError {
          public int LineNumber { get; set; }
     }
}

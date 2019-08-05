using System;

namespace XPW.Utilities.CustomExceptions {

     [Serializable]
     public class RequestFilteringException : Exception {
          public RequestFilteringException() { }
          public RequestFilteringException(string message) : base(string.Format("Error at : {0}", message)) { }
     }
}

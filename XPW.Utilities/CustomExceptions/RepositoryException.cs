using System;

namespace XPW.Utilities.CustomExceptions {

     [Serializable]
     public class RepositoryException : Exception {
          public RepositoryException() { }
          public RepositoryException(string message) : base(string.Format("Error at : {0}", message)) { }
     }
}

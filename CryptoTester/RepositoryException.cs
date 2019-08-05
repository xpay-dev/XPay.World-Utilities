using System;
using System.Runtime.Serialization;

namespace CryptoTester {
     [Serializable]
     internal class RepositoryException : Exception {
          private string v1;
          private string v2;
          private string v3;
          private string name;

          public RepositoryException() {
          }

          public RepositoryException(string message) : base(message) {
          }

          public RepositoryException(string message, Exception innerException) : base(message, innerException) {
          }

          public RepositoryException(string v1, string v2, string v3, string name) {
               this.v1 = v1;
               this.v2 = v2;
               this.v3 = v3;
               this.name = name;
          }

          protected RepositoryException(SerializationInfo info, StreamingContext context) : base(info, context) {
          }
     }
}
using System;
using System.Configuration;
using System.Diagnostics;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.CustomExceptions {
     [Serializable]
     public class RepositoryException : Exception {
          public RepositoryException() { }
          public RepositoryException(string errorCode, string errorType, string message, string errorbase) : base(string.Format("Error at : {0}", message)) {
               bool saveError  = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveError"] == null ? "false" : ConfigurationManager.AppSettings["SaveError"].ToString());
               int lineNumber  = 0;
               var st          = new StackTrace(this, true);
               var frame       = st.GetFrame(0);
               lineNumber      = frame.GetFileLineNumber();
               string fileName = errorbase + DateTime.Now.ToString("HH-mm-ss") + ".json";
               _ = ErrorLogs<SystemErrorLogModel>.Write(new SystemErrorLogModel {
                    ErrorCode  = errorCode,
                    ErrorType  = errorType,
                    LineNumber = lineNumber,
                    Message    = message
               }, fileName);
          }
     }
}

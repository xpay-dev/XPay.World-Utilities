using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public class ErrorLogs {
          public static string Write(ErrorLogsModel log, Exception exception) {
               string fileName = DateTime.Now.ToString("HHmmss") + ".json";
               try {
                    StackTrace trace    = new StackTrace(exception, true);
                    string sourceFile   = trace.GetFrame(0).GetFileName();
                    int lineNumber      = trace.GetFrame(0).GetFileLineNumber();
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Errors" + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    log.SourceFile = sourceFile;
                    log.LineNumber = lineNumber;
                    log.StackTrace = exception.ToString();
                    List<ErrorLogsModel> logs = Reader<ErrorLogsModel>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<ErrorLogsModel>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<ErrorLogsModel>();
                    }
                    logs.Add(log);
                    return Writer<ErrorLogsModel>.JsonWriterList(logs, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    var message = ex.Message + st + "=========" + line;
                    throw ex;
               }
          }
          public static List<ErrorLogsModel> Read(string fileName) {
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Errors" + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    List<ErrorLogsModel> logs = Reader<ErrorLogsModel>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<ErrorLogsModel>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<ErrorLogsModel>();
                    }
                    return logs;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }
}

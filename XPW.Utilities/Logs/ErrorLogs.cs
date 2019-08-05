using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using XPW.Utilities.BaseContext;
using XPW.Utilities.NoSQL;

namespace XPW.Utilities.Logs {
     public class ErrorLogs<T> where T : BaseModelError, new() {
          public static string Write(T log, string fileName) {
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
                    List<T> logs = Reader<T>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<T>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<T>();
                    }
                    logs.Add(log);
                    return Writer<T>.JsonWriterList(logs, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    var message = ex.Message + st + "=========" + line;
                    throw ex;
               }
          }
          public static List<T> Read(string fileName) {
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
                    List<T> logs = Reader<T>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<T>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<T>();
                    }
                    return logs;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;

namespace XPW.Utilities.Logs {
     public class ErrorLogs<T> where T : class, new() {
          public static string Write(ErrorLogs<T> log, string errorType, string fileName) {
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Errors" + "\\" + errorType + "s\\" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    List<ErrorLogs<T>> logs = Reader<ErrorLogs<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<ErrorLogs<T>>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<ErrorLogs<T>>();
                    }
                    logs.Add(log);
                    return Writer<ErrorLogs<T>>.JsonWriterList(logs, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static List<ErrorLogs<T>> Read(string errorType, string fileName) {
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Errors" + "\\" + errorType + "s\\" + DateTime.Now.ToString("MM-dd-yyyy");
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    List<ErrorLogs<T>> logs = Reader<ErrorLogs<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<ErrorLogs<T>>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<ErrorLogs<T>>();
                    }
                    return logs;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }
}

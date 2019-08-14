using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     [Serializable]
     public static class ErrorLogs {
          public static string Write(ErrorLogsModel log, Exception exception) {
               if (log is null) {
                    return "Done";
               }
               if (exception is null) {
                    return "Done";
               }
               bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveError"] == null ? "false" : ConfigurationManager.AppSettings["SaveError"].ToString());
               if (!saveError) {
                    return "Done";
               }
               string fileName = DateTime.Now.ToString("HH") + ".json";
               string logName = DateTime.Now.ToString("HH") + ".log";
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
                    if (!File.Exists(FileLocation + "\\" + logName)) {
                         FileStream file = File.Create(FileLocation + "\\" + logName);
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
                    using (StreamWriter sw = File.AppendText(FileLocation + "\\" + logName)) {
                         sw.Write("[" + log.DateCreated + " -> " + log.Id + "]");
                         sw.Write(" Application : " + log.Application + "]");
                         sw.Write(" Controller : " + log.Controller + "]");
                         sw.Write(" Method : " + log.Method + "]");
                         sw.Write(" Action : " + log.CurrentAction + "]");
                         sw.Write(" ErrorCode : " + log.ErrorCode + "]");
                         sw.Write(" Message : " + log.Message + "]");
                         sw.Write(" SourceFile : " + log.SourceFile + "]");
                         sw.Write(" LineNumber : " + log.LineNumber + "]");
                         sw.WriteLine("StackTrace : " + log.StackTrace + "]");
                         sw.WriteLine("\n");
                    }
                    logs.Add(log);
                    return Writer<ErrorLogsModel>.JsonWriterList(logs, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static List<ErrorLogsModel> Read(string fileName) {
               if (string.IsNullOrEmpty(fileName)) {
                    return new List<ErrorLogsModel>();
               }
               bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveError"] == null ? "false" : ConfigurationManager.AppSettings["SaveError"].ToString());
               if (!saveError) {
                    return new List<ErrorLogsModel>();
               }
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

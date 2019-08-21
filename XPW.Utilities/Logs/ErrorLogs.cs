using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     [Serializable]
     public static class ErrorLogs {
          public static async Task Write(ErrorLogsModel log, Exception exception) {
               await Task.Run(() => { 
                    if (log is null) {
                         return;
                    }
                    if (exception is null) {
                         return;
                    }
                    bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveError"] == null ? "false" : ConfigurationManager.AppSettings["SaveError"].ToString());
                    if (!saveError) {
                         return;
                    }
                    string logName = DateTime.Now.ToString("HH") + ".log";
                    try {
                         string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Errors" + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
                         if (!Directory.Exists(FileLocation)) {
                              Directory.CreateDirectory(FileLocation);
                         }
                         if (!File.Exists(FileLocation + "\\" + logName)) {
                              FileStream file = File.Create(FileLocation + "\\" + logName);
                              file.Close();
                              file.Dispose();
                         }
                         using (StreamWriter sw = File.AppendText(FileLocation  + "\\" + logName)) {
                              sw.Write("[" + log.DateCreated + " -> " + log.Id  + "]\t");
                              sw.Write(" Application   : " + log.Application    + "\t");
                              sw.Write(" Controller    : " + log.Controller     + "\t");
                              sw.Write(" Method        : " + log.Method         + "\t");
                              sw.Write(" Action        : " + log.CurrentAction  + "\t");
                              sw.Write(" ErrorCode     : " + log.ErrorCode      + "\t");
                              sw.Write(" Message       : " + log.Message        + "\t");
                              sw.Write(" SourceFile    : " + log.SourceFile     + "\t");
                              sw.Write(" LineNumber    : " + log.LineNumber     + "\t");
                              sw.WriteLine("StackTrace : " + log.StackTrace     + "\t");
                              sw.WriteLine("\n=====");
                         }
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          //To Change
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

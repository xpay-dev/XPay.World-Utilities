using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     [Serializable]
     public static class RequestErrorLogs {
          public static async Task WriteAsync(RequestErrorLogModel log) {
               await Task.Run(() => { 
                    if (log is null) {
                         return;
                    }
                    bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRequestError"] == null ? "false" : ConfigurationManager.AppSettings["SaveRequestError"].ToString());
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
                              sw.Write("[" + log.DateCreated + " -> " + log.Id  + "]");
                              sw.Write(" Action        : " + log.ErrorCode      + "]");
                              sw.Write(" ErrorCode     : " + log.ErrorType      + "]");
                              sw.Write(" Message       : " + log.Message        + "]");
                              sw.Write(" SourceFile    : " + log.URLPath        + "]");
                              sw.WriteLine("\n");
                         }
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public static void Write(RequestErrorLogModel log) {
               if (log is null) {
                    return;
               }
               bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRequestError"] == null ? "false" : ConfigurationManager.AppSettings["SaveRequestError"].ToString());
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
                    using (StreamWriter sw = File.AppendText(FileLocation + "\\" + logName)) {
                         sw.Write("[" + log.DateCreated + " -> " + log.Id + "]");
                         sw.Write(" Action        : " + log.ErrorCode + "]");
                         sw.Write(" ErrorCode     : " + log.ErrorType + "]");
                         sw.Write(" Message       : " + log.Message + "]");
                         sw.Write(" SourceFile    : " + log.URLPath + "]");
                         sw.WriteLine("\n=====");
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          //To Change
          public static List<RequestErrorLogModel> Read(string fileName) {
               if (string.IsNullOrEmpty(fileName)) {
                    return new List<RequestErrorLogModel>();
               }
               bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveError"] == null ? "false" : ConfigurationManager.AppSettings["SaveError"].ToString());
               if (!saveError) {
                    return new List<RequestErrorLogModel>();
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
                    List<RequestErrorLogModel> logs = Reader<RequestErrorLogModel>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (logs == null) {
                         logs = new List<RequestErrorLogModel>();
                    }
                    if (logs.Count == 0) {
                         logs = new List<RequestErrorLogModel>();
                    }
                    return logs;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }
}

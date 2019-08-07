using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public static class RequestErrorLogs {
          public static string Write(RequestErrorLogModel log) {
               if (log is null) {
                    return "Done";
               }
               bool saveError = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRequestError"] == null ? "false" : ConfigurationManager.AppSettings["SaveRequestError"].ToString());
               if (!saveError) {
                    return "Done";
               }
               string fileName = DateTime.Now.ToString("HH") + ".json";
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
                    logs.Add(log);
                    return Writer<RequestErrorLogModel>.JsonWriterList(logs, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }
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

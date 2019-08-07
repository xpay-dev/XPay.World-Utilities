using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public static class RevisionLogs<T> where T : class, new() {
          public static async Task<string> Write(RevisionLog<T> log, string contextName, string fileName) {
               return await Task.Run(() => {
                    if (log is null) {
                         return "Done";
                    }
                    if (string.IsNullOrEmpty(contextName)) {
                         return "Done";
                    }
                    if (string.IsNullOrEmpty(fileName)) {
                         return "Done";
                    }
                    bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
                    if (!saveRevision) {
                         return "Done";
                    }
                    try {
                         string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName;
                         if (!Directory.Exists(FileLocation)) {
                              Directory.CreateDirectory(FileLocation);
                         }
                         if (!File.Exists(FileLocation + "\\" + fileName)) {
                              FileStream file = File.Create(FileLocation + "\\" + fileName);
                              file.Close();
                              file.Dispose();
                         }
                         List<RevisionLog<T>> logs = Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                         if (logs == null) {
                              logs = new List<RevisionLog<T>>();
                         }
                         if (logs.Count == 0) {
                              logs = new List<RevisionLog<T>>();
                         }
                         logs.Add(log);
                         return Writer<RevisionLog<T>>.JsonWriterList(logs, FileLocation + "\\" + fileName);
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public static async Task<List<RevisionLog<T>>> Read(string contextName, string fileName) {
               return await Task.Run(() => {
                    if (string.IsNullOrEmpty(contextName)) {
                         return new List<RevisionLog<T>>();
                    }
                    if (string.IsNullOrEmpty(fileName)) {
                         return new List<RevisionLog<T>>();
                    }
                    bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
                    if (!saveRevision) {
                         return new List<RevisionLog<T>>();
                    }
                    try {
                         string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName;
                         if (!Directory.Exists(FileLocation)) {
                              Directory.CreateDirectory(FileLocation);
                         }
                         if (!File.Exists(FileLocation + "\\" + fileName)) {
                              FileStream file = File.Create(FileLocation + "\\" + fileName);
                              file.Close();
                              file.Dispose();
                         }
                         List<RevisionLog<T>> logs = Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                         if (logs == null) {
                              logs = new List<RevisionLog<T>>();
                         }
                         if (logs.Count == 0) {
                              logs = new List<RevisionLog<T>>();
                         }
                         return logs;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
     }
}

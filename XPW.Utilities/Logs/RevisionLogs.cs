using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using XPW.Utilities.Functions;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public static class RevisionLogs<T> where T : class, new() {
          public static string Write(RevisionLog<T> log, string contextName, string fileName) {
               bool saveRevision        = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
               if (!saveRevision) {
                    return "Done";
               }
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName;
                    if (!Directory.Exists(FileLocation)) {
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString())) {
                              return "Done";
                         }
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions")) {
                              return "Done";
                         }
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName)) {
                              return "Done";
                         }
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
                    var st = new StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    var line = frame.GetFileLineNumber();
                    var message = ex.Message + st + "=========" + line;
                    throw ex;
               }
          }
          public static List<RevisionLog<T>> Read(string contextName, string fileName) {
               bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
               if (!saveRevision) {
                    return new List<RevisionLog<T>>();
               }
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName;
                    if (!Directory.Exists(FileLocation)) {
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString())) {
                              return new List<RevisionLog<T>>();
                         }
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions")) {
                              return new List<RevisionLog<T>>();
                         }
                         if (!Checker.CheckFolderPermission(ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName)) {
                              return new List<RevisionLog<T>>();
                         }
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
          }
     }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public class RevisionLogs<T> where T : class, new() {
          public static string Write(RevisionLog<T> log, string contextName, string fileName) {
               bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
               if (saveRevision) {
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
               } return "Done";
          }
          public static List<RevisionLog<T>> Read(string contextName, string fileName) {
               bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
               if (saveRevision) {
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
               } return new List<RevisionLog<T>>();
          }
     }
}

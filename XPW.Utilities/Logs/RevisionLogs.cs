using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public class RevisionLogs<T> where T : class, new() {
          public static string Write(RevisionLog<T> log, string fileName) {
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revision";
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    List<RevisionLog<T>> revisions = Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (revisions == null) {
                         revisions = new List<RevisionLog<T>>();
                    }
                    if (revisions.Count == 0) {
                         revisions = new List<RevisionLog<T>>();
                    }
                    revisions.Add(log);
                    return Writer<RevisionLog<T>>.JsonWriterList(revisions, FileLocation + "\\" + fileName);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static List<RevisionLog<T>> Read(string fileName) {
               try {
                    string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revision";
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + fileName)) {
                         FileStream file = File.Create(FileLocation + "\\" + fileName);
                         file.Close();
                         file.Dispose();
                    }
                    List<RevisionLog<T>> revisions = Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
                    if (revisions == null) {
                         revisions = new List<RevisionLog<T>>();
                    }
                    if (revisions.Count == 0) {
                         revisions = new List<RevisionLog<T>>();
                    }
                    return revisions;
               } catch (Exception ex) {
                    throw ex;
               }
          }
     }
}

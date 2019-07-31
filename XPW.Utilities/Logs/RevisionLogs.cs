using System.Collections.Generic;
using System.Configuration;
using System.IO;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public class RevisionLogs<T> where T : class, new() {
          public static string Write(RevisionLog<T> log, string fileName) {
               string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revision";
               if (!Directory.Exists(FileLocation)) {
                    Directory.CreateDirectory(FileLocation);
               }
               if (!File.Exists(FileLocation + "\\" + fileName)) {
                    var file = File.Create(FileLocation + "\\" + fileName);
                    file.Close();
                    file.Dispose();
               }
               var revisions =  Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
               revisions.Add(log);
               return Writer<RevisionLog<T>>.JsonWriterList(revisions, FileLocation + "\\" + fileName);
          }
          public static List<RevisionLog<T>> Read(string fileName) {
               string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revision";
               if (!Directory.Exists(FileLocation)) {
                    Directory.CreateDirectory(FileLocation);
               }
               if (!File.Exists(FileLocation + "\\" + fileName)) {
                    var file = File.Create(FileLocation + "\\" + fileName);
                    file.Close();
                    file.Dispose();
               }
               return Reader<RevisionLog<T>>.JsonReaderList(FileLocation + "\\" + fileName);
          }
     }
}

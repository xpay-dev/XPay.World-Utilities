using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     [Serializable]
     public static class RevisionLogs<T> where T : class, new() {
          public static async Task Write(RevisionLog<T> log, string contextName, string fileName, T entity) {
               await Task.Run(() => {
                    if (log is null) {
                         return;
                    }
                    if (string.IsNullOrEmpty(contextName)) {
                         return;
                    }
                    if (string.IsNullOrEmpty(fileName)) {
                         return;
                    }
                    bool saveRevision = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"] == null ? "false" : ConfigurationManager.AppSettings["SaveRevision"].ToString());
                    if (!saveRevision) {
                         return;
                    }
                    try {
                         string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
                         if (!Directory.Exists(FileLocation)) {
                              Directory.CreateDirectory(FileLocation);
                         }
                         if (!File.Exists(FileLocation + "\\" + fileName)) {
                              FileStream file = File.Create(FileLocation + "\\" + fileName);
                              file.Close();
                              file.Dispose();
                         }
                         PropertyInfo[] properties = typeof(T).GetProperties();
                         using (StreamWriter sw = File.AppendText(FileLocation + "\\" + fileName)) {
                              string toWrite = string.Empty;
                              sw.Write("{ ");
                              toWrite += "\"Context     \" : " + (log.Context == null ? "null" + "," : "\"" + log.Context.ToString() + "\",") + "\t";
                              toWrite += "\"Entity      \" : " + (log.Entity == null ? "null" + "," : "\"" + log.Entity.ToString() + "\",") + "\t";
                              toWrite += "\"RevisionType\" : " +"\"" + log.RevisionType.ToString() + "\"," + "\t";
                              toWrite += "\"DateCreated \" : " + (log.DateCreated == null ? "null" + "," : "\"" + log.DateCreated.ToString() + "\",") + "\t";
                              toWrite += "\"Revisions   \" : ";
                              toWrite += "{ ";
                              foreach (PropertyInfo property in properties) {
                                   toWrite += "\"" + property.Name + "\" : " + (property.GetValue(entity, null) == null ? "null" + "," : "\"" + property.GetValue(entity, null).ToString() + "\",") + "\t";
                              }
                              toWrite = toWrite.Trim().TrimEnd(',');
                              toWrite += "}";
                              toWrite = toWrite.Trim().TrimEnd(',');
                              sw.Write(toWrite);
                              sw.Write("}");
                              sw.WriteLine("\n");
                         }
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
                         string FileLocation = ConfigurationManager.AppSettings["LogLocation"].ToString() + "\\" + "Revisions" + "\\" + contextName + "\\" + DateTime.Now.ToString("MM-dd-yyyy");
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

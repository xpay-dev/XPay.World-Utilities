using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XPW.Utilities.NoSQL {
     [Serializable]
     public class Reader<T> where T : class, new() {
          public static List<T> JsonReaderList(string file) {
               try {
                    if (!File.Exists(file)) {
                         throw new Exception("File not exist");
                    }
                    using (StreamReader r = new StreamReader(file)) {
                         string json = r.ReadToEnd();
                         var items = JsonConvert.DeserializeObject<List<T>>(json);
                         r.Close();
                         r.Dispose();
                         return items;
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static T JsonReader(string file) {
               try {
                    if (!File.Exists(file)) {
                         throw new Exception("File not exist");
                    }
                    using (StreamReader r = new StreamReader(file)) {
                         string json = r.ReadToEnd();
                         var items = JsonConvert.DeserializeObject<T>(json);
                         r.Close();
                         r.Dispose();
                         return items;
                    }
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static List<string[]> CSVReader(string file) {
               List<string[]> csvData = new List<string[]>();
               try {
                    if (!File.Exists(file)) {
                         throw new Exception("File not exist");
                    }
                    var reader = new StreamReader(File.OpenRead(file));
                    while (!reader.EndOfStream) {
                         var line = reader.ReadLine();
                         if (!string.IsNullOrEmpty(line)) {
                              var data = line.Split(',');
                              csvData.Add(data);
                         }
                    }
                    reader.Close();
                    reader.Dispose();
                    return csvData;
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static async Task<List<T>> JsonReaderListAsync(string file) {
               return await Task.Run(() => {
                    try {
                         if (!File.Exists(file)) {
                              throw new Exception("File not exist");
                         }
                         using (StreamReader r = new StreamReader(file)) {
                              string json = r.ReadToEnd();
                              var items = JsonConvert.DeserializeObject<List<T>>(json);
                              r.Close();
                              r.Dispose();
                              return items;
                         }
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public static async Task<T> JsonReaderAsync(string file) {
               return await Task.Run(() => {
                    try {
                         if (!File.Exists(file)) {
                              throw new Exception("File not exist");
                         }
                         using (StreamReader r = new StreamReader(file)) {
                              string json = r.ReadToEnd();
                              var items = JsonConvert.DeserializeObject<T>(json);
                              r.Close();
                              r.Dispose();
                              return items;
                         }
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public static async Task<List<string[]>> CSVReaderAsync(string file) {
               return await Task.Run(() => {
                    List<string[]> csvData = new List<string[]>();
                    try {
                         if (!File.Exists(file)) {
                              throw new Exception("File not exist");
                         }
                         var reader = new StreamReader(File.OpenRead(file));
                         while (!reader.EndOfStream) {
                              var line = reader.ReadLine();
                              if (!string.IsNullOrEmpty(line)) {
                                   var data = line.Split(',');
                                   csvData.Add(data);
                              }
                         }
                         reader.Close();
                         reader.Dispose();
                         return csvData;
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
     }
}

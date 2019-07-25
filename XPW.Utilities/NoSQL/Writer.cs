using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace XPW.Utilities.NoSQL {
     public class Writer<T> where T : class, new() {
          public static string JsonWriter(T entity, string path) {
               try {
                    StreamWriter readFile = new StreamWriter(path);
                    readFile.Close();
                    readFile.Dispose();
                    File.Delete(path);
                    using (StreamWriter outputFile = new StreamWriter(path, false)) {
                         string line = JsonConvert.SerializeObject(entity);
                         outputFile.WriteLine(line);
                         outputFile.Close();
                         outputFile.Dispose();
                    }
                    return "Done";
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static string JsonWriterList(List<T> entity, string path) {
               try {
                    StreamWriter readFile = new StreamWriter(path);
                    readFile.Close();
                    readFile.Dispose();
                    File.Delete(path);
                    using (StreamWriter outputFile = new StreamWriter(path, false)) {
                         string line = JsonConvert.SerializeObject(entity);
                         outputFile.WriteLine(line);
                         outputFile.Close();
                         outputFile.Dispose();
                    }
                    return "Done";
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public static async Task<string> JsonWriterAsync(T entity, string path) {
               return await Task.Run(() => { 
                    try {
                         StreamWriter readFile = new StreamWriter(path);
                         readFile.Close();
                         readFile.Dispose();
                         File.Delete(path);
                         using (StreamWriter outputFile = new StreamWriter(path, false)) {
                              string line = JsonConvert.SerializeObject(entity);
                              outputFile.WriteLine(line);
                              outputFile.Close();
                              outputFile.Dispose();
                         }
                         return "Done";
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
          public static async Task<string> JsonWriterListAsync(List<T> entity, string path) {
               return await Task.Run(() => {
                         try {
                         StreamWriter readFile = new StreamWriter(path);
                         readFile.Close();
                         readFile.Dispose();
                         File.Delete(path);
                         using (StreamWriter outputFile = new StreamWriter(path, false)) {
                              string line = JsonConvert.SerializeObject(entity);
                              outputFile.WriteLine(line);
                              outputFile.Close();
                              outputFile.Dispose();
                         }
                         return "Done";
                    } catch (Exception ex) {
                         throw ex;
                    }
               });
          }
     }
}

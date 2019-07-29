using System.Configuration;
using System.IO;
using System.Threading.Tasks;
using System.Web.Hosting;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.Logs {
     public class RevisionLogs<T> where T : class, new() {
          private static string FileLocation { get; set; }
          public RevisionLogs() {
               FileLocation   = string.Format("{0}//{1}//{2}", HostingEnvironment.ApplicationPhysicalPath, ConfigurationManager.AppSettings["LogLocation"].ToString(), "Revision");
          }
          public static async Task<string> WriteAsync(RevisionLog<T> log, string fileName) {
               if (!File.Exists(string.Format("{0}//{1}", FileLocation, fileName))) {
                    File.Create(string.Format("{0}//{1}", FileLocation, fileName));
               }
               return await Task.Run(() => {
                    return Writer<RevisionLog<T>>.JsonWriterAsync(log, string.Format("{0}/{1}", FileLocation, fileName));
               });
          }
          public static async Task<RevisionLog<T>> ReadAsync(string fileName) {
               if (!File.Exists(string.Format("{0}//{1}", FileLocation, fileName))) {
                    File.Create(string.Format("{0}//{1}", FileLocation, fileName));
               }
               return await Task.Run(() => {
                    return Reader<RevisionLog<T>>.JsonReaderAsync(string.Format("{0}/{1}", FileLocation, fileName));
               });
          }
          public static string Write(RevisionLog<T> log, string fileName) {
               if (!File.Exists(string.Format("{0}//{1}", FileLocation, fileName))) {
                    File.Create(string.Format("{0}//{1}", FileLocation, fileName));
               }
               return Writer<RevisionLog<T>>.JsonWriter(log, string.Format("{0}/{1}", FileLocation, fileName));
          }
          public static RevisionLog<T> Read(string fileName) {
               if (!File.Exists(string.Format("{0}//{1}", FileLocation, fileName))) {
                    File.Create(string.Format("{0}//{1}", FileLocation, fileName));
               }
               return Reader<RevisionLog<T>>.JsonReader(string.Format("{0}/{1}", FileLocation, fileName));
          }
     }
}

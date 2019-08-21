using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using XPW.Utilities.Logs;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.AppConfigManagement {
     [Serializable]
     public class AppConfig {
          private static string FileLocation = string.Empty;
          internal List<AppConfigSettingsModel> appConfigSettings = new List<AppConfigSettingsModel>();
          private string FileName { get; set; }
          public AppConfig(string location, string file) {
               FileLocation = location;
               FileName = file;
          }
          public async Task<TValue> AppSettingAsync<TValue>(string key, bool autoCreate = false, AppConfigSettingsModel value = null, bool requiredException = false) {
               return await Task.Run(async () => { 
               try {
                         if (!Directory.Exists(FileLocation)) {
                              Directory.CreateDirectory(FileLocation);
                         }
                         if (!File.Exists(FileLocation + "\\" + FileName)) {
                              var configFile = File.Create(FileLocation + "\\" + FileName);
                              configFile.Close();
                              configFile.Dispose();
                         }
                         appConfigSettings = Reader<AppConfigSettingsModel>.JsonReaderList(FileLocation + "\\" + FileName);
                         if (appConfigSettings == null) {
                              appConfigSettings = new List<AppConfigSettingsModel>();
                         }
                         StringValueChecker(key);
                         AppConfigSettingsModel appSetting = appConfigSettings.Where(a => a.Name.Equals(key, StringComparison.CurrentCulture)).FirstOrDefault();
                         if (appSetting == null) {
                              if (autoCreate) {
                                   value.Name = key;
                                   appConfigSettings.Add(value);
                                   _ = Writer<AppConfigSettingsModel>.JsonWriterList(appConfigSettings, FileLocation + "\\" + FileName);
                                   appSetting = value;
                              } else {
                                   throw new Exception("No Application Settings Found");
                              }
                         }
                         return (TValue)Convert.ChangeType(appSetting.Value, typeof(TValue));
                    } catch (Exception ex) {
                         if (!requiredException) {
                              return (TValue)Convert.ChangeType(null, typeof(TValue));
                         }
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application = "Utilities",
                              Message     = "Error on writing files",
                              StackTrace  = ex.ToString()
                         }, ex);
                         throw ex;
                    }
               });
          }
          public TValue AppSetting<TValue>(string key, bool autoCreate = false, AppConfigSettingsModel value = null, bool requiredException = false) {
               try {
                    if (!Directory.Exists(FileLocation)) {
                         Directory.CreateDirectory(FileLocation);
                    }
                    if (!File.Exists(FileLocation + "\\" + FileName)) {
                         var configFile = File.Create(FileLocation + "\\" + FileName);
                         configFile.Close();
                         configFile.Dispose();
                    }
                    appConfigSettings = Reader<AppConfigSettingsModel>.JsonReaderList(FileLocation + "\\" + FileName);
                    if (appConfigSettings == null) {
                         appConfigSettings = new List<AppConfigSettingsModel>();
                    }
                    StringValueChecker(key);
                    AppConfigSettingsModel appSetting = appConfigSettings.Where(a => a.Name.Equals(key, StringComparison.CurrentCulture)).FirstOrDefault();
                    if (appSetting == null) {
                         if (autoCreate) {
                              value.Name = key;
                              appConfigSettings.Add(value);
                              _ = Writer<AppConfigSettingsModel>.JsonWriterList(appConfigSettings, FileLocation + "\\" + FileName);
                              appSetting = value;
                         } else {
                              throw new Exception("No Application Settings Found");
                         }
                    }
                    return (TValue)Convert.ChangeType(appSetting.Value, typeof(TValue));
               } catch (Exception ex) {
                    if (!requiredException) {
                         return (TValue)Convert.ChangeType(null, typeof(TValue));
                    }
                    _ = ErrorLogs.Write(new ErrorLogsModel {
                         Application = "Utilities",
                         Message = "Error on writing files",
                         StackTrace = ex.ToString()
                    }, ex);
                    throw ex;
               }
          }
          private static void StringValueChecker(string stringValue) {
               if (string.IsNullOrEmpty(stringValue)) {
                    throw new Exception("Value cannot be null");
               }
          }
     }
}

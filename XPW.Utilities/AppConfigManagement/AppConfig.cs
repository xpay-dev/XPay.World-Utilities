using System;
using System.Collections.Generic;
using System.Linq;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.AppConfigManagement {
     public class AppConfig {
          private static string fileLocation = string.Empty;
          internal List<AppConfigSettingsModel> appConfigSettings = new List<AppConfigSettingsModel>();
          public AppConfig(string location, string file) {
               fileLocation = location;
               appConfigSettings = Reader<AppConfigSettingsModel>.JsonReaderList(fileLocation + "\\" + file);
          }
          public TValue AppSetting<TValue>(string key, bool requiredException = false) {
               try {
                    StringValueChecker(key);
                    AppConfigSettingsModel appSetting = appConfigSettings.Where(a => a.Name.Equals(key, StringComparison.CurrentCulture)).FirstOrDefault();
                    if (appSetting == null) {
                         throw new Exception("No Application Settings Found");
                    }
                    return (TValue)Convert.ChangeType(appSetting.Value, typeof(TValue));
               } catch (Exception ex) {
                    if (!requiredException) {
                         return (TValue)Convert.ChangeType(null, typeof(TValue));
                    }
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

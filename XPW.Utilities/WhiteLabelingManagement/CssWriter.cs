using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.WhiteLabelingManagement {
     [Serializable]
     public class CssWriter {
          private static string css = string.Empty;
          private static string json = string.Empty;
          private static string directory = string.Empty;
          public CssWriter(string cssName, string dir) {
               css = cssName + ".css";
               json = cssName + ".json";
               directory = dir;
          }
          public string CssFileWriter(List<WhiteLabelingModel> cssObject) {
               try {
                    string fileName = directory + "\\" + css;
                    if (string.IsNullOrEmpty(directory)) {
                         throw new Exception("No directory is Set");
                    }
                    if (string.IsNullOrEmpty(css)) {
                         throw new Exception("No Css File Name is Set");
                    }
                    if (!Directory.Exists(directory)) {
                         Directory.CreateDirectory(directory);
                    }
                    if (!File.Exists(fileName)) {
                         File.Create(fileName).Dispose();
                    }
                    using (StreamWriter file = new StreamWriter(fileName)) {
                         StringBuilder cssLog = new StringBuilder();
                         cssObject.ForEach(a => {
                              cssLog.AppendLine("." + a.ClassName + " { ");
                              a.Properties.ForEach(b => {
                                   cssLog.AppendLine(b.Property + " : " + b.Value + " !important;");
                              });
                              cssLog.AppendLine(" } ");
                         });
                         Writer<WhiteLabelingModel>.JsonWriterList(cssObject, directory + "\\" + json);
                         file.WriteLine(cssLog.ToString());
                         file.Close();
                         file.Dispose();
                    }
                    return "Done";
               } catch (Exception ex) {
                    return ex.Message;
               }
          }
          public List<WhiteLabelingModel> CssJsonReader() {
               return Reader<WhiteLabelingModel>.JsonReaderList(directory + "\\" + json);
          }
     }
}

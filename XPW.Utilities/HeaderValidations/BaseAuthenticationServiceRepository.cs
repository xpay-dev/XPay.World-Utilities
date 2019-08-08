using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HeaderValidations {
     public class BaseAuthenticationServiceRepository {
          private string FileName;
          private string FileLocation;
          public BaseAuthenticationServiceRepository() {
               string FileLocation = HostingEnvironment.ApplicationPhysicalPath + "App_Authentications";
               string FileName = new BaseAuthenticationModel().GetType().Name + ".json";
               if (!Directory.Exists(FileLocation)) {
                    Directory.CreateDirectory(FileLocation);
               }
               if (!File.Exists(FileLocation + "\\" + FileName)) {
                    FileStream fileStream = File.Create(FileLocation + "\\" + FileName);
                    fileStream.Close();
                    fileStream.Dispose();
               }
          }
          public async Task<List<BaseAuthenticationModel>> GetAll() {
               return await Task.Run(async () => {
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    return records;
               });
          }
          public BaseAuthenticationModel Get(string authentication) {
               BaseAuthenticationModel record = new BaseAuthenticationModel();
               var decodedValues = GetValues(authentication);
               if (string.IsNullOrEmpty(decodedValues.Item1)) { return null; }
               if (string.IsNullOrEmpty(decodedValues.Item2)) { return null; }
               List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
               records = Reader<BaseAuthenticationModel>.JsonReaderList(FileLocation + "\\" + FileName);
               record = records.Where(a => a.Username.Equals(decodedValues.Item2, StringComparison.CurrentCulture)).FirstOrDefault();
               if (record != null) { return null; }
               if (record.Tag != Enums.Status.Active) { return null; }
               if (record.Password != DecodingFromBase64(decodedValues.Item2)) { return null; }
               return record;
          }
          public BaseAuthenticationModel Get(string username, string password) {
               BaseAuthenticationModel record = new BaseAuthenticationModel();
               List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
               records = Reader<BaseAuthenticationModel>.JsonReaderList(FileLocation + "\\" + FileName);
               record = records.Where(a => a.Username.Equals(username, StringComparison.CurrentCulture)).FirstOrDefault();
               if (record != null) { return null; }
               if (record.Tag != Enums.Status.Active) { return null; }
               if (record.Password != DecodingFromBase64(password)) { return null; }
               record.Password = DecodingFromBase64(password);
               return record;
          }
          public async Task<BaseAuthenticationModel> Get(Guid id) {
               return await Task.Run(async () => {
                    BaseAuthenticationModel record = new BaseAuthenticationModel();
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    record = records.Find(a => a.Id == id);
                    return record;
               });
          }
          public async Task<BaseAuthenticationModel> Save(BaseAuthenticationModel entity) {
               return await Task.Run(async () => {
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    records.Add(entity);
                    _ = Writer<BaseAuthenticationModel>.JsonWriterListAsync(records, FileLocation + "\\" + FileName);
                    return entity;
               });
          }
          public async Task<BaseAuthenticationModel> Update(BaseAuthenticationModel entity) {
               return await Task.Run(async () => {
                    BaseAuthenticationModel record      = new BaseAuthenticationModel();
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records             = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    records             = records.Where(a => a.Id != entity.Id).ToList();
                    entity.DateUpdated  = DateTime.Now;
                    records.Add(entity);
                    _ = Writer<BaseAuthenticationModel>.JsonWriterListAsync(records, FileLocation + "\\" + FileName);
                    return entity;
               });
          }
          public async Task Delete(Guid id) {
               await Task.Run(async () => {
                    BaseAuthenticationModel record = new BaseAuthenticationModel();
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    records = records.Where(a => a.Id != id).ToList();
                    _ = Writer<BaseAuthenticationModel>.JsonWriterListAsync(records, FileLocation + "\\" + FileName);
               });
          }
          public async Task<BaseAuthenticationModel> ReTag(Guid id) {
               return await Task.Run(async () => {
                    BaseAuthenticationModel record = new BaseAuthenticationModel();
                    List<BaseAuthenticationModel> records = new List<BaseAuthenticationModel>();
                    records             = await Reader<BaseAuthenticationModel>.JsonReaderListAsync(FileLocation + "\\" + FileName);
                    record              = records.Find(a => a.Id == id);
                    records             = records.Where(a => a.Id != id).ToList();
                    record.Tag          = (record.Tag == Enums.Status.Active ? Enums.Status.Inactive : Enums.Status.Active);
                    record.DateUpdated  = DateTime.Now;
                    records.Add(record);
                    _ = Writer<BaseAuthenticationModel>.JsonWriterListAsync(records, FileLocation + "\\" + FileName);
                    return record;
               });
          }
          private string DecodingFromBase64(string base64String) {
               byte[] bytes = Convert.FromBase64String(base64String);
               string returnValue = Encoding.UTF8.GetString(bytes);
               return returnValue;
          }
          private string EncodingToBase64(string baseString) {
               byte[] bytes = Encoding.UTF8.GetBytes(baseString);
               string returnValue = Convert.ToBase64String(bytes);
               return returnValue;
          }
          private Tuple<string, string> GetValues(string base64String) {
               var decoded = DecodingFromBase64(base64String);
               var values  = decoded.Split('|');
               if (values.Length < 2) {
                    return new Tuple<string, string>(string.Empty, string.Empty);
               }
               return new Tuple<string, string>(values[0], values[1]);
          }
     }
}

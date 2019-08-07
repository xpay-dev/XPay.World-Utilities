using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using XPW.Utilities.BaseContext;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.HeaderValidations {
     public class BaseAuthenticationServiceRepository<C, E> where C : 
          DbContext, new() where E :
          BaseAuthenticationModel, new() {
          internal class BaseRepo : BaseRepository<C, E>, IBaseRepo { }
          internal interface IBaseRepo : IBaseRepository<E> { }
          internal class BaseServices : BaseService<E, BaseRepo> { }
          internal BaseServices Service = new BaseServices();
          private bool isFile;
          public BaseAuthenticationServiceRepository(bool file) {
               isFile = file;
          }
          public async Task<List<E>> GetAll() {
               return await Task.Run(() => {
                    List<E> records = new List<E>();
                    if (isFile) {
                         records = Reader<E>.JsonReaderList(HostingEnvironment.ApplicationPhysicalPath + "App_Authentications" + "\\" + new E().GetType().Name + ".json");
                    } else {
                         records = Service.GetAll().ToList();
                    }
                    return records;
               });
          }
          public async Task<E> Get(string authentication) {
               return await Task.Run(() => {
                    E record = new E();
                    var decodedValues = GetValues(authentication);
                    if (string.IsNullOrEmpty(decodedValues.Item1)) {
                         return null;
                    }
                    if (string.IsNullOrEmpty(decodedValues.Item2)) {
                         return null;
                    }
                    if (string.IsNullOrEmpty(decodedValues.Item3)) {
                         return null;
                    }
                    List<E> records = new List<E>();
                    if (isFile) {
                         records = Reader<E>.JsonReaderList(HostingEnvironment.ApplicationPhysicalPath + "App_Authentications" + "\\" + new E().GetType().Name + ".json");
                    } else {
                         records = Service.GetAll().ToList();
                    }
                    records = records.Where(a => a.Client.Equals(decodedValues.Item1, StringComparison.CurrentCulture)).ToList();
                    record  = records.Where(a => a.Username.Equals(decodedValues.Item2, StringComparison.CurrentCulture)).FirstOrDefault();
                    if (record.Password != DecodingFromBase64(decodedValues.Item3)) {
                         return null;
                    }
                    return record;
               });
          }
          internal string DecodingFromBase64(string base64String) {
               byte[] bytes = Convert.FromBase64String(base64String);
               string returnValue = Encoding.UTF8.GetString(bytes);
               return returnValue;
          }
          internal string EncodingToBase64(string baseString) {
               byte[] bytes = Encoding.UTF8.GetBytes(baseString);
               string returnValue = Convert.ToBase64String(bytes);
               return returnValue;
          }
          internal Tuple<string, string, string> GetValues(string base64String) {
               var decoded = DecodingFromBase64(base64String);
               var values  = decoded.Split('|');
               if (values.Length != 3) {
                    return new Tuple<string, string, string>(string.Empty, string.Empty, string.Empty);
               }
               return new Tuple<string, string, string>(values[0], values[1], values[2]);
          }
     }
}

using CryptoManagement;
using System;
using System.Text;

namespace XPW.Utilities.CryptoHashingManagement {
     public class HashUtilityManagement {
          internal static string Key;
          internal static string IV;
          public HashUtilityManagement(string key, string iv) {
               Key = key;
               IV = iv;
          }
          public HashUtilityManagement() { }
          public string Encrypt(string value) {
               CryptoProvider crypto = new CryptoProvider(Key, IV);
               value = crypto.Encrypt(value.Trim());
               return value;
          }
          public string Decrypt(string value) {
               try {
                    CryptoProvider crypto = new CryptoProvider(Key, IV);
                    value = crypto.Decrypt(value.Trim());
                    return value;
               } catch {
                    throw new Exception("Invalid encryption reference, cannot be decrypt");
               }
          }
          public string DecodingFromBase64(string base64String) {
               try {
                    byte[] bytes = Convert.FromBase64String(base64String);
                    string returnValue = Encoding.UTF8.GetString(bytes);
                    return returnValue;
               } catch {
                    return string.Empty;
               }
          }
          public string EncodingToBase64(string baseString) {
               try {
                    byte[] bytes = Encoding.UTF8.GetBytes(baseString);
                    string returnValue = Convert.ToBase64String(bytes);
                    return returnValue;
               } catch {
                    return string.Empty;
               }
          }
     }
}

using CryptoManagement;
using System;

namespace XPW.Utilities.CryptoHashingManagement {
     public class HashUtilityManagement {
          internal static string Key;
          internal static string IV;
          public HashUtilityManagement(string key, string iv) {
               Key = key;
               IV = iv;
          }
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
     }
}

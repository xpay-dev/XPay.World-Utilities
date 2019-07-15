using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace XPW.Utilities.CryptoHashingManagement {
     public class CardHashiingManagement {
          public static Tuple<string, string, string> CardEncryption(string cardNumber, int firstNumbers, int lastNumbers, bool returnNull) {
               try {
                    string cardMask = MaskCard(cardNumber, firstNumbers, lastNumbers);
                    byte[] key;
                    byte[] IV;
                    string data = GenerateSymmetricKeyAndEcryptData(cardMask, out key, out IV);
                    return new Tuple<string, string, string>(data, Convert.ToBase64String(key), Convert.ToBase64String(IV));
               } catch (Exception ex) {
                    if (returnNull) {
                         return null;
                    } else {
                         throw ex;
                    }
               }
          }
          public static string CardDecryption(string cardMask, string key, string IV, bool returnNull) {
               try {
                    string data = DecryptEncDataWithKeyAndIV(cardMask, key, IV);
                    return data;
               } catch (Exception ex) {
                    if (returnNull) {
                         return null;
                    } else {
                         throw ex;
                    }
               }
          }
          internal static string MaskCard(string cardNumber, int firstNumbers, int lastNumbers) {
               string first = cardNumber.Substring(0, firstNumbers);
               string middle = new string('*', cardNumber.Length - (firstNumbers + lastNumbers));
               string last = cardNumber.Substring(cardNumber.Length - lastNumbers);
               string cardMask = first + middle + last;
               return cardMask;
          }
          internal static string GenerateSymmetricKeyAndEcryptData(string data, out byte[] key, out byte[] IV) {
               try {
                    byte[] desKey;
                    byte[] desIV;
                    MemoryStream output = new MemoryStream();
                    byte[] byteData = new UnicodeEncoding().GetBytes(data);
                    TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                    CryptoStream encrypt = new CryptoStream(output, des.CreateEncryptor(des.Key, des.IV), CryptoStreamMode.Write);
                    desKey = des.Key;
                    desIV = des.IV;
                    encrypt.Write(byteData, 0, byteData.Length);
                    encrypt.Close();
                    output.Close();
                    string encData = Convert.ToBase64String(output.ToArray());
                    key = desKey;
                    IV = desIV;
                    return encData;
               } catch {
                    throw new Exception("Encryption failed.");
               }
          }
          internal static string DecryptEncDataWithKeyAndIV(string encData, string key, string IV) {
               try {
                    byte[] byteKey = Convert.FromBase64String(key);
                    byte[] byteIV = Convert.FromBase64String(IV);
                    TripleDESCryptoServiceProvider tddes = new TripleDESCryptoServiceProvider();
                    MemoryStream output = new MemoryStream();
                    byte[] byteData = Convert.FromBase64String(encData);
                    CryptoStream decrypt = new CryptoStream(output, tddes.CreateDecryptor(byteKey, byteIV), CryptoStreamMode.Write);
                    decrypt.Write(byteData, 0, byteData.Length);
                    decrypt.Close();
                    output.Close();
                    return new UnicodeEncoding().GetString(output.ToArray());
               } catch (Exception) {
                    throw new Exception("Unable to decrypt data");
               }
          }
     }
}

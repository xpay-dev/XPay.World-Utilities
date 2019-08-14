using System;
using System.Text;

namespace XPW.Utilities.Functions {
     [Serializable]
     public class Generator {
          public static string ConvertHexaToString(string hexa) {
               byte[] data = FromHex(hexa);
               string text = Encoding.ASCII.GetString(data);

               return text;
          }

          public static byte[] FromHex(string hex) {
               hex = hex.Replace("-", "");
               byte[] raw = new byte[hex.Length / 2];
               for (int i = 0; i < raw.Length; i++) {
                    raw[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
               }
               return raw;
          }

          public static string DecodeBase64ToHexaString(string base64) {
               string binary = ConvertBase64ToBinary(base64);
               string hexa = ConvertBinaryStringToHexString(binary.Replace(" ", ""));

               return hexa;
          }

          public static string ConvertBase64ToBinary(string input) {
               byte[] buff = Convert.FromBase64String(input);
               string binary = "";
               foreach (byte b in buff) {
                    binary += Convert.ToString(b, 2).PadLeft(8, '0') + " ";
               }

               return binary;
          }

          public static string ConvertBinaryStringToHexString(string binary) {
               StringBuilder result = new StringBuilder(binary.Length / 8 + 1);

               int mod4Len = binary.Length % 8;
               if (mod4Len != 0) {
                    binary = binary.PadLeft(((binary.Length / 8) + 1) * 8, '0');
               }

               for (int i = 0; i < binary.Length; i += 8) {
                    string eightBits = binary.Substring(i, 8);
                    result.AppendFormat("{0:X2}", Convert.ToByte(eightBits, 2));
               }

               return result.ToString();
          }

          public static string GenerateOrderId() {
               Random rnd = new Random();
               int value = rnd.Next(100000, 999999);

               string orderId = DateTime.Now.ToString("yyyyMMddhhmmss") + Convert.ToString(value);

               return orderId;
          }

          public static string GenerateVerificationNumber(int lenght = 1) {
               Random random = new Random();

               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < lenght; i++)
                    sb.Append(random.Next(0, 9).ToString());

               return sb.ToString();
          }

          public static string GenerateESignatureCode() {
               Random random = new Random();

               StringBuilder sb = new StringBuilder();

               for (int i = 0; i < 6; i++)
                    sb.Append(random.Next(0, 9).ToString());

               return "VPI" + DateTime.Now.ToString("yyyyddmm") + sb.ToString();
          }
     }
}

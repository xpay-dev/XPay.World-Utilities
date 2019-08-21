using System;
using System.Globalization;
using System.IO;
using System.Security.AccessControl;
using System.Text;
using System.Text.RegularExpressions;

namespace XPW.Utilities.Functions {
     public class Checker {
          public static bool EmailValidator(string email) {
               if (string.IsNullOrWhiteSpace(email)) {
                    return false;
               }
               try {
                    email = Regex.Replace(email, @"(@)(.+)$",
                        DomainMapper,
                        RegexOptions.None,
                        TimeSpan.FromMilliseconds(200));

                    string DomainMapper(Match match) {
                         var idn = new IdnMapping();
                         var domainName = idn.GetAscii(match.Groups[2].Value);
                         return match.Groups[1].Value + domainName;
                    }
               } catch (RegexMatchTimeoutException) {
                    return false;
               } catch (ArgumentException) {
                    return false;
               }
               try {
                    return Regex.IsMatch(email,
                        @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                        @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                        RegexOptions.IgnoreCase,
                        TimeSpan.FromMilliseconds(250));
               } catch (RegexMatchTimeoutException) {
                    return false;
               }
          }
          public static bool CheckFolderPermission(string folderPath) {
               DirectoryInfo dirInfo = new DirectoryInfo(folderPath);
               try {
                    DirectorySecurity dirAC = dirInfo.GetAccessControl(AccessControlSections.All);
                    return true;
               } catch (PrivilegeNotHeldException) {
                    return false;
               }
          }
          public static string NumberExtractor(string toExtract) {
               string splitPattern = @"[^\d]";
               string[] results = Regex.Split(toExtract, splitPattern);
               StringBuilder sb = new StringBuilder();
               foreach (string s in results) {
                    sb.Append(s);
               }
              return sb.ToString();
          }
     }
}

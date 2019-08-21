using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web.Hosting;
using XPW.Utilities.NoSQL;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.EmailManagement {
     [Serializable]
     public class EmailSending {
          internal static List<EmailManagementConfiguration> emainConfigurations = new List<EmailManagementConfiguration>();
          internal static string defaultEmailConfigFolder   = HostingEnvironment.ApplicationPhysicalPath + "App_Settings";
          public void SendEmail(EmailManagementodel email, string emailConfigName, string configName) { 
               try {
                    if (!Directory.Exists(defaultEmailConfigFolder)) {
                         Directory.CreateDirectory(defaultEmailConfigFolder);
                    }
                    emailConfigName = defaultEmailConfigFolder + "\\" + emailConfigName + ".json";
                    if (!File.Exists(emailConfigName)) {
                         FileStream file = File.Create(emailConfigName);
                         file.Close();
                         file.Dispose();
                    }
                    emainConfigurations = Reader<EmailManagementConfiguration>.JsonReaderList(emailConfigName);
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.AlternateViews.Add(email.EmailContent);
                    EmailManagementConfiguration emailConfig = GetEmailConfiguration(configName);
                    if (emailConfig == null) {
                         throw new Exception("No email configuration found!");
                    }
                    mailMessage.From = new MailAddress(emailConfig.Properties.From, email.SenderDisplayName);
                    if (email.EmailReceipients == null) {
                         throw new Exception("Receipients cannot be null");
                    }
                    if (email.EmailReceipients.Count == 0) {
                         throw new Exception("Receipients cannot be null");
                    }
                    email.EmailReceipients.ForEach(a => {
                         mailMessage.To.Add(new MailAddress(a.Email, a.DisplayName));
                    });
                    if (email.EmailCcReceipients != null) {
                         if (email.EmailCcReceipients.Count != 0) {
                              email.EmailCcReceipients.ForEach(a => {
                                   mailMessage.CC.Add(new MailAddress(a.Email, a.DisplayName));
                              });
                         }
                    }
                    if (email.EmailBccReceipients != null) {
                         if (email.EmailBccReceipients.Count != 0) {
                              email.EmailBccReceipients.ForEach(a => {
                                   mailMessage.Bcc.Add(new MailAddress(a.Email, a.DisplayName));
                              });
                         }
                    }
                    mailMessage.Subject           = email.Subject;
                    SmtpClient client             = new SmtpClient();
                    client.EnableSsl              = emailConfig.Properties.SSL;
                    client.Port                   = Convert.ToInt32(emailConfig.Properties.Port);
                    client.Host                   = emailConfig.Properties.Host;
                    client.Timeout                = emailConfig.Properties.TimeOut;
                    client.DeliveryMethod         = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials  = false;
                    client.Credentials            = new System.Net.NetworkCredential(emailConfig.Properties.User, emailConfig.Properties.Password);
                    client.Send(mailMessage);
               } catch (Exception ex) {
                    throw ex;
               }
          }
          public EmailManagementConfiguration GetEmailConfiguration(string config) {
               return (emainConfigurations == null ? null : emainConfigurations.Count == 0 ? null : emainConfigurations.Where(a => a.Name.Equals(config, StringComparison.CurrentCulture)).FirstOrDefault());
          }
     }
}

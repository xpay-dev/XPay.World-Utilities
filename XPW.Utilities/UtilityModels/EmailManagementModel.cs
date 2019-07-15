using System.Collections.Generic;
using System.Net.Mail;

namespace XPW.Utilities.UtilityModels {
     public class EmailManagementodel {
          public AlternateView EmailContent { get; set; }
          public string SenderDisplayName { get; set; }
          public List<EmailManagementodelReceipientModel> EmailReceipients { get; set; }
          public List<EmailManagementodelReceipientModel> EmailCcReceipients { get; set; }
          public List<EmailManagementodelReceipientModel> EmailBccReceipients { get; set; }
          public string Subject { get; set; }
     }
     public class EmailManagementodelReceipientModel {
          public string Email { get; set; }
          public string DisplayName { get; set; }
     }
     public class EmailManagementConfiguration {
          public string Name { get; set; }
          public EmailManagementConfigurationProperties Properties { get; set; }
     }
     public class EmailManagementConfigurationProperties {
          public string From { get; set; }
          public string Port { get; set; }
          public string Host { get; set; }
          public string User { get; set; }
          public string Password { get; set; }
          public bool SSL { get; set; }
     }

}

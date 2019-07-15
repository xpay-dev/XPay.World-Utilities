using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Hosting;
using XPW.Utilities.APICallManagement;
using XPW.Utilities.AppConfigManagement;
using XPW.Utilities.Enums;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.GoogleRecaptcha {
     public class Validation {
          public async Task<GoogleRecaptchaModel> Checker(string recaptchaResponse, string ip) {
               AppConfig appConfigManager = new AppConfig(HostingEnvironment.ApplicationPhysicalPath + "App_Settings", "appConfig.json");
               var response = await new APIConnect<GoogleRecaptchaModel>(APIRequestMethod.Get).ClassObject(new APICallModel {
                    Host = appConfigManager.AppSetting<string>("GoogleReCaptchaHost"),
                    Path = string.Format("recaptcha/api/siteverify?secret={0}&response={1}&remoteip={2}", appConfigManager.AppSetting<string>("GoogleReCaptchaSecretKey"), recaptchaResponse, ip),
                    AdditionalHeaders = new List<APICallHeaderModel>()
               });
               return response;
          }
     }
}

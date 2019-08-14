using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XPW.Utilities.Enums;
using XPW.Utilities.UtilityModels;

namespace XPW.Utilities.APICallManagement {
     [Serializable]
     public class APIConnect<T> where T : class, new() {
          private APIRequestMethod APICallMethod { get; } = APIRequestMethod.None;
          public APIConnect(APIRequestMethod method) { APICallMethod = method; }
          //Data Table
          public async Task<T> DataTable(APICallModel request) {
               using (var httpClient = new HttpClient { BaseAddress = new Uri(request.Host) }) {
                    if (request.AdditionalHeaders.Count > 0) {
                         request.AdditionalHeaders.ForEach(a => {
                              httpClient.DefaultRequestHeaders.TryAddWithoutValidation(a.Name, a.Value);
                         });
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    using (var content = new StringContent(request.ContentData, Encoding.UTF8, request.ContentType)) {
                         string responseData = string.Empty;
                         if (APICallMethod == APIRequestMethod.Post) {
                              using (var response = await httpClient.PostAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Get) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Put) {
                              using (var response = await httpClient.PutAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Delete) {
                              using (var response = await httpClient.DeleteAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else { responseData = "Invalid Request Method"; }
                         if (responseData == "Unauthorized") {
                              throw new UnauthorizedAccessException("Unauthorized access");
                         }
                         if (responseData == "Invalid Request Method") {
                              throw new UnauthorizedAccessException("Invalid Request Method");
                         }
                         try {
                              var convertResponse = responseData.Replace(@"\", string.Empty).TrimStart('\"').TrimEnd('\"');
                              var genericResponse = JsonConvert.DeserializeObject<T>(convertResponse);
                              return genericResponse;
                         } catch { return null; }
                    }
               }
          }
          //Data Table List
          public async Task<List<T>> DataTableList(APICallModel request) {
               using (var httpClient = new HttpClient { BaseAddress = new Uri(request.Host) }) {
                    if (request.AdditionalHeaders.Count > 0) {
                         request.AdditionalHeaders.ForEach(a => {
                              httpClient.DefaultRequestHeaders.TryAddWithoutValidation(a.Name, a.Value);
                         });
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    using (var content = new StringContent(request.ContentData, Encoding.UTF8, request.ContentType)) {
                         string responseData = string.Empty;
                         if (APICallMethod == APIRequestMethod.Post) {
                              using (var response = await httpClient.PostAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Get) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Put) {
                              using (var response = await httpClient.PutAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Delete) {
                              using (var response = await httpClient.DeleteAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else { responseData = "Invalid Request Method"; }
                         if (responseData == "Unauthorized") {
                              throw new UnauthorizedAccessException("Unauthorized access");
                         }
                         if (responseData == "Invalid Request Method") {
                              throw new UnauthorizedAccessException("Invalid Request Method");
                         }
                         try {
                              var convertResponse = responseData.Replace(@"\", string.Empty).TrimStart('\"').TrimEnd('\"');
                              var genericResponse = JsonConvert.DeserializeObject<List<T>>(convertResponse);
                              return genericResponse;
                         } catch { return null; }
                    }
               }
          }
          //Class Object
          public async Task<T> ClassObject(APICallModel request) {
               using (var httpClient = new HttpClient { BaseAddress = new Uri(request.Host) }) {
                    if (request.AdditionalHeaders.Count > 0) {
                         request.AdditionalHeaders.ForEach(a => {
                              httpClient.DefaultRequestHeaders.TryAddWithoutValidation(a.Name, a.Value);
                         });
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    using (var content = new StringContent(request.ContentData, Encoding.UTF8, request.ContentType)) {
                         string responseData = string.Empty;
                         if (APICallMethod == APIRequestMethod.Post) {
                              using (var response = await httpClient.PostAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Get) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Put) {
                              using (var response = await httpClient.PutAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Delete) {
                              using (var response = await httpClient.DeleteAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else { responseData = "Invalid Request Method"; }
                         if (responseData == "Unauthorized") {
                              throw new UnauthorizedAccessException("Unauthorized access");
                         }
                         if (responseData == "Invalid Request Method") {
                              throw new UnauthorizedAccessException("Invalid Request Method");
                         }
                         try {
                              var genericResponse = JsonConvert.DeserializeObject<T>(responseData);
                              return genericResponse;
                         } catch { return null; }
                    }
               }
          }
          //Class Object List
          public async Task<List<T>> ClassObjectList(APICallModel request) {
               using (var httpClient = new HttpClient { BaseAddress = new Uri(request.Host) }) {
                    if (request.AdditionalHeaders.Count > 0) {
                         request.AdditionalHeaders.ForEach(a => {
                              httpClient.DefaultRequestHeaders.TryAddWithoutValidation(a.Name, a.Value);
                         });
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    using (var content = new StringContent(request.ContentData, Encoding.UTF8, request.ContentType)) {
                         string responseData = string.Empty;
                         if (APICallMethod == APIRequestMethod.Post) {
                              using (var response = await httpClient.PostAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Get) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Put) {
                              using (var response = await httpClient.PutAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Delete) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else { responseData = "Invalid Request Method"; }
                         if (responseData == "Unauthorized") {
                              throw new UnauthorizedAccessException("Unauthorized access");
                         }
                         if (responseData == "Invalid Request Method") {
                              throw new UnauthorizedAccessException("Invalid Request Method");
                         }
                         try {
                              var genericResponse = JsonConvert.DeserializeObject<List<T>>(responseData);
                              return genericResponse;
                         } catch { return null; }
                    }
               }
          }
     }
     [Serializable]
     public class APIConnect {
          private APIRequestMethod APICallMethod { get; } = APIRequestMethod.None;
          public APIConnect(APIRequestMethod method) { APICallMethod = method; }
          //File Uploader
          public async Task<string> FileUploader(APICallModel request, string fileLocation, string apiAuthUser, string apiAuthPassword) {
               return await Task.Run(() => {
                    StreamReader sr = new StreamReader(fileLocation);
                    string filename = Path.GetFileNameWithoutExtension(fileLocation);
                    string fileExtension = Path.GetExtension(fileLocation);
                    byte[] result;
                    using (var streamReader = new MemoryStream()) {
                         sr.BaseStream.CopyTo(streamReader);
                         result = streamReader.ToArray();
                    }
                    Uri wsHost = new Uri(string.Format("{0}{1}", request.Host, request.Path));
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(wsHost);
                    string encoded = Convert.ToBase64String(Encoding.GetEncoding("ISO-8859-1").GetBytes(apiAuthUser + ":" + apiAuthPassword));
                    request.AdditionalHeaders.ForEach(a => {
                         webRequest.Headers.Add(a.Name, a.Value);
                    });
                    webRequest.Method = "POST";
                    webRequest.ContentType = request.ContentType;
                    webRequest.ContentLength = result.Length;
                    using (var data = webRequest.GetRequestStream()) {
                         data.Write(result, 0, result.Length);
                    }
                    string responseResult = string.Empty;
                    using (var response = (HttpWebResponse)webRequest.GetResponse()) {
                         ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                         using (StreamReader srr = new StreamReader(response.GetResponseStream())) {
                              responseResult = srr.ReadToEnd();
                         }
                    }
                    if (responseResult == "Unauthorized") {
                         throw new UnauthorizedAccessException("Unauthorized access");
                    }
                    return responseResult;
               });
          }
          //Data Type
          public async Task<TValue> DataType<TValue>(APICallModel request, string returnError) {
               using (var httpClient = new HttpClient { BaseAddress = new Uri(request.Host) }) {
                    if (request.AdditionalHeaders.Count > 0) {
                         request.AdditionalHeaders.ForEach(a => {
                              httpClient.DefaultRequestHeaders.TryAddWithoutValidation(a.Name, a.Value);
                         });
                    }
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;
                    using (var content = new StringContent(request.ContentData, Encoding.UTF8, request.ContentType)) {
                         string responseData = string.Empty;
                         if (APICallMethod == APIRequestMethod.Post) {
                              using (var response = await httpClient.PostAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Get) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Put) {
                              using (var response = await httpClient.PutAsync(request.Path, content)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else if (APICallMethod == APIRequestMethod.Delete) {
                              using (var response = await httpClient.GetAsync(request.Path)) {
                                   responseData = await response.Content.ReadAsStringAsync();
                              }
                         } else { responseData = "Invalid Request Method"; }
                         if (responseData == "Unauthorized") {
                              throw new UnauthorizedAccessException("Unauthorized access");
                         }
                         if (responseData == "Invalid Request Method") {
                              throw new UnauthorizedAccessException("Invalid Request Method");
                         }
                         try {
                              return (TValue)Convert.ChangeType(responseData, typeof(TValue));
                         } catch { return (TValue)Convert.ChangeType(returnError, typeof(TValue)); }
                    }
               }
          }
     }
}

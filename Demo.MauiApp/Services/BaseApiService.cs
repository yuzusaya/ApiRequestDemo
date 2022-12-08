using Microsoft.AppCenter.Crashes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Demo.MauiApp.Services
{
    public class BaseApiService
    {
        protected const string baseServiceUrl = "https://demowebapi1208.azurewebsites.net/";
        public HttpClient Client { get; } = new HttpClient();
        public async Task<T> GetTAsync<T>(string url, CancellationToken token, bool internalApi = true)
        {
            try
            {
                if (internalApi)
                {
                    //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                    url = baseServiceUrl + url;
                }
                else
                {
                    Client.DefaultRequestHeaders.Clear();
                }
                HttpResponseMessage response = await Client.GetAsync(url, token);
                if (response.IsSuccessStatusCode)
                {
                    T result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    //T result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    return result;
                }
                else
                {
                    throw new HttpRequestException($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}");
                }
            }
            catch (TaskCanceledException) { throw; }
            catch (OperationCanceledException) { throw; }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Cancel"))//Java.IO.IOException
                {
                    throw;
                }
                var properties = new Dictionary<string, string> {
                    { "Method", "GET" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }
        public async Task<T> GetTAsync<T>(string url, bool internalApi = true)
        {
            try
            {
                if (internalApi)
                {
                    //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                    url = baseServiceUrl + url;
                }
                else
                {
                    Client.DefaultRequestHeaders.Clear();
                }
                HttpResponseMessage response = await Client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T result = JsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()
                    {
                        PropertyNameCaseInsensitive = true
                    });
                    //T result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
                    return result;
                }
                else
                {
                    throw new HttpRequestException($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}");
                }
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "GET" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostTAsync<T>(string url, T item)
        {
            try
            {
                //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                var itemJson = JsonConvert.SerializeObject(item);
                var content = new StringContent(itemJson, Encoding.UTF8, "application/json");
                var response = await Client.PostAsync(baseServiceUrl + url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var properties = new Dictionary<string, string> {
                        { "Method", "POST (json)" },
                        {"Url", url }
                    };
                    Crashes.TrackError(new Exception($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}"), properties);
                }
                return response;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                        { "Method", "POST (json)" },
                        {"Url", url }
                    };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PostTAsync<T>(string url, T item, List<KeyValuePair<string, Stream>> streamsWithName)
        {
            try
            {
                //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                var itemJson = JsonConvert.SerializeObject(item);
                var content = new StringContent(itemJson, Encoding.UTF8, "application/json");
                multiContent.Add(content, "model");

                if (streamsWithName != null)
                {
                    foreach (var streamWithName in streamsWithName)
                    {
                        multiContent.Add(new StreamContent(streamWithName.Value), "files", streamWithName.Key);
                    }
                }

                var response = await Client.PostAsync(baseServiceUrl + url, multiContent);
                if (!response.IsSuccessStatusCode)
                {
                    var properties = new Dictionary<string, string> {
                        { "Method", "POST (json)" },
                        {"Url", url }
                    };
                    Crashes.TrackError(new Exception($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}"), properties);
                }
                return response;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "POST (json)" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutAsync<T>(string url, T item)
        {
            try
            {
                //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                var response = await Client.PutAsync(baseServiceUrl + url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var properties = new Dictionary<string, string> {
                        { "Method", "PUT (json)" },
                        {"Url", url }
                    };
                    Crashes.TrackError(new Exception($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}"), properties);
                }
                return response;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "PUT (json)" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

        public async Task<HttpResponseMessage> PutTAsync<T>(string url, T item, List<KeyValuePair<string, Stream>> streamsWithName)
        {
            try
            {
                //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                MultipartFormDataContent multiContent = new MultipartFormDataContent();
                var itemJson = JsonConvert.SerializeObject(item);
                var content = new StringContent(itemJson, Encoding.UTF8, "application/json");
                multiContent.Add(content, "model");

                if (streamsWithName != null)
                {
                    foreach (var streamWithName in streamsWithName)
                    {
                        multiContent.Add(new StreamContent(streamWithName.Value), "files", streamWithName.Key);
                    }
                }

                var response = await Client.PutAsync(baseServiceUrl + url, multiContent);
                if (!response.IsSuccessStatusCode)
                {
                    var properties = new Dictionary<string, string> {
                        { "Method", "PUT (json)" },
                        {"Url", url }
                    };
                    Crashes.TrackError(new Exception($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}"), properties);
                }
                return response;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "PUT (json)" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string url)
        {
            try
            {
                //Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", await SecureStorage.GetAsync(MySettings.AccessTokenKey));
                var response = await Client.DeleteAsync(baseServiceUrl + url);
                if (!response.IsSuccessStatusCode)
                {
                    var properties = new Dictionary<string, string> {
                        { "Method", "DeleteAsync" },
                        {"Url", url }
                    };
                    Crashes.TrackError(new HttpRequestException($"{response.StatusCode} {response.ReasonPhrase}: {response.Content}"), properties);
                }
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                var properties = new Dictionary<string, string> {
                    { "Method", "DeleteAsync" },
                    {"Url", url }
                };
                Crashes.TrackError(ex, properties);
                throw;
            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ELDEL_EaseeAPI_Library.Services
{
    public interface IHttpService
    {
        Task<T> GetAsync<T>(HttpClient httpClient, string requestUri, string token = "", Dictionary<string, string> queryParameters = null);
        Task<T> PostAsync<T>(HttpClient httpClient, Uri uri, string body, string token = "", Dictionary<string, string> queryParameters = null);
        Task<T> PutAsync<T>(HttpClient httpClient, Uri uri, string body, string token = "", Dictionary<string, string> queryParameters = null);
        Task<T> DeleteAsync<T>(HttpClient httpClient, Uri uri, string token = "", Dictionary<string, string> queryParameters = null);
    }
}

using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IWebService
{
    Task<HttpResponseMessage> GetAsync(string url);
    Task PostAsync(string url);
    Task PutAsync(string url);
    Task DeleteAsync(string url);
}
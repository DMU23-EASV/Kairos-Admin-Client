using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IWebService
{
    Task<ResponsPackage> GetAsync(string url);
    Task PostAsync(string url);
    Task PutAsync(string url);
    Task DeleteAsync(string url);
}
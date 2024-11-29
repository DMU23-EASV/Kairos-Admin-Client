using System.Net;
using System.Net.Http;
using System.Text.Json.Nodes;
using WPF_MVVM_TEMPLATE.Entitys;

namespace WPF_MVVM_TEMPLATE.InterfaceAdapter;

public interface IWebService
{
    Task<ResponsPackage> GetAsync(string url);
    Task<ResponsPackage>  PostAsync(string url, object data);
    Task<ResponsPackage>  PutAsync(string url, object data);
    Task<ResponsPackage>  DeleteAsync(string url);
}
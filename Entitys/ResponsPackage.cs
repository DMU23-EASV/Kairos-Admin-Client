using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WPF_MVVM_TEMPLATE.Entitys;

public class ResponsPackage
{
    public HttpResponseHeaders? Headers { get; set; }
    public HttpStatusCode? StatusCode { get; set; }
    public string? ResponseBody { get; set; }
    
}
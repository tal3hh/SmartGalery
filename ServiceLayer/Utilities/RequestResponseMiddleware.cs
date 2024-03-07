using Microsoft.AspNetCore.Http;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Utilities
{
    public class RequestResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            //try
            //{
            //    Log.Information("Request received: {@Request}", httpContext.Request);

            //    await _next.Invoke(httpContext);

            //    Log.Information("Response sent: {@Response}", httpContext.Response);
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex, "An error occurred processing the request");
            //    httpContext.Response.StatusCode = 500;
            //    httpContext.Response.ContentType = "text/plain";
            //    await httpContext.Response.WriteAsync("Servislə bağlı xəta baş verdi.");
            //}

            try
            {
                // HTTP tələbi məlumatlarını string kimi yarat
                string requestInfo = "-------------------------------------------------------------------" +
                    $"RequestMethod: {httpContext.Request.Method}, " +
                                     $"RequestPath: {httpContext.Request.Path}, " +
                                     $"QueryString: {httpContext.Request.QueryString}, " +
                                     $"Headers: {string.Join(", ", httpContext.Request.Headers.Select(h => $"{h.Key}: {h.Value}"))}";

                Log.Information("Tələb qəbul edildi: {@RequestInfo}", requestInfo);

                await _next.Invoke(httpContext);

                // HTTP cavabı məlumatlarını string kimi yarat
                string responseInfo = $"StatusCode: {httpContext.Response.StatusCode}, " +
                                      $"Headers: {string.Join(", ", httpContext.Response.Headers.Select(h => $"{h.Key}: {h.Value}"))}";

                Log.Information("Cavab göndərildi: {@ResponseInfo}", responseInfo);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Tələbi emal etdikdə səhv baş verdi");
                httpContext.Response.StatusCode = 500;
                httpContext.Response.ContentType = "text/plain";
                await httpContext.Response.WriteAsync("Servislə bağlı xəta baş verdi.");
            }
        }
    }
}

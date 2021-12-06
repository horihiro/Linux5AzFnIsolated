using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Linux5AzFnIsolated
{
  public static class Function2
  {
    [Function("Function2")]
    public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req,
        FunctionContext executionContext)
    {
      ProcessStartInfo startInfo = new ProcessStartInfo
      {
        FileName = "sh",
        ArgumentList = {"-c", "ls -al ./.playwright/ms-playwright/**/chrome-linux/chrome"},
        RedirectStandardInput = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false
      };
      Process process = Process.Start(startInfo);
      var query = System.Web.HttpUtility.ParseQueryString(req.Url.Query);
      string output = process.StandardOutput.ReadToEnd();
      process.WaitForExit();
      var response = req.CreateResponse(HttpStatusCode.OK);
      response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

      response.WriteString(output);
      return response;
    }
  }
}

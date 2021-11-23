using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Linux5AzFnIsolated
{
    public static class Function1
    {
        [Function("Function1")]
        public static async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req, FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("Function1");
            logger.LogInformation("C# HTTP trigger function processed a request.");

            var response = req.CreateResponse(HttpStatusCode.OK);
            // response.Headers.Add("Content-Type", "text/plain; charset=utf-8");

            // response.WriteString("Welcome to Azure Functions!");

            // return response;

            response.Headers.Add("Content-Type", "image/png");

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync();
            var page = await browser.NewPageAsync();
            await page.GotoAsync("https://playwright.dev/dotnet");
            response.WriteBytes(await page.ScreenshotAsync());
            return response;
        }
    }
}

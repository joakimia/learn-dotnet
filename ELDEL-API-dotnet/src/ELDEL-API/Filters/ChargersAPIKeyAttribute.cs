using System;
using System.Threading.Tasks;
using ELDEL_API.Config;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace ELDEL_API.Filters
{
    [AttributeUsage(validOn: AttributeTargets.Method)]
    public class ChargersApiKeyAttribute : Attribute, IAsyncActionFilter
    {
        private const string API_KEY_NAME = "ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(API_KEY_NAME, out var potentialExtractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key was not provided"
                };
                return;
            }

            var eldelAPIConfig = context.HttpContext.RequestServices.GetRequiredService<IEldelAPIConfig>();

            var apiKey = eldelAPIConfig.ChargersAPIKey;

            if (!apiKey.Equals(potentialExtractedApiKey))
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    Content = "Api Key is not valid"
                };
                return;
            }

            await next();
        }
    }
}

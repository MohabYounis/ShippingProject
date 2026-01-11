using Microsoft.AspNetCore.Mvc.Filters;
using System.Text.Json;

namespace Shipping.Fillter
{
    public class LogActionFilter : IActionFilter
    {
        private readonly ILogger<LogActionFilter> logger;

        public LogActionFilter(ILogger<LogActionFilter> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context) //after action
        {
            logger.LogInformation("Action Executing: " + context.RouteData);
        }
        public void OnActionExecuted(ActionExecutedContext context)  //before action
        {
        }

    }
}

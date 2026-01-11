using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace Shipping.Fillter
{
    public class LogSensitiveActiveAttribute:ActionFilterAttribute
    {
        //private readonly ILogger<LogSensitiveActiveAttribute> logger;

        //public LogSensitiveActiveAttribute(ILogger<LogSensitiveActiveAttribute> logger)
        //{
        //    this.logger = logger;
        //}

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Debug.WriteLine(context.HttpContext.Request);
        }
    }
}

using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Globalization;

namespace Application.Controllers.Custom
{
    public class CustomController : Controller
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var theme = context.HttpContext.Request.Cookies["Theme"] ?? "default";
            ViewData["Path"] = $"/css/{theme}.min.css";

            ViewData["Lang"] = Thread.CurrentThread.CurrentUICulture.Name;
            base.OnActionExecuting(context);
        }
    }
}

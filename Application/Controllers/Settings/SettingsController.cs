using Application.Controllers.Custom;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Settings
{
    public class SettingsController : CustomController
    {
        [HttpGet]
        public IActionResult Settings()
        {
            var theme = Request.Cookies["Theme"] ?? "default";

            var langCookie = Request.Cookies[CookieRequestCultureProvider.DefaultCookieName];
            var lang = "en-US";
            if (!string.IsNullOrEmpty(langCookie))
            {
                var result = CookieRequestCultureProvider.ParseCookieValue(langCookie);
                lang = result?.UICultures.FirstOrDefault().Value ?? "en-US";
            }

            ViewData["CurrentTheme"] = theme;
            ViewData["CurrentLang"] = lang;

            return View();
        }


        [HttpPost]
        public IActionResult ApplySettings(string theme, string lang)
        {
            Response.Cookies.Append("Theme", theme, new CookieOptions
            {
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            var culture = new RequestCulture(lang);
            var cultureCookieValue = CookieRequestCultureProvider.MakeCookieValue(culture);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                cultureCookieValue,
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddDays(7) }
            );

            return RedirectToAction("Settings");
        }
    }
}

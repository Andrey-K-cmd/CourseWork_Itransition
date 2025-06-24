using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers
{
    public class SettingsController : CustomController
    {
        [HttpGet]
        public IActionResult Settings()
        {
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

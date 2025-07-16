using Application.Controllers.Custom;
using Application.Models.Intagration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Application.Controllers.Intagration
{
    [Authorize]
    public class IntegrationController : CustomController
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public IntegrationController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public IActionResult Integrate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Integrate(SalesforceUser model)
        {
            string token, url;

            var client = _httpClientFactory.CreateClient();
            var parameters = new Dictionary<string, string>
            {
                { "username", _configuration["Salesforce:Username"] },
                { "password", _configuration["Salesforce:Password"] + _configuration["Salesforce:SecurityToken"] },
                { "grant_type", "password" },
                { "client_id", _configuration["Salesforce:ClientId"] },
                { "client_secret", _configuration["Salesforce:ClientSecret"] }
            };

            var tokenResponse = await client.PostAsync("https://login.salesforce.com/services/oauth2/token", new FormUrlEncodedContent(parameters));
            var tokenContent = await tokenResponse.Content.ReadFromJsonAsync<JsonElement>();

            token = tokenContent.GetProperty("access_token").GetString();
            url = tokenContent.GetProperty("instance_url").GetString();

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            string escapedCompany = model.CompanyName.Replace("'", "\\'");
            string soql = $"SELECT Id FROM Account WHERE Name = '{escapedCompany}'";
            var queryResponse = await client.GetAsync($"{url}/services/data/v59.0/query?q={Uri.EscapeDataString(soql)}");
            var queryContent = await queryResponse.Content.ReadFromJsonAsync<JsonElement>();

            string accountId;
            if (queryContent.GetProperty("totalSize").GetInt32() > 0)
            {
                accountId = queryContent.GetProperty("records")[0].GetProperty("Id").GetString();
            }
            else
            {
                var accountPayload = new { Name = model.CompanyName };
                var accResp = await client.PostAsJsonAsync($"{url}/services/data/v59.0/sobjects/Account", accountPayload);
                var accContent = await accResp.Content.ReadFromJsonAsync<JsonElement>();
                accountId = accContent.GetProperty("id").GetString();
            }

            var contactPayload = new
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Title = model.Job,
                Phone = model.Phone,
                MailingCity = model.City,
                AccountId = accountId,
                BirthYear__c = model.BirthYear
            };
            await client.PostAsJsonAsync($"{url}/services/data/v59.0/sobjects/Contact", contactPayload);

            return RedirectToAction("Home", "Home");
        }

    }
}
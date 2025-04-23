using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectE.DTO.OfferDtos;
using System.Text;

namespace ProjectE.Web.Controllers
{
    public class OfferController : Controller
    {
        private readonly IHttpClientFactory _http;

        public OfferController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory;
        }

        public async Task<IActionResult> Incoming()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Company");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7034/api/Offer/assigned-to-me");
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Teklifler alınamadı.";
                return View(new List<ResultOfferDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var offers = JsonConvert.DeserializeObject<List<ResultOfferDto>>(json);

            return View(offers);
        }

        public async Task<IActionResult> Available()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Company");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7034/api/Offer/for-company"); // 🔗 doğru endpoint

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Teklifler alınamadı.";
                return View(new List<ResultOfferDto>());
            }

            var json = await response.Content.ReadAsStringAsync();
            var offers = JsonConvert.DeserializeObject<List<ResultOfferDto>>(json);

            return View(offers);
        }
        [HttpPost]
        public async Task<IActionResult> Assign(string offerId)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Company");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(new { OfferId = offerId }), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7034/api/Offer/assign", content);

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Teklif atanamadı.";
            else
                TempData["Success"] = "Teklif başarıyla alındı.";

            return RedirectToAction("Available");
        }

    }
}

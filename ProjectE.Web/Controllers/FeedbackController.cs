using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectE.DTO.FeedbackDtos;
using System.Text;

namespace ProjectE.Web.Controllers
{
    public class FeedbackController : Controller
    {
        private readonly IHttpClientFactory _http;

        public FeedbackController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory;
        }

        public async Task<IActionResult> CompanyPanel()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Company");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7034/api/Feedback/panel-data");

            if (!response.IsSuccessStatusCode)
                return View(new CompanyFeedbackPanelDto());

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<CompanyFeedbackPanelDto>(json);

            return View(data);
        }
        [HttpPost]
        public async Task<IActionResult> ReplyToFeedback(ReplyToFeedbackDto dto)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Company");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7034/api/Feedback/reply", content);

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = "Cevap eklenemedi.";
            else
                TempData["Success"] = "Cevap başarıyla eklendi.";

            return RedirectToAction("CompanyPanel");
        }
        [HttpGet]
        public async Task<IActionResult> MyComments()
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("https://localhost:7034/api/Feedback/by-user");

            if (!response.IsSuccessStatusCode)
                return View(new List<ResultFeedbackDto>());

            var json = await response.Content.ReadAsStringAsync();
            var feedbacks = JsonConvert.DeserializeObject<List<ResultFeedbackDto>>(json);

            return View(feedbacks);
        }
        [HttpPost]
        public async Task<IActionResult> React(string feedbackId, bool isLike)
        {
            var token = HttpContext.Session.GetString("token");
            if (string.IsNullOrEmpty(token))
                return RedirectToAction("Login", "Account");

            var client = _http.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var body = JsonConvert.SerializeObject(new
            {
                FeedbackId = feedbackId,
                IsLike = isLike
            });

            var content = new StringContent(body, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7034/api/Feedback/react", content);

            if (response.IsSuccessStatusCode)
                TempData["Success"] = "Tepkiniz kaydedildi.";
            else
                TempData["Error"] = "Tepkiniz daha önce verilmiş olabilir.";

            return RedirectToAction("MyComments"); // 👈 ya da CompanyPanel'e yönlendirebilirsin
        }
    }
}

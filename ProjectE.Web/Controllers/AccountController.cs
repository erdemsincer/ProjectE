using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectE.DTO.UserDtos;
using System.Text;

namespace ProjectE.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AccountController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUserDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7034/api/Auth/login", content);

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Giriş başarısız.";
                return View();
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse>(resultJson);

            HttpContext.Session.SetString("token", result.Token);

            return RedirectToAction("Dashboard");
        }

        public IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("token") == null)
                return RedirectToAction("Login");

            return View(); // → Views/Account/Dashboard.cshtml
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login");
        }
    }
}

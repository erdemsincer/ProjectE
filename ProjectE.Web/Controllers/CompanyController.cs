﻿using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectE.DTO.CompanyDtos;
using ProjectE.DTO.UserDtos;
using System.Text;

namespace ProjectE.Web.Controllers
{
    public class CompanyController : Controller
    {
        private readonly IHttpClientFactory _http;

        public CompanyController(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginCompanyDto dto)
        {
            var client = _http.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(dto), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7034/api/Company/login", content);
            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Firma girişi başarısız!";
                return View();
            }

            var resultJson = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<LoginResponse>(resultJson);

            HttpContext.Session.SetString("token", result.Token);

            return RedirectToAction("Panel");
        }

        public IActionResult Panel()
        {
            if (HttpContext.Session.GetString("token") == null)
                return RedirectToAction("Login");

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("token");
            return RedirectToAction("Login");
        }
    }
}

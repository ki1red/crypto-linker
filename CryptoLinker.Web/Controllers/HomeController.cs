using CryptoLinker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;

namespace CryptoLinker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public async Task<JsonResult> GetBinanceStatus()
        {
            var service = new BinanceStatusService();
            bool isOnline;
            long serverTime = 0;

            try
            {
                isOnline = await service.PingAsync();
                if (isOnline)
                    serverTime = await service.GetServerTimeAsync();
            }
            catch (Exception ex) // TODO нужно ли?
            {
                isOnline = false;
            }

            return Json(new { isOnline, serverTime });
        }
    }

    public class BinanceStatusService
    {
        private const string URL = "https://api.binance.com";
        private const string ForPing = "/api/v3/ping";
        private const string ForTestTime = "api/v3/time";

        private readonly HttpClient _httpClient;

        public BinanceStatusService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(URL)
            };
        }

        // —читаем врем€ доступности до бинанса
        public async Task<long> GetServerTimeAsync()
        {
            var response = await _httpClient.GetAsync(ForTestTime);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            return doc.RootElement.GetProperty("serverTime").GetInt64();
        }

        // ѕровер€ем доступность бинанса
        public async Task<bool> PingAsync()
        {
            var response = await _httpClient.GetAsync(ForPing);
            return response.IsSuccessStatusCode;
        }
    }
}

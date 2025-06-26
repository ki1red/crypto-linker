using CryptoLinker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CryptoLinker.Core.Services;
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
            // TODO
            // ������������ ������� ����� ������
            // ����������� �������� ������ ������
            // ������������ ���� � �������
            // �������� ����������� ������ ���������� ������ � ��������

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
        // ��������� ����������� Binance � �������� ����� �������
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
            catch
            {
                isOnline = false;
            }

            return Json(new { isOnline, serverTime });
        }
    }
}

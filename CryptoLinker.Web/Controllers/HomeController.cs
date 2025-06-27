using CryptoLinker.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CryptoLinker.Core.Services;
using System.Text.Json;
using CryptoLinker.Core.Models;

namespace CryptoLinker.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private static List<PortfolioItem> _portfolioItems = new()
        {
            new PortfolioItem { Symbol = "BTC", Amount = 1m },
            new PortfolioItem { Symbol = "XRP", Amount = 15000m },
            new PortfolioItem { Symbol = "XMR", Amount = 50m },
            new PortfolioItem { Symbol = "DASH", Amount = 30m }
        };

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index() // асинхронный стал
        {
            var calculator = new PortfolioCalculator(_portfolioItems);
            var result = await calculator.CalculateAsync();

            var model = new PortfolioViewModel
            {
                Items = _portfolioItems,
                Calculated = result
            };
            // TODO
            // добавить возможность мен€ть количество валюты в портфеле

            return View(model);
        }

        public ActionResult Edit()
        {
            return View(new PortfolioViewModel { Items = _portfolioItems });
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
        // ѕровер€ем доступность Binance и получаем врем€ сервера
        public async Task<JsonResult> GetRialtoStatus()
        {
            var service = new RialtoStatusService();
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

        [HttpGet]
        public async Task<JsonResult> GetRates()
        {
            var calculator = new PortfolioCalculator(_portfolioItems);
            var result = await calculator.CalculateAsync();
            return Json(result);
        }

        [HttpPost]
        public ActionResult Save(List<PortfolioItem> items)
        {
            _portfolioItems = items;
            return RedirectToAction("Index");
        }
    }
}

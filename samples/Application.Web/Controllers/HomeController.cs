using System;
using System.Threading.Tasks;
using Compradon.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly WarehouseManager<Guid> _warehouse;

        private ILogger _logger;

        public HomeController(
            WarehouseManager<Guid> warehouse,
            ILogger<HomeController> logger)
        {
            _warehouse = warehouse;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}

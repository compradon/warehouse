using System;
using System.Threading.Tasks;
using Application.Entities;
using Application.Models;
using Compradon.Warehouse;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Application.Controllers
{
    public class HomeController : Controller
    {
        private readonly WarehouseManager _warehouse;

        private ILogger _logger;

        public HomeController(
            WarehouseManager warehouse,
            ILogger<HomeController> logger)
        {
            _warehouse = warehouse;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _warehouse.FindAsync<Order>();

            return View(orders);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _warehouse.NewAsync<Order>();

                order.Number = DateTime.UtcNow.Ticks;
                order.Price = model.Price;
                order.Comment = model.Comment;

                var result = await _warehouse.SaveAsync(order);
            }

            return View(model);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> Update(Guid orderId) => View(new OrderViewModel(await _warehouse.FindByIdAsync<Order>(orderId)));

        [HttpPost("{orderId}")]
        public async Task<IActionResult> Update(Guid orderId, OrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                var order = await _warehouse.FindByIdAsync<Order>(orderId);

                order.Price = model.Price;
                order.Comment = model.Comment;

                var result = await _warehouse.SaveAsync(order);

                if (result.Succeeded) return RedirectToAction(nameof(Index));

                foreach (var item in result.Errors)
                    ModelState.AddModelError(item.Code, item.Description);
            }

            return View(model);
        }

        [HttpPost("{orderId}")]
        public async Task<IActionResult> Delete(Guid orderId)
        {
            var result = await _warehouse.DeleteAsync(orderId);

            if (result.Succeeded) return RedirectToAction(nameof(Index));

            foreach (var item in result.Errors)
                ModelState.AddModelError(item.Code, item.Description);

            return View();
        }
    }
}

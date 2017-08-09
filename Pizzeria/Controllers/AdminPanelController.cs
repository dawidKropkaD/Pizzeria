using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pizzeria.Data;
using Pizzeria.Models;
using Microsoft.AspNetCore.Authorization;
using Pizzeria.ViewModels;

namespace Pizzeria.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminPanelController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Profits()
        {
            ProfitsViewModel profitsVM = new ProfitsViewModel();
            var completedOrderList = _context.Order
                .Where(x => x.Completed == true)
                .Select(x => new { x.Date, x.Value, x.Profit })
                .ToList();

            if (completedOrderList == null || completedOrderList.Count() == 0)
            {
                return View("NoProfits");
            }

            for (int i = 0; i < completedOrderList.Count(); i++)
            {
                profitsVM.ProfitList.Add(new ProfitsViewModel.Profit(
                    completedOrderList[i].Date, completedOrderList[i].Value, completedOrderList[i].Profit));
            }
            profitsVM.PaidForOrderSum = profitsVM.ProfitList.Select(x => x.PaidForOrder).Sum();
            profitsVM.ProfitSum = profitsVM.ProfitList.Select(x => x.OrderProfit).Sum();

            return View(profitsVM);
        }
    }
}
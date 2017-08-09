using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Pizzeria.Data;
using Pizzeria.Models;
using Pizzeria.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;

namespace Pizzeria.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public MenuController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: Menu
        public async Task<IActionResult> Index()
        {
            List<ProductDb> productDbList = await _context.ProductDb.Where(x => x.IsInLocal == true).ToListAsync();
            MenuBusinessLayer menuBL = new MenuBusinessLayer(_context);
            List<Product> menu = menuBL.ConvertProductDbToProduct(productDbList);

            MenuViewModel menuVM = new MenuViewModel();
            menuVM.Menu = menu;
            menuVM.Categories = menu.Select(x => x.Category).Distinct().ToList();
            menuVM.CategoriesId = menuVM.Categories.Select(x => Regex.Replace(x, @"\s+", "")).ToList();
            menuVM.CanOrderProduct = false;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                menuVM.UserRoles = new List<string>(roles);
            }
            
            return View(menuVM);
        }


        public async Task<IActionResult> OnlineMenu()
        {
            List<ProductDb> productDbList = await _context.ProductDb.Where(x => x.IsOnline == true).ToListAsync();
            MenuBusinessLayer menuBL = new MenuBusinessLayer(_context);
            List<Product> menu = menuBL.ConvertProductDbToProduct(productDbList);

            MenuViewModel menuVM = new MenuViewModel();
            menuVM.Menu = menu;
            menuVM.Categories = menu.Select(x => x.Category).Distinct().ToList();
            menuVM.CategoriesId = menuVM.Categories.Select(x => Regex.Replace(x, @"\s+", "")).ToList();
            menuVM.CanOrderProduct = true;

            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                var roles = await _userManager.GetRolesAsync(user);
                menuVM.UserRoles = new List<string>(roles);
            }

            return View(menuVM);
        }


        // GET: Menu/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(string productName, string category, decimal price)
        {
            if (productName == null || category == null)
            {
                return NotFound();
            }

            var productDb = await _context.ProductDb.SingleOrDefaultAsync(m => m.ProductName.Equals(productName) && m.Category.Equals(category) && m.Price == price);
            if (productDb == null)
            {
                return NotFound();
            }

            return View(productDb);
        }


        // GET: Menu/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }


        // POST: Menu/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("ID,ProductName,Category,SubCategory,Components,Price,Size,Weight,IsInLocal,IsOnline,Profit")] ProductDb productDb)
        {
            if (ModelState.IsValid)
            {
                _context.Add(productDb);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productDb);
        }


        // GET: Menu/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string productName, string category, decimal price)
        {
            if (productName == null || category == null)
            {
                return NotFound();
            }

            var productDb = await _context.ProductDb.SingleOrDefaultAsync(m => m.ProductName.Equals(productName) && m.Category.Equals(category) && m.Price == price);
            if (productDb == null)
            {
                return NotFound();
            }

            return View(productDb);
        }


        // POST: Menu/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("ID,ProductName,Category,SubCategory,Components,Price,Size,Weight,IsInLocal,IsOnline,Profit")] ProductDb productDb)
        {
            if (id != productDb.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(productDb);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductDbExists(productDb.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index");
            }
            return View(productDb);
        }


        // GET: Menu/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string productName, string category, decimal price)
        {
            if (productName == null || category == null)
            {
                return NotFound();
            }

            var productDb = await _context.ProductDb.SingleOrDefaultAsync(m => m.ProductName.Equals(productName) && m.Category.Equals(category) && m.Price == price);
            if (productDb == null)
            {
                return NotFound();
            }

            return View(productDb);
        }


        // POST: Menu/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var productDb = await _context.ProductDb.SingleOrDefaultAsync(m => m.ID == id);
            _context.ProductDb.Remove(productDb);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        private bool ProductDbExists(int id)
        {
            return _context.ProductDb.Any(e => e.ID == id);
        }
    }
}

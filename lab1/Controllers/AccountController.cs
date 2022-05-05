using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using lab1.Models;
using lab1.Dto;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace lab1.Controllers
{
    public class AccountController : Controller
    {
        private PhotostudioInnoDbContext db; 

        public AccountController(PhotostudioInnoDbContext context)
        {
            db = context;
        }
        
        private async Task Authenticate(string userName, string email ,string roleName)
        {
            // создаем один claim
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName ),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, roleName),
                new Claim(ClaimTypes.Email, email)
            };
            // создаем объект ClaimsIdentity
            ClaimsIdentity id = new ClaimsIdentity(claims, "UserDataCookies", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
            // установка аутентификационных куки
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }
        
        [HttpGet]
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterModel model)
        {
            int userCount = db.Users.Where(u => u.Email == model.Email).Count();
            if (userCount == 0)
            {
                User user = new User
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    PhoneNumber = model.PhoneNumber,
                    Password = model.Password,
                    Email = model.Email,
                    FkRole = 1                    
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
                await Authenticate(user.Name, user.Email, "User");
                return RedirectToAction("Index", "Home");
            }
            else
                ModelState.AddModelError("", "Некорректные логин и(или) пароль");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = db.Users
                .Where(u => u.Email == model.Email && u.Password == model.Password)
                .Include(u => u.FkRoleNavigation)
                .FirstOrDefault();
            
            if (user != null)
            {
                await Authenticate(user.Name,user.Email, user.FkRoleNavigation.Name);
                return RedirectToAction("UserAdministrationPanel", "Manage");
            }
            return View(model);
        }
        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
        
    }
}
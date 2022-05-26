using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebExcelCreate.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _singInManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            //_userManager.CreateAsync(new IdentityUser { UserName = "deneme41", Email = "deneme41@gmail.com" }, "deneme41").Wait();
            //var user = new IdentityUser { UserName = "Deneme41Berat", Email = "deneme41berat@hotmail.com" };
            //var result = await _userManager.CreateAsync(user, "Deneme.41Berat");

            if (user == null)
            {
                return View();
            }

            var signInResult = await _singInManager.PasswordSignInAsync(user, password, true, false);

            if (!signInResult.Succeeded)
            {
                return View();
            }

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}

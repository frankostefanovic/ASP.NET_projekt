using Lab2.RezervacijeProstora.Models;
using Lab2.RezervacijeProstora.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class AccountController : Controller
    {
        private static readonly string[] AllowedRoles = { "Admin", "Manager" };
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            PopulateRoles();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                PopulateRoles(model.Role);
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Email,
                Email = model.Email,
                OIB = model.OIB,
                JMBG = model.JMBG
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(model.Role) && AllowedRoles.Contains(model.Role))
                {
                    await _userManager.AddToRoleAsync(user, model.Role);
                }

                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            PopulateRoles(model.Role);
            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded)
            {
                if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Neispravan email ili lozinka.");
            ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Vanjska prijava nije uspjela: {remoteError}");
                ViewData["ReturnUrl"] = returnUrl;
                ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
                return View(nameof(Login));
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Nije moguce dohvatiti podatke vanjske prijave.");
                ViewData["ReturnUrl"] = returnUrl;
                ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
                return View(nameof(Login));
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider,
                info.ProviderKey,
                isPersistent: false,
                bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrWhiteSpace(email))
            {
                ModelState.AddModelError(string.Empty, "Google racun nije vratio email adresu.");
                ViewData["ReturnUrl"] = returnUrl;
                ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
                return View(nameof(Login));
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new AppUser
                {
                    UserName = email,
                    Email = email,
                    OIB = "00000000000",
                    JMBG = "0000000000000"
                };

                var createResult = await _userManager.CreateAsync(user);

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }

                    ViewData["ReturnUrl"] = returnUrl;
                    ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
                    return View(nameof(Login));
                }
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);

            if (!addLoginResult.Succeeded)
            {
                foreach (var error in addLoginResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                ViewData["ReturnUrl"] = returnUrl;
                ViewBag.ExternalLogins = await _signInManager.GetExternalAuthenticationSchemesAsync();
                return View(nameof(Login));
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void PopulateRoles(string? selectedRole = null)
        {
            ViewBag.Roles = new SelectList(
                new[]
                {
                    new SelectListItem { Text = "Bez role", Value = string.Empty },
                    new SelectListItem { Text = "Admin", Value = "Admin" },
                    new SelectListItem { Text = "Manager", Value = "Manager" }
                },
                "Value",
                "Text",
                selectedRole);
        }
    }
}

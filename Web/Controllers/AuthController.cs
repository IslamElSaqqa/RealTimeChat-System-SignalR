using AdvancedChat.Web.Services;
using AdvancedChat.Web.ViewModels;
using Advanced.Web.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace AdvancedChat.Web.Controllers;

public class AuthController : Controller
{
    private readonly ApiService _api;

    public AuthController(ApiService api)
    {
        _api = api;
    }

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterViewModel vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please fill in all fields correctly";
                return View();
            }

            var result = await _api.PostAsync<AuthResponseDto>(
                "api/auth/register",
                vm);

            if (result == null)
            {
                ViewBag.Error = "Registration failed. Please check your details and try again.";
                return View();
            }

            if (result.Success)
            {
                TempData["SuccessMessage"] = "Registration successful! Please log in.";
                return RedirectToAction("Login");
            }
            else
            {
                ViewBag.Error = result.Message ?? "Registration failed";
                return View();
            }
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Registration error: {ex.Message}";
            Console.WriteLine($"Register Exception: {ex}");
            return View();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginViewModel vm)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Please enter both email and password";
                return View();
            }

            Console.WriteLine($"Attempting login for: {vm.Email}");

            var result = await _api.PostAsync<AuthResponseDto>(
                "api/auth/login",
                vm);

            if (result == null)
            {
                ViewBag.Error = "Login failed. Please check your credentials and try again.";
                return View();
            }

            if (!result.Success)
            {
                ViewBag.Error = result.Message ?? "Invalid login credentials";
                return View();
            }

            if (string.IsNullOrEmpty(result.Token))
            {
                ViewBag.Error = "No token received from server. Please try again.";
                return View();
            }

            Console.WriteLine("Login successful, setting token in session");
            HttpContext.Session.SetString("token", result.Token);

            return RedirectToAction(
                "Index",
                "Chat");
        }
        catch (Exception ex)
        {
            ViewBag.Error = $"Login error: {ex.Message}";
            Console.WriteLine($"Login Exception: {ex}");
            return View();
        }
    }
}

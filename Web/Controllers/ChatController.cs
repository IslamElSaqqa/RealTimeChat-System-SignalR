using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Index()
        {
            // The Chat view reads @ViewBag.Token to seed localStorage with
            // the JWT saved in Session by AuthController.Login. This action
            // never set that property, so ViewBag.Token was always empty:
            // the browser had no token, every API/SignalR call failed, and
            // the page's own script immediately redirected back to
            // /Auth/Login - a fatal loop that made the chat page (and room
            // creation with it) completely unusable after logging in.
            var token = HttpContext.Session.GetString("token");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Auth");
            }

            ViewBag.Token = token;
            return View();
        }
    }
}

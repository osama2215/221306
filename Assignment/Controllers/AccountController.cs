using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(string username, string password)
    {
        // Example login validation
        if (username == "admin" && password == "1234")
        {
            // Redirect to Dashboard page after successful login
            return RedirectToAction("https://localhost:7048/Dashboard");
        }

        // If invalid, show error message
        ViewBag.Error = "Invalid username or password.";
        return View();
    }
}

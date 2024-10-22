using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TipsRundan.Application.DataTransferObjects;
using TipsRundan.Application.Services;
using TipsRundan.Web.Models;

namespace TipsRundan.Web.Controllers;

public class UserController : Controller
{
    private readonly IAccountService _service;
    public UserController(IAccountService service)
    {
        _service = service;
    }
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterUserView model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            var result = _service.CreateAccountAsync(new CreateAccountDTO(){
                username = model.Username,
                password = model.Password,
                email = model.Email
            }).Result;
            if (result)
                return RedirectToAction("Login", "User");
            else
                return View(model);
        }
        catch (Exception e)
        {
            ModelState.AddModelError("CreationFailed", e.InnerException is null ? "Unexpected error." : e.InnerException.Message);
            return View(model);
        }
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginView model)
    {
        if (!ModelState.IsValid) return View(model);
        try
        {
            _service.SigninUserAsync(new LoginDTO(){ Name = model.Username, PasswordString = model.PasswordString });
            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            ModelState.AddModelError("InvalidLogin", e.InnerException is null ? "Unexpected error.": e.InnerException.Message );
            return View(model);
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult Logout()
    {
        _service.SignoutUserAsync();
        return RedirectToAction("Index", "Home");
    }
}
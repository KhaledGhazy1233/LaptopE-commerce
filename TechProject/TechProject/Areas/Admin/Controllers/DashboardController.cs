using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tech.Utility;

[Area("Admin")]
[Authorize(Roles = SD.Role_User_Admin)]
public class DashboardController : Controller
{
    
    public IActionResult Index()
    {
        return View();
    }

}

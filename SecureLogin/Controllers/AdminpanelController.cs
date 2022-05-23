using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecureLogin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminpanelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

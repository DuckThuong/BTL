using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BTL.Data;

namespace BTL.Controllers
{
    public class LoginController : Controller
    {
        private readonly WebNcContext _context;

        public LoginController(WebNcContext context)
        {
            _context = context;
        }

        // GET: Login
        public async Task<IActionResult> Login()
        {
            return View(await _context.Users.ToListAsync());
        }
       
    }
}

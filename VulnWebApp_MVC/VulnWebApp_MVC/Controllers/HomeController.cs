using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using VulnWebApp.Services;
using VulnWebApp_MVC.Models;

namespace VulnWebApp_MVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public JsonFileUserService UserService;
        public OSCommandService CommandService;
        public Dictionary<string, string> Users { get; private set; }
        public string Output { get; private set; }
        public bool IsAdmin { get; private set; }

        public HomeController(
            ILogger<HomeController> logger,
            JsonFileUserService userService,
            OSCommandService commandService)
        {
            _logger = logger;
            UserService = userService;
            CommandService = commandService;
        }

        public IActionResult Index()
        {
            Users = UserService.GetUsers();
            if (Users["password"] != "guest")
            {
                ViewBag.Output = "Password changed";
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string pswd)
        {
            Users = UserService.GetUsers();
            if (pswd != Users["password"])
            {
                ViewBag.Output = "Incorrect password!";
                return View();
            }
            return RedirectToAction("Authorized");
        }

        [HttpGet]
        public IActionResult Authorized()
        {
            Users = UserService.GetUsers();
            if (Users["role"] == "admin")
            {
                IsAdmin = true;
            }
            else
            {
                IsAdmin = false;
            }

            return View();
        }

        [HttpPost]
        public IActionResult Authorized(string cmd)
        {
            CommandService.ExecuteCommand(cmd);
            ViewBag.Output = CommandService.CommandResult;
            return View();
        }

        [HttpGet]
        public IActionResult ChangePasswd()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ChangePasswd(string pswd)
        {
            UserService.ModifyUser(pswd);
            ViewBag.Output = "Password changed";
            return RedirectToAction("Index");
        }

        public IActionResult RestoreData()
        {
            UserService.RestoreUser();
            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

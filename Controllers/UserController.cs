using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BsaHw2.Models;
using BsaHw2.Services;

namespace BsaHw2.Controllers
{
  public class UserController : Controller
  {
    private IBlogService _blogService;

    public UserController(IBlogService blogService)
    {
      this._blogService = blogService;
    }

    public IActionResult Index()
    {
      var model = _blogService.GetUsers();
      return View(model);
    }

    public IActionResult Details(int id)
    {
      var model = _blogService.GetUser(id);
      return View(model);
    }

    public IActionResult About()
    {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public IActionResult Contact()
    {
      ViewData["Message"] = "Your contact page.";

      return View();
    }

    public IActionResult Privacy()
    {
      return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}

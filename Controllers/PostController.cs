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
  public class PostController : Controller
  {
    private IBlogService _blogService;

    public PostController(IBlogService blogService)
    {
      this._blogService = blogService;
    }

    public IActionResult Index()
    {
      var posts = _blogService.GetPosts();

      return View(posts);
    }

    public IActionResult Details(int id)
    {
      var post = _blogService.GetPost(id);

      return View(post);
    }
  }
}

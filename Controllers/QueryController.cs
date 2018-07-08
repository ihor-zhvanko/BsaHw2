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
  public class QueryController : Controller
  {
    private IBlogService _blogService;

    public QueryController(IBlogService blogService)
    {
      this._blogService = blogService;
    }

    public IActionResult FirstQuery([Bind(Prefix = "id")] int? userId)
    {
      if (!userId.HasValue)
      {
        return SelectUser("FirstQuery");
      }

      var user = _blogService.GetUser(userId.Value);
      var model = _blogService.GetCommentsCount(userId.Value);
      return View((user, model));
    }

    public IActionResult SecondQuery([Bind(Prefix = "id")] int? userId)
    {
      if (!userId.HasValue)
      {
        return SelectUser("SecondQuery");
      }

      var user = _blogService.GetUser(userId.Value);
      var model = _blogService.GetCommentsLessThan50Length(userId.Value);
      return View((user, model));
    }

    public IActionResult ThirdQuery([Bind(Prefix = "id")] int? userId)
    {
      if (!userId.HasValue)
      {
        return SelectUser("ThirdQuery");
      }

      var user = _blogService.GetUser(userId.Value);
      var model = _blogService.GetCompletedToDos(userId.Value);
      return View((user, model));
    }

    public IActionResult ForthQuery()
    {
      var model = _blogService.GetUsersSorted();
      return View(model);
    }

    public IActionResult FifthQuery([Bind(Prefix = "id")] int? userId)
    {
      if (!userId.HasValue)
      {
        return SelectUser("FifthQuery");
      }

      var model = _blogService.GetUserStats(userId.Value);
      return View(model);
    }

    public IActionResult SixthQuery([Bind(Prefix = "id")] int? postId)
    {
      if (!postId.HasValue)
      {
        return SelectPost("SixthQuery");
      }

      var model = _blogService.GetPostStats(postId.Value);
      return View(model);
    }

    public IActionResult SelectUser(string redirect)
    {
      var users = _blogService.GetUsers();

      return View("SelectUser", (users, redirect));
    }

    public IActionResult SelectPost(string redirect)
    {
      var posts = _blogService.GetPosts();

      return View("SelectPost", (posts, redirect));
    }
  }
}

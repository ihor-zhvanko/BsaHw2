using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BsaHw2.Connectors;
using BsaHw2.Models;
using Newtonsoft.Json;

namespace BsaHw2.Services
{
  public interface IBlogService
  {
    IList<(Post, int)> GetCommentsCount(int userId);
    IList<string> GetCommentsLessThan50Length(int userId);
    IList<(int, string)> GetCompletedToDos(int userId);
    IList<User> GetUsersSorted();
    UserStats GetUserStats(int userId);
    PostStats GetPostStats(int postId);

    IList<User> GetUsers();
    User GetUser(int id);
    Post GetUserPost(int userId, int postId);

    IList<Post> GetPosts();
    Post GetPost(int postId);
  }

  public class BlogService : IBlogService
  {
    private IBlogConnector _connector;
    private Lazy<IList<User>> _cachedUsers;

    public BlogService(IBlogConnector connector)
    {
      this._connector = connector;

      // Just To Make It Work Faster
      _cachedUsers = new Lazy<IList<User>>(() => LoadData().Result);
    }

    protected async Task<IList<User>> LoadData()
    {
      var usersTask = _connector.GetUsersAsync();
      var postsTask = _connector.GetPostsAsync();
      var commentsTask = _connector.GetCommentsAsync();
      var addressesTask = _connector.GetAddressesAsync();
      var toDosTask = _connector.GetToDosAsync();

      return BuildDataSet(
        await usersTask, await postsTask, await commentsTask,
        await addressesTask, await toDosTask
      );
    }

    protected IList<User> BuildDataSet(
      IList<User> usersResponce, IList<Post> postsResponce, IList<Comment> comments,
      IList<Address> addresses, IList<ToDo> todos
    )
    {
      var posts = from p in postsResponce
                  join c in comments
                    on p.Id equals c.PostId into postComments
                  select Post.Create(p, postComments);

      var users = from u in usersResponce
                  join p in posts
                    on u.Id equals p.UserId into userPosts
                  join a in addresses
                    on u.Id equals a.UserId into userAddress
                  join t in todos
                    on u.Id equals t.UserId into userToDos
                  select User.Create(u, userPosts, userToDos, userAddress);

      return users.ToList();
    }

    public IList<(Post, int)> GetCommentsCount(int userId)
    {
      var users = _cachedUsers.Value;

      var userPosts = users.FirstOrDefault(x => x.Id == userId)
                           ?.Posts
                           .Select(x => (x, x.Comments.Count));

      return userPosts?.ToList();
    }

    public IList<string> GetCommentsLessThan50Length(int userId)
    {
      var users = _cachedUsers.Value;

      var comments = users.FirstOrDefault(x => x.Id == userId)
                          ?.Posts
                          .SelectMany(x => x.Comments)
                          .Where(x => x.Body.Length < 50)
                          .Select(x => x.Body);

      return comments?.ToList();
    }

    public IList<(int, string)> GetCompletedToDos(int userId)
    {
      var users = _cachedUsers.Value;

      var todos = users.FirstOrDefault(x => x.Id == userId)
                       ?.ToDos
                       .Where(x => x.IsComplete)
                       .Select(x => (x.Id, x.Name));

      return todos?.ToList();
    }

    public IList<User> GetUsersSorted()
    {
      var users = _cachedUsers.Value;

      var sortedOne = users.OrderBy(x => x.Name).Select(x => new User(x)
      {
        ToDos = x.ToDos.OrderByDescending(t => t.Name.Length).ToList()
      });

      return sortedOne.ToList();
    }

    public UserStats GetUserStats(int userId)
    {
      var user = _cachedUsers.Value.FirstOrDefault(x => x.Id == userId);

      if (user == null)
        return null;

      var userStats = new UserStats(user);
      userStats.LastPost = user.Posts.OrderByDescending(x => x.CreatedAt).FirstOrDefault();
      userStats.LastPostCommentsCount = userStats.LastPost?.Comments.Count;
      userStats.IncompletedTasksCount = user.ToDos.Where(x => !x.IsComplete).Count();
      userStats.TheMostCommentsPost = user.Posts.OrderByDescending(x =>
        x.Comments.Where(c => c.Body.Length > 80).Count()
      ).FirstOrDefault();
      userStats.PopularPost = user.Posts.OrderByDescending(x => x.Likes).FirstOrDefault();

      return userStats;
    }

    public PostStats GetPostStats(int postId)
    {
      var post = _cachedUsers.Value.SelectMany(x => x.Posts).FirstOrDefault(x => x.Id == postId);

      if (post == null)
        return null;

      var postStats = new PostStats(post);
      postStats.TheLongestComment = post.Comments.OrderByDescending(x => x.Body.Length).FirstOrDefault();
      postStats.PopularComment = post.Comments.OrderByDescending(x => x.Likes).FirstOrDefault();
      postStats.ShortOrNotPopularComments = post.Comments.Where(x => x.Likes == 0 || x.Body.Length < 80).Count();

      return postStats;
    }

    public IList<User> GetUsers()
    {
      return _cachedUsers.Value;
    }

    public User GetUser(int id)
    {
      return _cachedUsers.Value.FirstOrDefault(x => x.Id.Equals(id));
    }

    public Post GetUserPost(int userId, int postId)
    {
      return GetUser(userId)?.Posts.FirstOrDefault(x => x.Id.Equals(postId));
    }

    public IList<Post> GetPosts()
    {
      return _cachedUsers.Value.SelectMany(x => x.Posts).ToList();
    }

    public Post GetPost(int postId)
    {
      return GetPosts().FirstOrDefault(x => x.Id.Equals(postId));
    }
  }
}

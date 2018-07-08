using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using BsaHw2.Models;
using Newtonsoft.Json;

namespace BsaHw2.Connectors
{
  public interface IBlogConnector
  {
    Task<IList<User>> GetUsersAsync();
    Task<IList<Post>> GetPostsAsync();
    Task<IList<Comment>> GetCommentsAsync();
    Task<IList<ToDo>> GetToDosAsync();
    Task<IList<Address>> GetAddressesAsync();
  }

  public class BlogConnector : IBlogConnector
  {
    private const string BASE_URL = "https://5b128555d50a5c0014ef1204.mockapi.io/";

    // data is static on mockserver. Just Make It Faster!
    private IList<User> _cashedUsers = null;
    private IList<Post> _cashedPosts = null;
    private IList<Comment> _cashedComments = null;
    private IList<ToDo> _cashedToDos = null;
    private IList<Address> _cashedAddress = null;

    public async Task<IList<User>> GetUsersAsync()
    {
      if (_cashedUsers == null)
      {
        _cashedUsers = await Get<IList<User>>("users");
      }

      return _cashedUsers;
    }

    public async Task<IList<Post>> GetPostsAsync()
    {
      if (_cashedPosts == null)
      {
        _cashedPosts = await Get<IList<Post>>("posts");
      }

      return _cashedPosts;
    }

    public async Task<IList<Comment>> GetCommentsAsync()
    {
      if (_cashedComments == null)
      {
        _cashedComments = await Get<IList<Comment>>("comments");
      }

      return _cashedComments;
    }

    public async Task<IList<ToDo>> GetToDosAsync()
    {
      if (_cashedToDos == null)
      {
        _cashedToDos = await Get<IList<ToDo>>("todos");
      }

      return _cashedToDos;
    }

    public async Task<IList<Address>> GetAddressesAsync()
    {
      if (_cashedAddress == null)
      {
        _cashedAddress = await Get<IList<Address>>("address");
      }

      return _cashedAddress;
    }

    private async Task<T> Get<T>(string url)
    {
      var client = new HttpClient();
      client.BaseAddress = new Uri(BASE_URL);
      var data = await client.GetStringAsync(url);

      return JsonConvert.DeserializeObject<T>(data);
    }
  }
}

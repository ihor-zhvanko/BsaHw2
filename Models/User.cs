using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BsaHw2.Models
{
  public class User
  {
    public User() { }
    public User(User user)
    {
      Id = user.Id;
      CreatedAt = user.CreatedAt;
      Name = user.Name;
      AvatarUrl = user.AvatarUrl;
      Email = user.Email;

      Posts = user.Posts;
      ToDos = user.ToDos;
      Addresses = user.Addresses;
    }

    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    [JsonProperty("avatar")]
    public string AvatarUrl { get; set; }
    public string Email { get; set; }

    [JsonIgnore]
    public IList<Post> Posts { get; set; }
    [JsonIgnore]
    public IList<ToDo> ToDos { get; set; }
    [JsonIgnore]
    public IList<Address> Addresses { get; set; }

    public static User Create(User user, IEnumerable<Post> posts, IEnumerable<ToDo> toDos, IEnumerable<Address> addresses)
    {
      var newUser = new User(user);
      newUser.Posts = posts.ToList();
      newUser.ToDos = toDos.ToList();
      newUser.Addresses = addresses.ToList();

      return newUser;
    }
  }
}
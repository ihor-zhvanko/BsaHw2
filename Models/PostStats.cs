using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BsaHw2.Models
{

  public class PostStats : Post
  {
    public PostStats(Post post)
    {
      Id = post.Id;
      CreatedAt = post.CreatedAt;
      Title = post.Title;
      Body = post.Body;
      UserId = post.UserId;
      Likes = post.Likes;

      Comments = post.Comments;
    }
    public Comment TheLongestComment { get; set; }
    public Comment PopularComment { get; set; }
    public int ShortOrNotPopularComments { get; set; }
  }
}
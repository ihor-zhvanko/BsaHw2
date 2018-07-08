using System;
using System.Linq;
using System.Collections.Generic;

namespace BsaHw2.Models
{
  public class Post
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
    public IList<Comment> Comments { get; set; }

    public static Post Create(Post post, IEnumerable<Comment> comments)
    {
      var newPost = post.MemberwiseClone() as Post;
      newPost.Comments = comments.ToList();

      return newPost;
    }
  }
}
using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace BsaHw2.Models
{
  public class UserStats : User
  {
    public UserStats(User user)
      : base(user) { }
    public Post LastPost { get; set; }
    public int? LastPostCommentsCount { get; set; }
    public int? IncompletedTasksCount { get; set; }
    public Post TheMostCommentsPost { get; set; }
    public Post PopularPost { get; set; }
  }
}
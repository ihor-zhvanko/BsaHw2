using System;
using Newtonsoft.Json;

namespace BsaHw2.Models
{
  public class ToDo
  {
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public bool IsComplete { get; set; }
    public int UserId { get; set; }
  }
}

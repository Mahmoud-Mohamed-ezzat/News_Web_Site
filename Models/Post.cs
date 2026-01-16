using System;
using System.Collections.Generic;
namespace News_App.Models;
public partial class Post
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Post1 { get; set; }
    public List<string>? Image { get; set; } = new List<string>();
    public string ?PublisherId { get; set; }
    public int? CategoryId { get; set; }
    public int? NewspageId { get; set; }
    public User Publisher { get; set; }
    public virtual Category Category { get; set; }
    public virtual Newspage Newspage { get; set; }
}
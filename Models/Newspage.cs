using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace News_App.Models;
public partial class Newspage
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool? Iscreated { get; set; }
    public string? AdminId { get; set; } //for admin of page
    public User ?Admin { get; set; }
    public virtual ICollection<Post>? Posts { get; set; } = new List<Post>();
    public ICollection<User>? Publishers { get; set; } = new List<User>();
}
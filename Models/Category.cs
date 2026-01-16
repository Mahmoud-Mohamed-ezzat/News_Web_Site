using System;
using System.Collections.Generic;

namespace News_App.Models;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}

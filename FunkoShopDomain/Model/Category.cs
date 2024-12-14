using System;
using System.Collections.Generic;

namespace FunkoShopDomain.Model;

public partial class Category
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Figure> Figures { get; set; } = new List<Figure>();
}

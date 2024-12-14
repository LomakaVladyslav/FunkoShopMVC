using System;
using System.Collections.Generic;

namespace FunkoShopDomain.Model;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<Figure> Figures { get; set; } = new List<Figure>();
}

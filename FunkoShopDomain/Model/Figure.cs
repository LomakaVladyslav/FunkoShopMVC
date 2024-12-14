using System;
using System.Collections.Generic;

namespace FunkoShopDomain.Model;

public partial class Figure
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public int? CategoryId { get; set; }

    public int? CountryId { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}

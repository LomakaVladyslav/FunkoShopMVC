using System;
using System.Collections.Generic;

namespace FunkoShopDomain.Model;

public partial class OrderItem
{
    public int Id { get; set; }

    public int? OrderId { get; set; }

    public int? FigureId { get; set; }

    public int Quantity { get; set; }

    public decimal Price { get; set; }

    public virtual Figure? Figure { get; set; }

    public virtual Order? Order { get; set; }
}

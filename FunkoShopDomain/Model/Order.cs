using System;
using System.Collections.Generic;

namespace FunkoShopDomain.Model;

public partial class Order
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}

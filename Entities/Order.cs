using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? FeedbackId { get; set; }

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public string City { get; set; } = null!;

    public int TotalAmount { get; set; }

    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Feedback? Feedback { get; set; }

    public virtual ICollection<OrderImage> OrderImages { get; set; } = new List<OrderImage>();

    public virtual User User { get; set; } = null!;
}

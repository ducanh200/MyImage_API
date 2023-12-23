using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FeedbackId { get; set; }

    public int TotalAmount { get; set; }

    public int Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Feedback Feedback { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();

    public virtual User User { get; set; } = null!;
}

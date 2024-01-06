using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class OrderImage
{
    public int Id { get; set; }

    public int FrameId { get; set; }

    public int HangerId { get; set; }

    public int SizeId { get; set; }

    public int OrderId { get; set; }

    public string Thumbnail { get; set; } = null!;

    public int Quantity { get; set; }

    public int Amount { get; set; }

    public virtual Frame Frame { get; set; } = null!;

    public virtual Hanger Hanger { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual Size Size { get; set; } = null!;
}

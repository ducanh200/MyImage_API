using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Size
{
    public int Id { get; set; }

    public int SizeAmount { get; set; }

    public string SizeName { get; set; } = null!;

    public string SizeWidth { get; set; } = null!;

    public string SizeHeight { get; set; } = null!;

    public virtual ICollection<OrderImage> OrderImages { get; set; } = new List<OrderImage>();
}

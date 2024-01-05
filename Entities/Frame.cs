using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Frame
{
    public int Id { get; set; }

    public int FrameAmount { get; set; }

    public string FrameName { get; set; } = null!;

    public string FrameColorOutsite { get; set; } = null!;

    public string FrameColorInsite { get; set; } = null!;

    public virtual ICollection<OrderImage> OrderImages { get; set; } = new List<OrderImage>();
}

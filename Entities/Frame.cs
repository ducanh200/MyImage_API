using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Frame
{
    public int Id { get; set; }

    public int FrameAmount { get; set; }

    public string FrameColor { get; set; } = null!;

    public string FrameName { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}

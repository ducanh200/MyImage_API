using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Color
{
    public int Id { get; set; }

    public int ColorAmount { get; set; }

    public string ColorName { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}

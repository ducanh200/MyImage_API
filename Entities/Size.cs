using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Size
{
    public int Id { get; set; }

    public int SizeAmount { get; set; }

    public string SizeName { get; set; } = null!;

    public int SizeWidth { get; set; }

    public int SizeHeight { get; set; }

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}

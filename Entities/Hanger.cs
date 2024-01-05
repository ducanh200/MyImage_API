using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Hanger
{
    public int Id { get; set; }

    public int HangerAmount { get; set; }

    public string HangerName { get; set; } = null!;

    public virtual ICollection<OrderImage> OrderImages { get; set; } = new List<OrderImage>();
}

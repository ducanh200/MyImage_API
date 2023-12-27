using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Material
{
    public int Id { get; set; }

    public int MaterialAmount { get; set; }

    public string MaterialName { get; set; } = null!;

    public virtual ICollection<Image> Images { get; set; } = new List<Image>();
}

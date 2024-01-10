using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class ImageUrl
{
    public int Id { get; set; }

    public string Thumbnail { get; set; } = null!;
}

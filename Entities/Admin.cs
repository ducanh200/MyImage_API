﻿using System;
using System.Collections.Generic;

namespace MyImage_API.Entities;

public partial class Admin
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
}

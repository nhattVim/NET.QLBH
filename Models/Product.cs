using System;
using System.Collections.Generic;

namespace QLBH.Models;

public partial class Product
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public string Images { get; set; } = null!;

    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}

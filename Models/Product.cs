using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace QLBH.Models;

public partial class Product
{
    public int Id { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Ten la bat buoc")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Ten phai tu 1 den 256 ki tu")]
    public string Name { get; set; } = null!;

    [BindProperty]
    [Required(ErrorMessage = "Mo ta la bat buoc")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Mo ta phai tu 1 den 500 ki tu")]
    public string? Description { get; set; }

    public List<string> Images { get; set; } = new List<string>();

    [BindProperty]
    [Required(ErrorMessage = "Gia la bat buoc")]
    [Range(0, int.MaxValue, ErrorMessage = "Gia phai la so nguyen duong")]
    public decimal Price { get; set; }

    public int Stock { get; set; }

    public int? CategoryId { get; set; }

    public virtual Category? Category { get; set; }
}

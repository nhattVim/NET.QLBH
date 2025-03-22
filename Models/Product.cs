using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Models;

public class Product
{
    public int Id { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Ten la bat buoc")]
    [StringLength(256, MinimumLength = 1, ErrorMessage = "Ten phai tu 1 den 256 ki tu")]
    public string Name { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Mo ta la bat buoc")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "Mo ta phai tu 1 den 500 ki tu")]
    public string Description { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Gia la bat buoc")]
    [Range(0, int.MaxValue, ErrorMessage = "Gia phai la so nguyen duong")]
    public int? Price { get; set; }

    public List<string> ImagePaths { get; set; } = new List<string>();
}

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using QLBH.Validation;

namespace QLBH.Models;

public class Contact
{
    [BindProperty]
    [DisplayName("Id cua ban")]
    [Range(1, 100, ErrorMessage = "Nhap sai")]
    public int ContactId { get; set; }

    [BindProperty]
    public string? FirstName { get; set; }

    [BindProperty]
    public string? LastName { get; set; }

    [BindProperty]
    [DataType(DataType.Date)]
    [CustomBirthDate(ErrorMessage = "Ngay sinh khong hop le")]
    public DateTime DateOfBirth { get; set; }

    [BindProperty]
    [EmailAddress(ErrorMessage = "Nhap sai dinh dang")]
    public string? Email { get; set; }
}

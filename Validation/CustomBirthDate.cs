using System;
using System.ComponentModel.DataAnnotations;

namespace QLBH.Validation;

public class CustomBirthDate : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        DateTime dateTime = Convert.ToDateTime(value);
        return dateTime <= DateTime.Now;
    }
}

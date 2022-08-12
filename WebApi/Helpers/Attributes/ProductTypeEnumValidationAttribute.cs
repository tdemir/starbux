using System.ComponentModel.DataAnnotations;
using WebApi.Enums;

namespace WebApi.Helpers.Attributes;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
public class ProductTypeEnumValidationAttribute : ValidationAttribute
{
    private const string _defaultErrorMessage = "'{0}' is not valid. Accepted values are ";


    public ProductTypeEnumValidationAttribute() : base(_defaultErrorMessage + string.Join(',', Enum.GetNames<EProductType>()))
    {

    }

    public override string FormatErrorMessage(string name)
    {
        return base.FormatErrorMessage(name);
    }

    public override bool IsValid(object? value)
    {
        if (value == null)
        {
            return false;
        }

        var strVal = value.ToString();

        if (!Enum.TryParse<EProductType>(strVal, true, out _))
        {
            return false;
        }
        return true;

        //return base.IsValid(value);
    }
}
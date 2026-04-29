using System.ComponentModel.DataAnnotations;

namespace Xeno.Starstay.Models
{
    public sealed class MustBeTrueAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            return value is true;
        }
    }
}

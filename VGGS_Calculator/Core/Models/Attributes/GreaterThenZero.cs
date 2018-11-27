using System.ComponentModel.DataAnnotations;

namespace VGGS_Calculator.Core.Models
{
    public class GreaterThenZero: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value is double && (double)value > 0)
                return true;
            return false;
        }
    }
}

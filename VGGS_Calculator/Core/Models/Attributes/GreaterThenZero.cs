using System.ComponentModel.DataAnnotations;

namespace VGGS_Calculator.Core.Models
{
    public class GreaterThenZero: ValidationAttribute
    {
        public GreaterThenZero(string message)
    : base(message) { }
        public GreaterThenZero()
            : base("Value have to be number and greate then 0") { }
        public override bool IsValid(object value)
        {
            if (value is double && (double)value > 0)
                return true;
            return false;
        }
    }
}

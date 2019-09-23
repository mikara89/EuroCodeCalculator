using CalcModels;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VGGS_Calculator.Core.Models
{

    public class ConcreteExist : ValidationAttribute
    { 
        public ConcreteExist(string message)
            :base(message) {}
        public ConcreteExist()
            : base("Concrate class don't exist in database") { }
        public override bool IsValid(object value)
        {
            if (value is string)
            {
                var b = value as string;
                return BetonModelEC.ListOfBetonClasses()
                    .Any(x => x.name == (string)value); ;
            }
            return false;
        }
    }
}

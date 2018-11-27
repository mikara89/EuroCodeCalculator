using System.ComponentModel.DataAnnotations;
using System.Linq;
using TabeleEC2;

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
                return BetonClasses.GetBetonClassListEC()
                    .Any(x => x.name == (string)value); ;
            }
            return false;
        }
    }
}

using System.ComponentModel.DataAnnotations;
using System.Linq;
using TabeleEC2;

namespace VGGS_Calculator.Core.Models
{
    public class ReinforcementExist : ValidationAttribute 
    {
        public ReinforcementExist(string message) 
            :base(message) { }
        public ReinforcementExist()
            : base("Reinforcement type don't exist in database") { }
        public override bool IsValid(object value) 
        {
            if (value is string)
            {
                var b = value as string;
                return ReinforcementType.GetArmatura()
                    .Any(x => x.name == (string)value); ;
            }
            return false;
        }
    }
}

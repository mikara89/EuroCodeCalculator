using System.ComponentModel.DataAnnotations;

namespace CalcBlazor.Data.Models
{
    public class AddForcesModel 
    {
        [Required]
        public double M { get; set; } 
        [Required] 
        public double N { get; set; }
    }
}


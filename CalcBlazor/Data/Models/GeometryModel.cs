using CalcModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Data.Models
{
    public class GeometryModel
    {
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double b { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double As_1 { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double As_2 { get; set; }
        public double b_eff { get; set; }
        public double h_f { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double h { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double d1 { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double d2 { get; set; }

    }
    public class MaterialModel
    {
        [Required]
        public string SelectedReinf { get; set; }
        [Required]
        public string SelectedConcrete { get; set; }
    }
    public class AddForcesModel 
    {
        [Required]
        public double M { get; set; } 
        [Required] 
        public double N { get; set; }
    }
}

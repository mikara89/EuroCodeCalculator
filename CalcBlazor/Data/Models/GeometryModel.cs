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

        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double b_eff_top { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double h_f_top { get; set; }

        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double b_eff_bottom { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double h_f_bottom { get; set; }


        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double h { get; set; }
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double d1 { get; set; }

        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double d2 { get; set; }
        [Required]
        public SectionType SectionType { get; set; }

    }
}


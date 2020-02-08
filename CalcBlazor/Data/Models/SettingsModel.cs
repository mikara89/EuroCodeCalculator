using System.ComponentModel.DataAnnotations;

namespace CalcBlazor.Data.Models
{
    public class SettingsModel
    {
        [Required]
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public double alfa_cc { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [Required]
        public  double alfa_ct { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [Required]
        public  double gama_s { get; set; }
        [Range(0.0, double.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        [Required]
        public  double gama_c { get; set; }
    }
}


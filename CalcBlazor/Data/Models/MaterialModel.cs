using System.ComponentModel.DataAnnotations;

namespace CalcBlazor.Data.Models
{
    public class MaterialModel
    {
        [Required]
        public string SelectedReinf { get; set; }
        [Required]
        public string SelectedConcrete { get; set; }
    }
}


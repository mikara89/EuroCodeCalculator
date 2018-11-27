using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VGGS_Calculator.Core.Models
{
    public class VitkostModel 
    {
        
        [JsonProperty(PropertyName = "Slenderness")]
        public string Slenderness { get; set; }
        [Required]
        [GreaterThenZero]
        public double k { get; set; }
        [Required]
        [JsonProperty(PropertyName = "N")]
        public double N { get; set; }
        [Required]
        [JsonProperty(PropertyName = "M_top")]
        public double M_top { get; set; }
        [Required]
        [JsonProperty(PropertyName = "M_bottom")]
        public double M_bottom { get; set; }
        [Required]
        [GreaterThenZero]
        [JsonProperty(PropertyName = "L")]
        public double L { get; set; }
        [Required]
        [GreaterThenZero]
        public double b { get; set; }
        [Required]
        [GreaterThenZero]
        public double h { get; set; }
        [Required]
        [GreaterThenZero]
        public double d1 { get; set; }
        [Required]
        [ReinforcementExist]
        public string armtype { get; set; }
        [Required]
        [ConcreteExist]
        public string betonClass { get; set; }
        public string result { get; set; }
    }
}

using CalcModels;
using System.ComponentModel.DataAnnotations;

namespace VGGS_Calculator.Core.Models
{
    public class SavijanjePravougaonogPresekaEC2Model  
    {
        [Required]
        [GreaterThenZero]
        public double b { get; set; }
        [Required]
        public double h { get; set; } 
        [Required]
        [GreaterThenZero] 
        public double d1 { get; set; }
        [Required] 
        [GreaterThenZero]
        public double d2 { get; set; }
        [Required]
        [ReinforcementExist]
        public string armtype { get; set; }
        [Required]
        [ConcreteExist]
        public string betonClass { get; set; }
        public double Mg { get; set; }
        public double Mq { get; set; }
        public double Msd { get; set; }
        public double Ng { get; set; }
        public double Nq { get; set; }
        public double Nsd { get; set; }
        public double mu { get; set; }
        public ResultModel result { get; set; }
        

        public class ResultModel
        {
            public double Msds { get; set; } 
            public CoeffForCalcRectCrossSectionModelEC kof { get; set; }  
            public double As1_pot { get; set; }
            public double As2_pot { get; set; }
            public double μSd { get; set; } 
            public double Msd { get; internal set; }
            public double Nsd { get; internal set; }
            public string Result { get; set; }

        }
    }
    public class MaterialModel 
    {
        [Required]
        [GreaterThenZero]
        public double b { get; set; }
        [Required]
        public double h { get; set; }
        [Required]
        [GreaterThenZero]
        public double d1 { get; set; }
        [Required]
        [GreaterThenZero]
        public double d2 { get; set; }
        [Required]
        [ReinforcementExist]
        public string armtype { get; set; }
        [Required]
        [ConcreteExist]
        public string betonClass { get; set; }
    }
}
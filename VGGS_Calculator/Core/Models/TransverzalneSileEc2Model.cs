using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace VGGS_Calculator.Core.Models
{

    public class TransverzalneSileEc2Model
    {
        public double Vg { get; set; }
        public double Vq { get; set; }
        public double Ved { get; set; }
        [Required]
        [GreaterThenZero]
        public double b { get; set; }
        [Required]
        [GreaterThenZero]
        public double h { get; set; }
        [Required]
        [GreaterThenZero]
        public double d1 { get; set; }
        public ArmHolder armLongitudinal { get; set; }
        [Required]
        [ReinforcementExist]
        public string armtype { get; set; }
        [Required]
        [ConcreteExist]
        public string betonClass { get; set; }
        public double s { get; set; }
        public int m { get; set; }
        public int u_diametar { get; set; }
        public ArmHolder addArm { get; set; }
    }
    public class TransverzalneSileEc2ResultModel
    {
        public double addArm_usv { get; set; } 
        public double addArm_pot { get; set; }
        public double TransArm_pot { get; set; } 
        public double minArm_pot { get; set; }
        public double minArm_usv { get; set; }
        public double s { get; set; }
        public List<double> ListS { get; set; }  
        public int m { get; set; }
        public List<int> ListM { get; set; }
        public int u_diametar { get; set; }
        public string Result { get; set; }
        public double IskorArm { get; internal set; }
        public double IskorBeton { get; internal set; }
        public List<string> Errors { get; internal set; }
    }
    public class ArmHolder 
    {
        public int kom { get; set; }
        public int diametar { get; set; }
    }
}
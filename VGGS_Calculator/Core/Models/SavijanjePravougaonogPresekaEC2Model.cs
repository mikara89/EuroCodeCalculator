namespace VGGS_Calculator.Core.Models
{
    public class SavijanjePravougaonogPresekaEC2Model  
    {
        public double b { get; set; }
        public double h { get; set; }
        public double d1 { get; set; }
        public double d2 { get; set; }
        public string armtype { get; set; }
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
            public TabeleEC2.Model.KofZaProracunPravougaonogPresekaModelEC kof { get; set; }  
            public double As1_pot { get; set; }
            public double As2_pot { get; set; }
            public double μSd { get; set; } 
            public double Msd { get; internal set; }
            public double Nsd { get; internal set; }
            public string Result { get; set; }

        }
    }
}
/////Osnovni podaci///
//b/h={5}/{6} cm;
//d1={7} cm; μSd={8}; 
//Beton:{9}
//Armatura:{10}
//Msd={11} kNm; Nsd={12} kN;
/////Izračunato///
//Msds={0} kNm
//εc = { 1 }‰ εs1={2}‰
//As1_pot={3} cm2
//As2_pot = { 4 } cm2
//", Round(Msds),
//KofZaProracunPravougaonogPreseka.εc,
//KofZaProracunPravougaonogPreseka.εs1, 
//Round(As1_pot),
//Round(As2_pot),
//b,h,d1,Round(μSd,3),
//beton.name,armatura.name,
//Round(Msd),Round(Nsd));
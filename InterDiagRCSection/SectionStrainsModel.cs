using System;

namespace InterDiagRCSection
{
    public class SectionStrainsModel : ICrossSectionStrains
    {
        public double F_c{get;set;}

        public double F_s1{get;set;} 

        public double F_s2{get;set;}

        public double z_c{get;set;}

        public double z_s1{get;set;}

        public double z_s2{get;set;} 

        public double c{get;set;} 
        public double M_Rd{get;set;}

        public double N_Rd{get;set;} 

        public double sig_c{get;set;}  

        public double sig_s1{get;set;}

        public double sig_s2{get;set;} 

        public double x{get;set;} 
        public double eps_c1{get;set;} 

        public double eps_c{get;set;}

        public double eps_s1{get;set;} 

        public double eps_s2{get;set;}  
    }
}

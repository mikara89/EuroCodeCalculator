using CalcModels;
using System;
using System.ComponentModel;

namespace CalcModels
{

    public class BetonModelPBAB : IBetonModel
    {
        public BetonModelPBAB(int fck) 
        {
            this.fck = fck;
            fcd = GetFcd(fck);
        }

        public BetonModelPBAB(string name)
        {
            if (String.IsNullOrEmpty(name)) 
                throw new ArgumentException("Invalid beton name");
            int fck;
            name = name.Replace("MB","");
            if (int.TryParse(name, out fck))
            {
                this.fck = fck;
                fcd = GetFcd(fck);
            }   
            else 
                throw new ArgumentException("Invalid beton name");
            
        }

        [DisplayName("Eb")]
        public double Ecm
        {
            get
            {
                return 9250 * Math.Pow((fck + 10), 1.0 / 3.0);
            }
        }
        [DisplayName("fb")]
        public double fcd { get; internal set; }
        [DisplayName("fbk")]
        public int fck { get; internal set; }
        [DisplayName("Eb")]
        public int fck_cube => fck;

        public int fcm
        {
            get
            {
                return fck + 10;
            }
        }

        public double GetSigma_f(double eps_b)
        {
            if (Math.Abs(this.εc2) == 0 || Math.Abs(this.εc2) <= Math.Abs(εc2))
                return 2 * fcd / Math.Abs(εcu2)*(Math.Abs(εcu2)-(Math.Pow(eps_b,2)/2/ Math.Abs(εcu2)));
            else return fcd;
        }

        public double fctm
        {
            get
            {
                return 0.25 * Math.Pow(fck, 2.0 / 3.0);
            }
        }

        public double n { get; set; } = 2.0;

        public string name => $"MB{fck}";

        public double ni { get; set; } = 1.0;
        public double αcc { get; set; } = 1.0;
        public double εc2 { get; set; } = -2.0;
        public double εc3 { get; set; } = -1.75;
        public double εcu2 { get; set; } = -3.5;
        public double εcu3 { get; set; } = -3.5;

        public double ρ => throw new NotImplementedException();
        public double fctk005
        {
            get
            {
                return 0.70 * fctm;
            }
        }
        public double fctk095
        {
            get
            {
                return 1.3 * fctm;
            }
        }

        public double ξ_lim => 0.45;

        private double GetFcd(int fck)
        {
            if (fck >= 10 && fck < 15) 
            {
                if (fck == 10) return 7.0;
                else return ((10.5 - 7) * (fck - 10) / (10 - 15)) + 7;
            }
            else if (fck >= 15 && fck < 20)
            {
                if (fck == 15) return 10.5;
                else return ((14 - 10.5) * (fck - 15) / (15 - 20)) + 10.5;
            }
            else if (fck >= 20 && fck <30)
            {
                if (fck == 20) return 14;
                else return ((20.5 - 14) * (fck - 20) / (20 - 30)) + 14;
            }
            else if (fck >= 30 && fck < 40)
            {
                if (fck == 30) return 20.5;
                else return ((25.5 - 20.5) * (fck - 30) / (30 - 40)) + 20.5;
            }
            else if (fck >= 40 && fck < 50)
            {
                if (fck == 40) return 25.5;
                else return ((30 - 25.5) * (fck - 40) / (40 - 50)) + 25.5;
            }
            else if (fck >= 50 && fck < 60)
            {
                if (fck == 50) return 30;
                else return ((33 - 30) * (fck - 50) / (50 - 60)) + 30;
            }
            else if (fck == 60)
            {
                if (fck == 60) return 33;
            }
            return 0;
        }
        public override string ToString()
        {
            //return $"{name}; fb: {Math.Round(fcd, 2)}MPa; fbk: {fck}Mpa; Eb: {Math.Round(Ecm / 1000, 2)}GPa";
            return $@"{name};
                    {"fb:",-4} {fcd,6:F2}{"MPa",-5}
                    {"fbk:",-4} {fck,6:F2}{"MPa",-5}
                    {"Eb:",-4} {Ecm / 1000,6:F2}{"GPa",-5}";
        
    }
    }
}

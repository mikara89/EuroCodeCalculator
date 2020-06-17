using System;
using System.Collections.Generic;
using System.Linq;

namespace CalcModels
{
    public class BetonModelEC : IBetonModel
    {
        
        private double y_c = 1.5;

        public double αcc { get; set; } = 0.85;
        public double αct { get; set; } = 1.0;
        public double ni { get; set; } = 0.85;

        public static IEnumerable<string> ListOfBetonClassesNames()
        {
            List<string> list = new List<string>();
            Enum.GetNames(typeof(BetonClassTypeEC))
                .ToList()
                .ForEach(x => list
                    .Add(x.Replace("_", "/")));
            return list;
        }
        public static IEnumerable<BetonModelEC> ListOfBetonClasses()
        {
            List<BetonModelEC> list = new List<BetonModelEC>();
            Enum.GetNames(typeof(BetonClassTypeEC))
                .ToList()
                .ForEach(x => list
                    .Add(new BetonModelEC(x.Replace("_", "/"))));
            return list;
        }
        private string GetStringFromType(BetonClassTypeEC betonClassType)
        {
            return Enum.GetName(typeof(BetonClassTypeEC), betonClassType).Replace("_", "/");
        }
        public BetonModelEC(BetonClassTypeEC betonClassType, double αcc = 0.85, double αct = 1.00, double y_c = 1.5)
        {
            this.name = GetStringFromType(betonClassType); 
            this.y_c = y_c;
            this.αcc = αcc;
            this.αct = αct;
        }
        public BetonModelEC(string betonClassName, double αcc = 0.85, double αct = 1.00, double y_c=1.5)
        {
            this.name = betonClassName;
            this.y_c = y_c;
            this.αcc = αcc;
            this.αct = αct;
        }
        public string name { get; internal set; }
        public int fck
        {
            get
            {
                int i;
                int.TryParse(name.Substring(1, 2), out i);
                if (i == 0) throw new System.ArgumentException("Invalid concrite name");
                return i;
            }
        }
        public int fck_cube
        {
            get
            {
                int i;
                int.TryParse(name.Substring(4, 2), out i);
                if (i == 0) throw new System.ArgumentException("Invalid concrite name");
                return i;
            }
        }
        public int fcm
        {
            get
            {
                return fck + 8;
            }
        }
        public double fctm
        {
            get
            {
                return 0.30 * Math.Pow(fck, 2.0 / 3.0);
            }
        }
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
        /// <summary>
        /// In MPa
        /// </summary>
        public double Ecm
        {
            get
            {
                //return 22*Math.Pow((fcm/10),1.0/3.0)*1000;
                return 9500 * Math.Pow((fck + 8), 1.0 / 3.0);
            }
        }
        //public double εc1 { get; set; }
        //public double εcu1 { get; set; }
        public double εc2 { get; set; } = -2.0;
        public double εcu2 { get; set; } = -3.5;
        public double n { get; set; } = 2;
        public double εc3 { get; set; } = -1.75;
        public double εcu3 { get; set; } = -3.5;
        public double ρ
        {
            get
            {
                switch (name)
                {
                    case "C12/16":
                    case "C16/20":
                    case "C20/25":
                        return 0.0007;
                    case "C25/30":
                    case "C30/37":
                    case "C35/45":
                        return 0.0011;
                    case "C40/50":
                    case "C45/55":
                    case "C55/60":
                        return 0.0013;
                    default:
                        break;
                }
                return 9500 * Math.Pow((fck + 8), 1.0 / 3.0);
            }
        }
        /// <summary>
        /// fcd=α*fck/1,5 => α=0.85;
        /// [MPa] /10 => [kN/cm2]
        /// </summary>
        public double fcd
        {
            get
            {
                return αcc * fck / y_c;
            }
        }

        public double ξ_lim { get {
                if (fck <= 50)
                    return 0.45;
                    
                else
                    return 0.35;
                    }
        }

        public override string ToString()
        {
            //return $"{name}; fcd: {Math.Round(fcd, 2)}MPa; fck: {fck}Mpa; Ecm: {Math.Round(Ecm / 1000, 2)}GPa";

            return $@"{name};
                    {"fck:",-4} {fck,5:F2}{"MPa",-5}
                    {"fcd:",-4} {fcd,5:F2}{"MPa",-5}
                    {"Ecm:",-4} {Ecm / 1000,5:F2}{"GPa",-5}";
        }

        public double GetSigma_f(double eps_b)
        {
            if (Math.Abs(this.εc2) == 0 || Math.Abs(this.εc2) <= Math.Abs(εc2))
                return fcd * Math.Abs(1 - (1 - Math.Pow(Math.Abs(eps_b) / Math.Abs(εc2), 2)));
            else return fcd;
        }
    }
}

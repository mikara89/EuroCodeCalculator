using System;
using System.Collections.Generic;
using System.Linq;

namespace TabeleEC2.Model
{
    public interface IBetonModel
    {

    }
   
    public enum BetonClassType
    {
         C12_16,
         C16_20,
         C20_25,
         C25_30,
        C30_37,
        C35_45,
        C40_50,
        C45_55,
        C55_60
    }
    public class BetonModelEC : IBetonModel  
    {
        public double αcc=0.85;

        public static IEnumerable<string> ListOfBetonClassesNames()
        {
            List<string> list = new List<string>();
            Enum.GetNames(typeof(BetonClassType))
                .ToList()
                .ForEach(x => list
                    .Add(x.Replace("_", "/")));
            return list;
        }
        public static IEnumerable<BetonModelEC> ListOfBetonClasses()
        {
            List<BetonModelEC> list = new List<BetonModelEC>();
            Enum.GetNames(typeof(BetonClassType))
                .ToList()
                .ForEach(x => list
                    .Add(new BetonModelEC(x.Replace("_", "/"))));
            return list;
        }
        private string GetStringFromType(BetonClassType betonClassType)
        {
            return Enum.GetName(typeof(BetonClassType), betonClassType).Replace("_", "/");
        }
        public BetonModelEC(BetonClassType betonClassType,double α=0.85)
        {
            this.name = GetStringFromType(betonClassType);
            this.αcc = α;
        }
        public BetonModelEC(string betonClassName, double α = 0.85) 
        {
            this.name = betonClassName;
            this.αcc = α;
        }
        public string name { get;internal set; }
        public int fck { get
            {
                int i;
                int.TryParse(name.Substring(1, 2),out i);
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
                return 0.70* fctm;
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
        //public double εc2 { get; set; }
        //public double εcu2 { get; set; }
        //public double n { get; set; }
        //public double εc3 { get; set; }
        //public double εcu3 { get; set; }
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
                return αcc * fck / 1.5;
            }
        }
        public override string ToString()
        {
            return $"{name}; fcd: {Math.Round(fcd, 2)}MPa; fck: {fck}Mpa; Ecm: {Math.Round(Ecm/1000,2)}GPa";
        }
    }
}

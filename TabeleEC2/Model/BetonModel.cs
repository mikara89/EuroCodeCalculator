using System;

namespace TabeleEC2.Model
{
    public interface IBetonModel
    {

    }
    public class BetonModelEC:IBetonModel
    {
        public string name { get; set; }
        public int fck { get; set; }
        public int fck_cube { get; set; }
        public int fcm { get; set; }
        public double fctm { get; set; }
        public double fctk005 { get; set; }
        public double fctk095 { get; set; }
        public int Ecm { get; set; }
        public double εc1 { get; set; }
        public double εcu1 { get; set; }
        public double εc2 { get; set; }
        public double εcu2 { get; set; }
        public double n { get; set; }
        public double εc3 { get; set; }
        public double εcu3 { get; set; }
        public double ρ { get; set; }
        public double α { get; set; } = 0.85;
        /// <summary>
        /// fcd=α*fck/1,5 => α=0.85;
        /// [MPa] /10 => [kN/cm2]
        /// </summary>
        public double fcd {
            get {
                return α * fck / 1.5;
            } }
        public override string ToString()
        {
            return$"{name}; fcd: {Math.Round(fcd,2)}MPa; fck: {fck}MPa; Ecm: {Ecm}GPa";
        }
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
    public class BetonModelEC_v2 : IBetonModel 
    {
        private readonly double α;

        private string GetStringFromType(BetonClassType betonClassType)
        {
            switch (betonClassType)
            {
                case BetonClassType.C12_16:
                    return "C12/16";
                case BetonClassType.C16_20:
                    return "C16/20";
                case BetonClassType.C20_25:
                    return "C20/25";
                case BetonClassType.C25_30:
                    return "C25/30";
                case BetonClassType.C30_37:
                    return "C30/37";
                case BetonClassType.C35_45:
                    return "C35/45";
                case BetonClassType.C40_50:
                    return "C40/50";
                case BetonClassType.C45_55:
                    return "C45/55";
                case BetonClassType.C55_60:
                    return "C55/60";
                default:
                    throw new ArgumentOutOfRangeException($"{nameof(betonClassType)} doesn't exist");
            }
        }
        public BetonModelEC_v2(double α = 0.85)
        {

        }
        public BetonModelEC_v2(BetonClassType betonClassType,double α=0.85)
        {
            this.name = GetStringFromType(betonClassType);
            this.α = α;
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
                return 0.30 * Math.Pow(fck, 2 / 3);
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
                return 9500*Math.Pow((fck+8),1/3);
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
                return 9500 * Math.Pow((fck + 8), 1 / 3);
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
                return α * fck / 1.5;
            }
        }
        public override string ToString()
        {
            return $"{name}; fcd: {Math.Round(fcd, 2)}MPa; fck: {fck}Mpa; Ecm: {Math.Round(Ecm,2)}GPa";
        }
    }

    public class BetonModelPBAB : IBetonModel
    {
        public string name {
            get           
            {
                return "MB"+fbk;
            }
        }
        public int fbk { get; set; } 
        /// <summary>
        /// fcd=α*fck/1,5 => α=0.85;
        /// [MPa] /10 => [kN/cm2]
        /// </summary>
        public double fb
        {
            get
            {
                return fbk / 1.4285714285714285714285714285714;
            }
        }
        
    }



}

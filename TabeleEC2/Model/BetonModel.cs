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
        /// <summary>
        /// fcd=α*fck/1,5 => α=0.85;
        /// [MPa] /10 => [kN/cm2]
        /// </summary>
        public double fcd {
            get {
                return 0.85 * fck / 1.5;
            } }
    }

    public class BetonModelEC_v2 : IBetonModel 
    {
        public string name { get; set; }
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
                return 0.85 * fck / 1.5;
            }
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

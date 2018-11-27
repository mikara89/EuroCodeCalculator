using System;

namespace CalculatorEC2Logic
{
    public interface IForcesSlenderness
    {
        double M_top { get; set; }
        double M_bottom { get; set; }
        double NEd { get; set; }
        double M0Ed { get; }
        double MEd(double e2 = 0);
        double M02 { get; }
        double M01 { get; }
    }
    public interface IForcesBendingAndCompressison 
    {
        double Msd { get; }
        double Nsd { get;}
        double Msds(double h, double d1);
        bool IsMsdNegativ { get; }
    }
    public class ForcesBendingAndCompressison : IForcesBendingAndCompressison
    {
        private double _Msd;
        private double _Msds;
        public ForcesBendingAndCompressison(double Msd, double Nsd)
        {
            this._Msd = Msd;
            this.Nsd = Nsd;
        }

        public double Msd { get => Math.Abs(_Msd); }
        public double Nsd { get; }
        public bool IsMsdNegativ { get => _Msd < 0?true:false; }
        public double Msds(double h, double d1)
        {
            return Msd + Nsd * (h / 100 / 2 - d1 / 100);
        }
    }
}

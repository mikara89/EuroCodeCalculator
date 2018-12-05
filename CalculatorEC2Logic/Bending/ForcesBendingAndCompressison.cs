using System;

namespace CalculatorEC2Logic
{
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

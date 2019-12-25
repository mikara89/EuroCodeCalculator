using System;

namespace InterDiagRCSection
{
    public class ConcreteForceCalc : IConcreteForceCalc
    {
        private double? Force= null;
        public int n { get; set; } = 100;
        public ISectionStrainsModel sectionStrains { get;internal set; }

        public ConcreteForceCalc(ISectionStrainsModel sectionStrains)
        {
            this.sectionStrains = sectionStrains;
        }
        public double GetForce()
        {
            if (sectionStrains.eps_c > 0)
                return 0;
            var h = sectionStrains.geometry.h;
            var x = h -sectionStrains.x;
           
            double dx = (double)(h - x) / n;

            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double xi = x + i * dx;
                double funVal = CaclForceValuePerDx(xi);
                double areaVal = funVal * dx;
                result += areaVal;
            }
            Force= -1 * result;
            return -1 * result;

        }
        public double GetDistance()
        {
            var h = sectionStrains.geometry.h;
            var x = h - sectionStrains.x;

            double Fc = Force!=null? (double)Force: GetForce();

            double dx = (double)(h - x) / n;

            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double xi = x + i * dx;
                double funVal = CaclForceValuePerDx(xi)*xi;
                double areaVal = funVal * dx;
                result += areaVal;
            }

            if (Fc == 0) 
                return 0;

            var a = result / Fc;
            return Math.Abs(result / Fc - sectionStrains.geometry.y2 + sectionStrains.x);

        }
        private double CaclForceValuePerDx(double xi)
        {
            var b = sectionStrains.Get_b(xi);
            var s = sectionStrains.Get_sig(xi);
            return b * s / 10;
        }
     
    }




}

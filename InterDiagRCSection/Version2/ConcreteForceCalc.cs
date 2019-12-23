namespace InterDiagRCSection
{
    public class ConcreteForceCalc : IConcreteForceCalc
    {
        public int n { get; set; } = 50000;
        public ISectionStrainsModel sectionStrains { get;internal set; }

        public ConcreteForceCalc(ISectionStrainsModel sectionStrains)
        {
            this.sectionStrains = sectionStrains;
        }
        public double GetForce()
        {
            var x = sectionStrains.x;
            var h = sectionStrains.geometry.h;
            double dx = (double)(h - x) / n;

            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double xi = x + i * dx;
                double funVal = CaclForceValuePerDx(xi);
                double areaVal = funVal * dx;
                result += areaVal;
            }

            return result;

        }
        public double GetDistance()
        {
            var x = sectionStrains.x;
            var h = sectionStrains.geometry.h;
            var Fc = GetForce();
            double dx = (double)(h - x) / n;

            double result = 0;
            for (int i = 0; i < n; i++)
            {
                double xi = x + i * dx;
                double funVal = CaclDistanceValuePerDx(xi);
                double areaVal = funVal * dx;
                result += areaVal;
            }

            return result / Fc - h - sectionStrains.geometry.y1;

        }
        private double CaclForceValuePerDx(double xi)
        {
            return sectionStrains.Get_b(xi) * sectionStrains.Get_sig(xi) / 10;
        }
        private double CaclDistanceValuePerDx(double xi)
        {
            return sectionStrains.Get_b(xi) * sectionStrains.Get_sig(xi) / 10 * xi;
        }
    }




}

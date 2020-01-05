using System;
using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public class ConcreteForceCalc : IConcreteForceCalc
    {
        private double? Force = null;
        private double? Distance = null;
        public int n { get; set; } = 1000;
        public ISectionStrainsFactory sectionStrains { get; internal set; }

        public ConcreteForceCalc(ISectionStrainsFactory sectionStrains)
        {
            this.sectionStrains = sectionStrains;
            Calc();
        }

        private void Calc()
        {
            if (sectionStrains.eps_c >= 0)
            {
                Force = 0;
                Distance = 0;
                return;
            }

            var h = sectionStrains.geometry.h;
            var x = h - sectionStrains.x;

            double dx = (double)(h - x) / n;

            double resultF = 0;
            double resultD = 0;
            for (int i = 0; i < n; i++)
            {
                double xi = x + i * dx;
                double funVal = CaclForceValuePerDx(xi);
                resultF += funVal * dx;
                resultD += funVal * (sectionStrains.x - xi) * dx;
            }
            Force = -1 * resultF;
            Distance =Math.Abs(resultD / (double)Force - sectionStrains.geometry.y1 + sectionStrains.x);
        }

        public double GetForce()
        {
            if (Force == null)
                Calc();
            return (double)Force;
        }
        public double GetDistance()
        {
            if(Distance==null)
                Calc();
            return (double)Distance;
        }
        private double CaclForceValuePerDx(double xi)
        {
            var b = sectionStrains.Get_b(xi);
            var s = sectionStrains.Get_sig(xi);
            return b * s / 10;
        }
    }
    //public class ConcreteForceCalc : IConcreteForceCalc
    //{
    //    private double? Force= null;
    //    public int n { get; set; } = 1000;
    //    public ISectionStrainsFactory sectionStrains { get;internal set; }

    //    public ConcreteForceCalc(ISectionStrainsFactory sectionStrains)
    //    {
    //        this.sectionStrains = sectionStrains;
    //    }
    //    public double GetForce()
    //    {
    //        if (sectionStrains.eps_c > 0)
    //            return 0;
    //        var h = sectionStrains.geometry.h;
    //        var x = h -sectionStrains.x;

    //        double dx = (double)(h - x) / n;

    //        double result = 0;
    //        for (int i = 0; i < n; i++)
    //        {
    //            double xi = x + i * dx;
    //            double funVal = CaclForceValuePerDx(xi);
    //            double areaVal = funVal * dx;
    //            result += areaVal;
    //        }
    //        Force= -1 * result;
    //        return -1 * result;

    //    }
    //    public async Task<double> GetForceAsync()
    //    {
    //        return await Task.Run(() =>
    //         {
    //             return GetForce();
    //         });
    //    }
    //    public async Task<double> GetDistanceAsync()
    //    {
    //        return await Task.Run(() =>
    //        {
    //            return GetDistance();
    //        });
    //    }

    //    public double GetDistance()
    //    {
    //        var h = sectionStrains.geometry.h;
    //        var x = h - sectionStrains.x;

    //        double Fc = Force!=null? (double)Force: GetForce();

    //        double dx = (double)(h - x) / n;

    //        double result = 0;
    //        for (int i = 0; i < n; i++)
    //        {
    //            double xi = x + i * dx;
    //            double funVal = CaclForceValuePerDx(xi)* (sectionStrains.x -xi);
    //            double areaVal = funVal * dx;
    //            result += areaVal;
    //        }

    //        if (Fc == 0) 
    //            return 0;

    //        var a = result / Fc- sectionStrains.geometry.y1 + sectionStrains.x;
    //        return result / Fc - sectionStrains.geometry.y1 + sectionStrains.x;

    //    }
    //    private double CaclForceValuePerDx(double xi)
    //    {
    //        var b = sectionStrains.Get_b(xi);
    //        var s = sectionStrains.Get_sig(xi);
    //        return b * s / 10;
    //    }

    //}
}

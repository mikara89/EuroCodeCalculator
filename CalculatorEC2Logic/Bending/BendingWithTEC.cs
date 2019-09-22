using CalcModels;
using System;
using TabeleEC2;
using TabeleEC2.Model;

namespace CalculatorEC2Logic
{
    public class BendingWithTEC : IDisposable
    {
        private readonly IForcesBendingAndCompressison forces;
        private readonly IElementGeometryT geometry;
        private readonly IMaterial material;
        private readonly ICoeffService coeffService;
        public BendingRectangularCrossSectionEC2 model;

        public BendingWithTEC(IForcesBendingAndCompressison forces, IElementGeometryT geometry, IMaterial material)
        {
            this.forces = forces ?? throw new ArgumentNullException(nameof(forces));
            this.geometry = geometry ?? throw new ArgumentNullException(nameof(geometry));
            this.material = material ?? throw new ArgumentNullException(nameof(material));
            coeffService = new CoeffService(material);

            Calc();
        }

        public void Dispose()
        {
            model.Dispose();
            GC.Collect();
        }

        public void Calc()
        {
            double Msd = forces.Msd;
            double Nu = forces.Nsd;
            double Mu;
            //double Du = 0;
            double b_eff = geometry.b_eff;
            double b_w = geometry.b; // sirina donje ivice grede
            double h_f = geometry.h_f; //debljina ploce
            double d = geometry.d; //visina preseka - d1
            double s;
            double δ = h_f / d;

            BetonModelEC beton = material.beton;

            ReinforcementTypeModelEC arm = material.armatura;

            var kof1 = coeffService.GetNew();
            var kof2 = coeffService.GetNew();

            bool done = false;
            s = δ;
            int i = 0;
            var x = 0.0;
            var s_add = 0.01;
            var μSd = (Msd * 100) / (b_eff * Math.Pow(d, 2) * beton.fcd / 10);
            var kof_test = coeffService.GetByμ(μSd);
            if (kof_test.ξ <= s)
            {
                var kof = kof_test;
                x = kof.ξ * d;
                Mu = Msd;
            }
            do
            {
                i++;
                kof1.SetByξ(s);
                x = s * d;
                var Du1 = kof1.αv * b_eff * s * d * beton.fcd / 10;
                var zb1 = d * (1 - kof1.ka * s);

                var Ebd = ((s - δ) / s) * kof1.εc;
                kof2.SetByEcEs1(Ebd, 20);



                var Du2 = kof2.αv * (b_eff - b_w) * (x - h_f) * beton.fcd / 10;
                var zb2 = d - h_f - kof2.ka * (x - h_f);

                Mu = Du1 * zb1 / 100 - Du2 * zb2 / 100;

                if (i > 200)
                    break;

                if (Math.Round(Mu, 3) < Math.Round(Msd, 3))
                {
                    s += s_add;
                    continue;
                }
                if (Math.Round(Mu, 3) > Math.Round(Msd, 3))
                {
                    s_add = s_add / 2;
                    s -= s_add;
                    continue;
                }

                if (Math.Round(Mu, 3) == Math.Round(Msd, 3))
                    done = true;

            } while (!done);

            if (i > 200 && done == false)
            {
                throw new Exception($"Can't calculate! {i}");
            }
            var bi = Mu * 100 / (kof1.μRd * Math.Pow(d, 2) * beton.fcd / 10);

            model = new BendingRectangularCrossSectionEC2
                (
                    new ForcesBendingAndCompressison(Mu, Nu),
                    new ElementGeometry
                    {
                        b = bi>geometry.b_eff? geometry.b_eff : bi,
                        //b =  bi,
                        h = geometry.h,
                        d1 = geometry.d1,
                        d2 = geometry.d2,
                        unit = UnitDimesionType.cm,
                    },
                    new Material { armatura = arm, beton = beton }
                );
            ;
        }
        public override string ToString()
        {
            return $@"//////Result///////
    Forces:
        Msd:        {forces.Msd:F2}kN
        Nsd:        {forces.Nsd:F2}kNm
        Msds:       {forces.Msds(geometry.h, geometry.d1):F2}kNm
    Material:
        Armatrua:   {material.armatura.ToString()}
        Beton:      {material.beton.ToString()}
    Geometry:
        b_eff:      {geometry.b_eff}{geometry.unit}
        h_f:        {geometry.h_f}{geometry.unit}
        b:          {geometry.b}{geometry.unit}
        h:          {geometry.h}{geometry.unit}
        d1:         {geometry.d1}{geometry.unit}
        d2:         {geometry.d2}{geometry.unit}
        d:          {geometry.d}{geometry.unit}
        bi:         {model.Geometry.b}{geometry.unit}
    Result:
        εc/εs1:     {model.KofZaProracunPravougaonogPreseka.εc:F3}‰/{model.KofZaProracunPravougaonogPreseka.εs1:F3}‰
        εs2:        {model.KofZaProracunPravougaonogPreseka.εs2(geometry.d, geometry.d2)}‰
        μRd:        {model.KofZaProracunPravougaonogPreseka.μRd:F3}
        x:          {model.X:F2} cm
        As1_pot:    {model.As1_pot:F2} cm2 
        As2_pot:    {model.As2_pot:F2} cm2
        μRd_lim:    {coeffService.GetByξ_lim().μRd:F3}";
        }


    }
}

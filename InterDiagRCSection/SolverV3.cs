using CalcModels;
using ConsoleTables;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public class SolverV3
    {
        public List<SectionStrainsModel> List { get; set; }
        public List<string> Worrnings { get; set; }

        public IMaterial Material;
        public IElementGeometryWithReinf Geometry;
        public SolverV3(IMaterial material, IElementGeometryWithReinf geometry) 
        {
            Material = material;
            Geometry = geometry;

        }

        public async Task CalcAsync(double precision = 0.01)
        {
            List = new List<SectionStrainsModel>();

            var j = 0;
            do
            {
                j++;
                bool Invert = j == 1 ? false : true;
                if (Invert)
                    Geometry.Invert();

                ISectionStrainsFactory sectionStrains = new SectionStrainsFactory(Material, Geometry);
                var RcSectionCalc = new RCSectionCalc(sectionStrains, new CalcForces(new ConcreteForceCalc(sectionStrains))); 
                ///Rotates about point A (scope 1 and 2)
                ///Bending with or without tension (scope 1 pure tension)
                for (double i = Material.armatura.eps_ud; i > Material.beton.εcu2; i -= precision)
                {
                    sectionStrains.SetByEcEs1(i, Material.armatura.eps_ud);
                    RcSectionCalc.Calc();
                    List.Add(SectionStrainsModelFactory.Create(RcSectionCalc));
                }


                sectionStrains.SetByEcEs1(Material.beton.εcu2, Material.armatura.eps_ud);
                RcSectionCalc.Calc();
                List.Add(SectionStrainsModelFactory.Create(RcSectionCalc));


                ///Rotates about point B
                ///Bending with or without compression or tensio (scope 3)
                ///excentric compression (scope 4)
                var lim = Material.beton.εcu2 * Geometry.d1 / Geometry.h;
                for (double i = Material.armatura.eps_ud; i > lim; i -= precision)
                {
                    sectionStrains.SetByEcEs1(Material.beton.εcu2, i);
                    RcSectionCalc.Calc();
                    List.Add(SectionStrainsModelFactory.Create(RcSectionCalc));

                    if (sectionStrains.eps_c1 == 0) break;
                }
                ///Rotates about point C
                ///Full compression
                for (double i = Material.beton.εcu2; i < Material.beton.εc2; i += precision / 10)
                {
                    sectionStrains.SetByEcEs1(i);
                    RcSectionCalc.Calc();
                    List.Add(SectionStrainsModelFactory.Create(RcSectionCalc));
                }

                sectionStrains.SetByEcEs1(Material.beton.εc2);
                RcSectionCalc.Calc();
                List.Add(SectionStrainsModelFactory.Create(RcSectionCalc));

                if (!Invert)
                {
                    List.Reverse();
                }

            } while (j < 2);
        }
        public override string ToString()
        {
            var table = new ConsoleTable("R.Br", "M_Rd", "N_Rd", "x", "Fc", "zc", "Fs1", "zs1", "Fs2", "zs2");

            List<object[]> rows = new List<object[]>();

            List.ForEach(x =>
            {
                //rows.Add(new { RBr=List.IndexOf(x), M_Rd=x.M_Rd.Round(2), N_Rd=x.N_Rd.Round(2), x=x.sectionStrains.x.Round(2), Fc=x.Forces["Fc"].F.Round(2), Fs1=x.Forces["Fs1"].F.Round(2), Fs2=x.Forces["Fs2"].F.Round(2) });
                table.Rows.Add(new object[] { List.IndexOf(x), x.M_Rd.Round(2), x.N_Rd.Round(2), x.x.Round(2), x.Fc.Round(2), x.Zc, x.Fs1.Round(2), x.Zs1.Round(2), x.Fs2.Round(2), x.Zs2.Round(2) });
            });

            var s = table
                 .Configure(o => o.NumberAlignment = Alignment.Right)
                 .ToMarkDownString();




            //var r = $"| R.Br    |   M_Rd    |    N_Rd   |    x  |    Fc |    Fs1    |    Fs2    |";
            //    r += $"{Environment.NewLine}---------------------------------------------------------------------------";
            //;
            //List.ForEach(x =>
            //{
            //    r += $"{Environment.NewLine}|   {List.IndexOf(x)}   |   {x.M_Rd.Round(2)}    |    {x.N_Rd.Round(2)}   |    {x.sectionStrains.x.Round(2)}   |    {x.Forces["Fc"].F.Round(2)}   |    {x.Forces["Fs1"].F.Round(2)}  |   {x.Forces["Fs2"].F.Round(2)}   |";
            //    r += $"{Environment.NewLine}---------------------------------------------------------------------------";
            //});
            return s;
        }
    }
}


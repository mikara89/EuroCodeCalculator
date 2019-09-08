using CalcModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InterDiagRCSection
{
    public class Solver
    {
        public List<CrossSectionStrains> List { get; set; }
        public IMaterial Material;
        public IElementGeometryWithReinf Geometry; 
        public Solver(IMaterial material, IElementGeometryWithReinf geometry)
        {
            Material = material;
            Geometry = geometry;
        }

        public async Task Calc()
        {
            await Task.Run(() =>
            {
                ///Rotates about point A (scope 2)
                ///Bending with or without tension
                for (double i = 0; i <= Material.beton.εcu2; i-=0.01)
                {
                    var a = new CrossSectionStrains(Material, Geometry);
                    a.SetByEcEs1(i, Material.armatura.eps_ud);
                    List.Add(a);
                }
                

                ///Rotates about point B
                ///Bending with or without compression or tensio (scope 3)
                ///excentric compression (scope 4)
                for (double i = Material.armatura.eps_ud; i <= -0.8; i -= 0.01)
                {
                    var b = new CrossSectionStrains(Material, Geometry);
                    b.SetByEcEs1(Material.beton.εcu2, i);
                    if (b.εc1 == 0) break;
                    List.Add(b);
                }
                ///Rotates about point C
                ///Full compression
                for (double i = Material.beton.εcu2; i <= Material.beton.εc2; i += 0.01)
                {
                    var b = new CrossSectionStrains(Material, Geometry);
                    b.SetByEcEs1(i);
                    List.Add(b);
                }
            });
        }
    }
}

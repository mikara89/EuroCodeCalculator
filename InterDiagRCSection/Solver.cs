using CalcModels;
using System.Collections.Generic;
using System.Linq;
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

        public async Task Calc(double precision=0.01)
        {
            int bb;
            List = new List<CrossSectionStrains>();
            await Task.Run(() =>
            {
                var j = 0;
                do
                {
                    j++;
                    bool Invert = j == 1 ? false : true;

                    ///Rotates about point A (scope 2)
                    ///Bending with or without tension
                    for (double i = 0; i > Material.beton.εcu2; i -= precision)
                    {
                        var a = new CrossSectionStrains(Material, Geometry, Invert);
                        a.SetByEcEs1(i, Material.armatura.eps_ud);
                        List.Add(a);
                    }
                    //if (Invert)
                    //{
                        var a1 = new CrossSectionStrains(Material, Geometry, Invert);
                        a1.SetByEcEs1(Material.beton.εcu2, Material.armatura.eps_ud);
                        List.Add(a1);
                    //}
                    

                    ///Rotates about point B
                    ///Bending with or without compression or tensio (scope 3)
                    ///excentric compression (scope 4)
                    for (double i = Material.armatura.eps_ud; i >= -2; i -= precision)
                    {
                        var b = new CrossSectionStrains(Material, Geometry, Invert);
                        b.SetByEcEs1(Material.beton.εcu2, i);
                        List.Add(b);
                        if (b.εc1 == 0) break;
                    }
                    ///Rotates about point C
                    ///Full compression
                    for (double i = Material.beton.εcu2; i < Material.beton.εc2; i += precision)
                    {
                        var c = new CrossSectionStrains(Material, Geometry, Invert);
                        c.SetByEcEs1(i);
                        List.Add(c);
                    }
                    //if (Invert)
                    //{
                        var c1 = new CrossSectionStrains(Material, Geometry, Invert);
                        c1.SetByEcEs1(Material.beton.εc2);
                        List.Add(c1);
                    //}
                    if (!Invert)
                        List.Reverse();
                    } while (j<2);
            });
        }
    }
}

﻿using CalcModels;
using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InterDiagRCSection
{

    public class Solver
    {
        public List<SectionStrainsModel> List { get; set; }
        public List<string> Worrnings { get; set; }

        public IMaterial Material;
        public IElementGeometryWithReinf Geometry;
        public Solver(IMaterial material, IElementGeometryWithReinf geometry)
        {
            Material = material;
            Geometry = geometry;

        }

        public async Task CalcAsync(double precision = 0.01) 
        {
            List = new List<SectionStrainsModel>();
            await Task.Run(() =>
            {
                var j = 0;
                do
                {
                    j++;
                    bool Invert = j == 1 ? false : true;
                    if (Invert)
                        Geometry.Invert();
                    ///Rotates about point A (scope 1 and 2)
                    ///Bending with or without tension (scope 1 pure tension)
                    for (double i = Material.armatura.eps_ud; i > Material.beton.εcu2; i -= precision)
                    {

                        ISectionStrainsFactory s_a = new SectionStrainsFactory(Material, Geometry);
                        s_a.SetByEcEs1(i, Material.armatura.eps_ud);
                        var a = new RCSectionCalc(s_a, new CalcForces(new ConcreteForceCalc(s_a)));
                        a.Calc();
                        List.Add(SectionStrainsModelFactory.Create(a));

                        //if (a.Forces["Fc"].F == 0) break;
                    }

                    ISectionStrainsFactory s_a1 = new SectionStrainsFactory(Material, Geometry);
                    s_a1.SetByEcEs1(Material.beton.εcu2, Material.armatura.eps_ud);
                    var a1 = new RCSectionCalc(s_a1, new CalcForces(new ConcreteForceCalc(s_a1)));
                    a1.Calc();
                    List.Add(SectionStrainsModelFactory.Create(a1));


                    ///Rotates about point B
                    ///Bending with or without compression or tensio (scope 3)
                    ///excentric compression (scope 4)
                    var lim = Material.beton.εcu2 * Geometry.d1 / Geometry.h;
                    for (double i = Material.armatura.eps_ud; i > lim; i -= precision)
                    {
                        ISectionStrainsFactory s_b = new SectionStrainsFactory(Material, Geometry);
                        s_b.SetByEcEs1(Material.beton.εcu2, i);
                        var b = new RCSectionCalc(s_b, new CalcForces(new ConcreteForceCalc(s_b)));
                        b.Calc();
                        List.Add(SectionStrainsModelFactory.Create(b));

                        if (s_b.eps_c1 == 0) break;
                    }
                    ///Rotates about point C
                    ///Full compression
                    for (double i = Material.beton.εcu2; i < Material.beton.εc2; i += precision / 10)
                    {
                        ISectionStrainsFactory s_c = new SectionStrainsFactory(Material, Geometry);
                        s_c.SetByEcEs1(i);
                        var c = new RCSectionCalc(s_c, new CalcForces(new ConcreteForceCalc(s_c)));
                        c.Calc();
                        List.Add(SectionStrainsModelFactory.Create(c));

                    }

                    ISectionStrainsFactory s_c1 = new SectionStrainsFactory(Material, Geometry);
                    s_c1.SetByEcEs1(Material.beton.εc2);
                    var c1 = new RCSectionCalc(s_c1, new CalcForces(new ConcreteForceCalc(s_c1)));
                    c1.Calc();
                    List.Add(SectionStrainsModelFactory.Create(c1));

                    if (!Invert)
                    {
                        List.Reverse();
                    }

                } while (j < 2);
            });

             
        }
        public override string ToString()
        {
            var table = new ConsoleTable("R.Br", "M_Rd", "N_Rd","x","Fc","zc","Fs1","zs1","Fs2", "zs2");

            List<object[]> rows =new List<object[]>();

            List.ForEach(x =>
            {
                table.Rows.Add(new object[] { List.IndexOf(x), x.M_Rd.Round(2), x.N_Rd.Round(2),  x.x.Round(2), x.F_c.Round(2), x.z_c.Round(2),  x.F_s1.Round(2), x.z_s1.Round(2), x.F_s2.Round(2), x.z_s2.Round(2) });
            });

           var s= table
                .Configure(o => o.NumberAlignment = Alignment.Right)
                .ToMarkDownString();
            return s;
        }
    }
    //public class Solver
    //    {
    //        public List<CrossSectionStrains> List { get; set; }
    //        public List<string> Worrnings { get; set; }

    //        public IMaterial Material;
    //        public IElementGeometryWithReinfold Geometry;
    //        public Solver(IMaterial material, IElementGeometryWithReinfold geometry)
    //        {
    //            Material = material;
    //            Geometry = geometry;
    //        }

    //        public async Task Calc(double precision = 0.01)
    //        {
    //            List = new List<CrossSectionStrains>();
    //            await Task.Run(() =>
    //            {
    //                var j = 0;
    //                do
    //                {
    //                    j++;
    //                    bool Invert = j == 1 ? false : true;

    //                    ///Rotates about point A (scope 1 and 2)
    //                    ///Bending with or without tension (scope 1 pure tension)
    //                    for (double i = Material.armatura.eps_ud; i > Material.beton.εcu2; i -= precision)
    //                    {
    //                        var a = new CrossSectionStrains(Material, Geometry, Invert);
    //                        a.SetByEcEs1(i, Material.armatura.eps_ud);
    //                        List.Add(a);
    //                    }

    //                    var a1 = new CrossSectionStrains(Material, Geometry, Invert);
    //                    a1.SetByEcEs1(Material.beton.εcu2, Material.armatura.eps_ud);
    //                    List.Add(a1);


    //                    ///Rotates about point B
    //                    ///Bending with or without compression or tensio (scope 3)
    //                    ///excentric compression (scope 4)
    //                    var lim = Material.beton.εcu2 * Geometry.d1 / Geometry.h;
    //                    for (double i = Material.armatura.eps_ud; i > lim; i -= precision)
    //                    {
    //                        var b = new CrossSectionStrains(Material, Geometry, Invert);
    //                        b.SetByEcEs1(Material.beton.εcu2, i);
    //                        List.Add(b);
    //                        if (b.εc1 == 0) break;
    //                    }
    //                    ///Rotates about point C
    //                    ///Full compression
    //                    for (double i = Material.beton.εcu2; i < Material.beton.εc2; i += precision)
    //                    {
    //                        var c = new CrossSectionStrains(Material, Geometry, Invert);
    //                        c.SetByEcEs1(i);
    //                        List.Add(c);
    //                    }

    //                    var c1 = new CrossSectionStrains(Material, Geometry, Invert);
    //                    c1.SetByEcEs1(Material.beton.εc2);
    //                    List.Add(c1);

    //                    if (!Invert)
    //                        List.Reverse();
    //                } while (j < 2);
    //            });
    //        }

    //        public void GetWorrnings(double M, double N)
    //        {

    //            var ShotList_65 = new List<CrossSectionStrains>();
    //            var ShotList_55 = new List<CrossSectionStrains>();
    //            var ShotList_40 = new List<CrossSectionStrains>();
    //            var ShotList_35 = new List<CrossSectionStrains>();
    //            List.ForEach(x =>
    //            {
    //                if (x.N_Rd >= Geometry.b * Geometry.h * Material.beton.fcd * 0.65)
    //                    ShotList_65.Add(x);
    //                if (x.N_Rd >= Geometry.b * Geometry.h * Material.beton.fcd * 0.55)
    //                    ShotList_55.Add(x);
    //                if (x.N_Rd >= Geometry.b * Geometry.h * Material.beton.fcd * 0.40)
    //                    ShotList_40.Add(x);
    //                if (x.N_Rd >= Geometry.b * Geometry.h * Material.beton.fcd * 0.35)
    //                    ShotList_35.Add(x);
    //            });

    //            if (Geometry.As_1 + Geometry.As_2 > Geometry.b * Geometry.h * 0.04)
    //                Worrnings.Add("Sum of reinforcement greater than 4%");
    //            if (ShotList_65.IsMNValid(M, N))
    //                Worrnings.Add("the ductility condition is not satisfied for v_rd = 0.65");
    //            if (ShotList_55.IsMNValid(M, N))
    //                Worrnings.Add("the ductility condition is not satisfied for v_rd = 0.55");
    //            if (ShotList_40.IsMNValid(M, N))
    //                Worrnings.Add("the ductility condition is not satisfied for v_rd = 0.40");
    //            if (ShotList_35.IsMNValid(M, N))
    //                Worrnings.Add("the ductility condition is not satisfied for v_rd = 0.35");
    //        }
    //    }
}


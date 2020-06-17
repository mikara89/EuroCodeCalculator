using CalcModels;
using CalculatorEC2Logic;
using CalculatorEC2Logic.Transversal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Persistence
{
    public class TransverzalSilaEc2Repository : ITransverzalSilaEc2Repository
    {
        public TransverzalneSileEc2ResultModel CalculateInit(TransverzalneSileEc2Model trans)
        {
            TransverzalneSileEc2ResultModel Result;
            var beton = new BetonModelEC(trans.betonClass, 1);
            var arm = ReinforcementType.GetArmatura().Where(n => n.name == trans.armtype).SingleOrDefault();
            if (trans.settings != null)
            {
                beton = new BetonModelEC(trans.betonClass, trans.settings.alfa_cc, trans.settings.alfa_ct, trans.settings.y_c);
                arm = ReinforcementType.GetArmatura(trans.settings.y_s).Where(n => n.name == trans.armtype).SingleOrDefault();
            }
                

            beton.ni = 0.85;
            
            var armLong = new ReinforcementModelEC(trans.armLongitudinal.diametar, trans.armLongitudinal.kom);
            bool armCalc = false;
            if (trans.u_diametar != 0 && trans.m != 0 && trans.s != 0) armCalc = true;


            var g = new ElementGeometryTransversal()
            {
                b = trans.b,
                h = trans.h,
                d1 = trans.d1,
                d2 = trans.d1,
                As1 = new ReinforcementModelEC(trans.armLongitudinal.diametar, trans.armLongitudinal.kom),
                unit = UnitDimesionType.cm

            };
            var f = new ForcesTransversal()
            {
                Ved = trans.Ved,
                Vg=trans.Vg,
                Vq=trans.Vq
            };
            var m = new Material()
            {
                beton = beton,
                armatura = arm,
            };

            using (var t = new TransversalCalcEC2(g,f,m))
                {
                    if (armCalc)
                    {
                        if (trans.alfa == null)
                            t.CalculateArmature(trans.m, trans.s, new ReinforcementModelEC(trans.u_diametar, 1));
                        else
                        t.CalculateArmature(trans.m, trans.s, new ReinforcementModelEC(trans.u_diametar, 1), trans.teta,(double)trans.alfa);
                    }
                    Result = new TransverzalneSileEc2ResultModel()
                    {
                        Result = t.ToString(),
                        s = t.Asw_min == 0 ? trans.s : t.s,
                        ListS = t.List_s,
                        ListM = t.List_m,
                        m = t.Asw_min == 0 ? trans.m : t.m,
                        teta=t.Θ,
                        alfa=t.alfa,
                        u_diametar = trans.u_diametar,
                        addArm_pot = t.As_add,
                        TransArm_pot = t.Asw,
                        minArm_pot = t.Asw_min,
                        IskorArm= t.IskoriscenostArmature/100,
                        IskorBeton =t.IskoriscenostBetona/100,
                        Errors = t.Errors
                    };
                }
            return Result;
        }
    }
}

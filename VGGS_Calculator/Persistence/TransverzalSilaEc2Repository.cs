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
        public TransverzalneSileEc2ResultModel CalculateArm(TransverzalneSileEc2Model trans)
        {
            throw new NotImplementedException();
        }

        public TransverzalneSileEc2ResultModel CalculateInit(TransverzalneSileEc2Model trans)
        {
            TransverzalneSileEc2ResultModel Result;

            var beton = TabeleEC2.BetonClasses.GetBetonClassListEC().Where(n => n.name == trans.betonClass).SingleOrDefault();
            var arm = TabeleEC2.ReinforcementType.GetArmatura().Where(n => n.name == trans.armtype).SingleOrDefault();
            var armLong = new TabeleEC2.Model.ReinforcementModelEC(trans.armLongitudinal.diametar, trans.armLongitudinal.kom);
            bool armCalc = false;
            if (trans.u_diametar != 0 && trans.m != 0 && trans.s != 0) armCalc = true;
            if (trans.Ved == 0)
            {
                using (var t = new CalculatorEC2Logic.TransverzalneSileEC2(
              trans.b,
              trans.h,
              beton,
              arm,
              armLong,
              trans.Vg,
              trans.Vq,
              trans.d1, 0, 0
              ))
                {
                    if (armCalc)
                        t.CalculateArmature(trans.m, trans.s, new TabeleEC2.Model.ReinforcementModelEC(trans.u_diametar, 1));
                    Result = new TransverzalneSileEc2ResultModel()
                    {
                        Result = t.ToString(),
                        s = t.Asw_min == 0 ? trans.s : t.s,
                        ListS = t.List_s,
                        ListM = t.List_m,
                        m = t.Asw_min == 0 ? trans.m : t.m,
                        u_diametar = trans.u_diametar,
                        addArm_pot = t.As_add,
                        TransArm_pot = t.Asw,
                        minArm_pot = t.Asw_min,
                        IskorArm = t.IskoriscenostArmature,
                        IskorBeton = t.IskoriscenostBetona,
                        Errors = t.Errors
                    };
                }
            }
            else
            {
                using (var t = new CalculatorEC2Logic.TransverzalneSileEC2(
             b: trans.b,
             h: trans.h,
             beton:beton,
             armatura: arm,
             As1_model: armLong,
             Ved: trans.Ved,
             d1: trans.d1,Ng: 0,Nq: 0
             ))
                {
                    if (armCalc)
                        t.CalculateArmature(trans.m, trans.s, new TabeleEC2.Model.ReinforcementModelEC(trans.u_diametar, 1));
                    Result = new TransverzalneSileEc2ResultModel()
                    {
                        Result = t.ToString(),
                        s = t.Asw_min == 0 ? trans.s : t.s,
                        ListS = t.List_s,
                        ListM = t.List_m,
                        m = t.Asw_min == 0 ? trans.m : t.m,
                        u_diametar = trans.u_diametar,
                        addArm_pot = t.As_add,
                        TransArm_pot = t.Asw,
                        minArm_pot = t.Asw_min,
                        IskorArm = t.IskoriscenostArmature,
                        IskorBeton = t.IskoriscenostBetona,
                        Errors = t.Errors
                    };
                }
            }
          
            return Result;
        }
    }
}

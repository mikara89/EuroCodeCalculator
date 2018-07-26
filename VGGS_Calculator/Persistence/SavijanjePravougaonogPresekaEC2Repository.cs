using System.Linq;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Persistence
{
    public class SavijanjePravougaonogPresekaEC2Repository : ISavijanjePravougaonogPresekaEC2Repository 
    {
        public SavijanjePravougaonogPresekaEC2Model Calculate(SavijanjePravougaonogPresekaEC2Model input) 
        {
            SavijanjePravougaonogPresekaEC2Model result= input;
            if (input.Msd == 0)
            {
                using (CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2 sav = new CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2(
                    
                    b:input.b,
                    h:input.h,
                    d1:input.d1,
                    d2: input.d2,
                    beton: TabeleEC2.BetonClasses.GetBetonClassListEC().Single(b => b.name == input.betonClass),
                    armatura: TabeleEC2.ReinforcementType.GetArmatura().Single(a => a.name == input.armtype),
                    Mg: input.Mg,
                    Mq: input.Mq,
                    Ng: input.Ng,
                    Nq: input.Nq,
                    kof: input.h == 0 ? TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetItem_Full(input.mu) : null))
                {
                    ///Doo some thing
                    result.result = new SavijanjePravougaonogPresekaEC2Model.ResultModel()
                    {
                        kof = sav.KofZaProracunPravougaonogPreseka,
                        As1_pot = sav.As1_pot,
                        As2_pot = sav.As2_pot,
                        Msd = sav.Msd,
                        Msds = sav.Msds,
                        Nsd = sav.Nsd,
                        μSd = sav.μSd,
                        Result = sav.ToString(),
                    };
                    result.h = input.h == 0 ? sav.h : result.h;
                }
            }else
            {
                using (CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2 sav = new CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2(

                    b: input.b,
                    h: input.h,
                    d1: input.d1,
                    d2: input.d2,
                    beton: TabeleEC2.BetonClasses.GetBetonClassListEC().Single(b => b.name == input.betonClass),
                    armatura: TabeleEC2.ReinforcementType.GetArmatura().Single(a => a.name == input.armtype),
                    Msd: input.Msd,
                    Nsd: input.Nsd,
                    kof: input.h == 0 ? TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetItem_Full(input.mu) : null))
                {
                    ///Doo some thing
                    result.result = new SavijanjePravougaonogPresekaEC2Model.ResultModel()
                    {
                        kof = sav.KofZaProracunPravougaonogPreseka,
                        As1_pot = sav.As1_pot,
                        As2_pot = sav.As2_pot,
                        Msd = sav.Msd,
                        Msds = sav.Msds,
                        Nsd = sav.Nsd,
                        μSd = sav.μSd,
                        Result = sav.ToString(),
                    };
                    result.h = input.h == 0 ? sav.h : result.h;
                }
            }
            return result;
        }
    }
}

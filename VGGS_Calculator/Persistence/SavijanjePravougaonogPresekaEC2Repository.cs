using System.Linq;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Persistence
{
    public class SavijanjePravougaonogPresekaEC2Repository : ISavijanjePravougaonogPresekaEC2Repository
    {
        public SavijanjePravougaonogPresekaEC2Model Calculate(SavijanjePravougaonogPresekaEC2Model input)
        {
            SavijanjePravougaonogPresekaEC2Model result = input;

            using (CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2_V2 sav = new CalculatorEC2Logic.SavijanjePravougaonogPresekaEC2_V2(

                material: new CalculatorEC2Logic.Material()
                {
                    beton = TabeleEC2.BetonClasses.GetBetonClassListEC().Single(b => b.name == input.betonClass),
                    armatura = TabeleEC2.ReinforcementType.GetArmatura().Single(a => a.name == input.armtype)
                },

                geometry: new CalculatorEC2Logic.ElementGeomety()
                {
                    b = input.b,
                    h = input.h,
                    d1 = input.d1,
                    d2 = input.d2
                },
                Forces: input.Msd == 0 ?
                    new CalculatorEC2Logic.ForcesBendingAndCompressison(1.35 * input.Mg + 1.5 * input.Mq, 1.35 * input.Ng + 1.5 * input.Nq) :
                    new CalculatorEC2Logic.ForcesBendingAndCompressison(input.Msd, input.Nsd),
                kof: input.h == 0 ? TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(input.mu) : null))
            {
                ///Doo some thing
                result.result = new SavijanjePravougaonogPresekaEC2Model.ResultModel()
                {
                    kof = sav.KofZaProracunPravougaonogPreseka,
                    As1_pot = sav.As1_pot,
                    As2_pot = sav.As2_pot,
                    Msd = sav.Forces.Msd,
                    Msds = sav.Forces.Msds(sav.geometry.h, sav.geometry.d1),
                    Nsd = sav.Forces.Nsd,
                    μSd = sav.μSd,
                    Result = sav.ToString(),
                };
                result.h = input.h == 0 ? sav.geometry.h : result.h;
            }
            return result;
        } 
    }
}

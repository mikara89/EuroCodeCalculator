using System.Linq;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;
using CalcModels;

namespace VGGS_Calculator.Persistence
{
    public class SavijanjePravougaonogPresekaEC2Repository : ISavijanjePravougaonogPresekaEC2Repository
    {
        public SavijanjePravougaonogPresekaEC2Model Calculate(SavijanjePravougaonogPresekaEC2Model input)
        {
            IBetonModel beton;
            IReinforcementTypeModel armatura ; 
            SavijanjePravougaonogPresekaEC2Model result = input;

            if (input.settings != null)
            {
                beton = new BetonModelEC(input.betonClass, input.settings.alfa_cc, input.settings.alfa_ct, input.settings.y_c);
                armatura = ReinforcementType.GetArmatura(input.settings.y_s).Single(a => a.name == input.armtype);
            }

            else
            {
                beton = new BetonModelEC(input.betonClass);
                armatura = ReinforcementType.GetArmatura().Single(a => a.name == input.armtype);
            }
                

            if (beton == null)
                throw new System.ArgumentNullException(nameof(beton), "cant be null");

            if (armatura == null)
                throw new System.ArgumentNullException(nameof(armatura), "cant be null");

            var geometry= new ElementGeometry()
            {
                b = input.b,
                h = input.h,
                d1 = input.d1,
                d2 = input.d2
            };
            var cs = new CoeffService(new Material { armatura = armatura, beton = beton }, geometry);
            using (CalculatorEC2Logic.BendingRectangularCrossSectionEC2 sav = new CalculatorEC2Logic.BendingRectangularCrossSectionEC2(
                
                material: new Material()
                {
                    beton = beton,
                    armatura = armatura
                },

                geometry: geometry,
                forces: input.Msd == 0 ?
                    new CalculatorEC2Logic.ForcesBendingAndCompressison(1.35 * input.Mg + 1.5 * input.Mq, 1.35 * input.Ng + 1.5 * input.Nq) :
                    new CalculatorEC2Logic.ForcesBendingAndCompressison(input.Msd, input.Nsd),
                kof: input.h == 0 ? cs.GetByμ(input.mu) : null))
            {
                ///Doo some thing
                result.result = new SavijanjePravougaonogPresekaEC2Model.ResultModel()
                {
                    coef = sav.KofZaProracunPravougaonogPreseka,
                    As1_req = sav.As1_pot,
                    As2_req = sav.As2_pot,
                    Msd = sav.Forces.Msd,
                    Msds = sav.Forces.Msds(sav.Geometry.h, sav.Geometry.d1),
                    Nsd = sav.Forces.Nsd,
                    μSd = sav.KofZaProracunPravougaonogPreseka.μRd,
                    Result = sav.ToString(),
                };
                result.h = input.h == 0 ? sav.Geometry.h : result.h;
            }
            return result;
        } 
    }
}

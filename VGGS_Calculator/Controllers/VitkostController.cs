using CalcModels;
using CalculatorEC2Logic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class VitkostController : Controller
    {
        [HttpPost("/api/Vitkost")]
        public ActionResult PostIzracunaj([FromBody] VitkostModel model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(x => x.Errors);
                return BadRequest(
                    new
                    {
                        message = "Invalid model",
                        error = allErrors.Select(x => x.ErrorMessage),
                        modelExp = new VitkostModel()
                        {
                            Slenderness = "Ukljesten sa jedne",
                            k = 2,
                            N = 1620,
                            M_top = -38.5,
                            M_bottom = 38.5,
                            L = 375,
                            b = 30,
                            h = 30,
                            d1 = 4,
                            armtype = "B500B",
                            betonClass = "C25/30",
                            result = null
                        },
                    });
            }
            try
            {
                var geo = new ElementGeometySlenderness()
                {
                    b = model.b,
                    h = model.h,
                    d1 = model.d1,
                    d2 = model.d1,
                    L = model.L,
                    k = model.k,
                    unit = UnitDimesionType.cm
                };
                var forces = new ForcesSlenderness(geo.li, geo.h)
                {
                    NEd = model.N,
                    M_bottom = model.M_bottom,
                    M_top = model.M_top
                };
                var beton = new BetonModelEC(model.betonClass);
                beton.ni=0.85;
                var material = new Material()
                {
                    beton = beton,
                    armatura = ReinforcementType
                        .GetArmatura()
                        .Single(a => a.name == model.armtype)
                };
                var v = new SlendernessEC2(geo, forces, material);

                v.Calculate();
                v.KontrolaCentPritPreseka();
                v.ProracunArmature();

                model.result = v.ToString();

                return Ok(model);
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            
        }
    }
}
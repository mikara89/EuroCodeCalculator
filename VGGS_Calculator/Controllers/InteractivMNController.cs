using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TabeleEC2;
using TabeleEC2.Model;
using InterDiagRCSection;
using CalcModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace VGGS_Calculator.Controllers
{
    public class InteractivMNController : Controller 
    {
        public InteractivMNController(ILogger<InteractivMNController> logger)
        {
            Logger = logger;
        }

        public ILogger<InteractivMNController> Logger { get; }

        [HttpPost("/api/InterMN")]
        public async Task<IActionResult> GetListOfAllLines([FromBody] MN model)
        {
            if (model == null)
            {
                Logger.LogError("model not valid");
                throw new System.ArgumentNullException(nameof(model)); 
            }
            try
            {
                var material = new Material()
                {
                    beton = new BetonModelEC(model.material.betonClass),
                    armatura = ReinforcementType.GetArmatura().First(n => n.name == model.material.armtype),
                };
                var geometry = new ElementGeometryWithReinf()
                {
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    d2 = model.geometry.d2,
                    h = model.geometry.h,
                    As_1 = model.geometry.as1,
                    As_2 = model.geometry.as2,
                };
                var s = new Solver(material, geometry);
                await s.Calc();
                Logger.LogInformation("API send interacion curves");
                return Ok(s.List.Select(x=>new { x=x.M_Rd, y=x.N_Rd }));   
            }
            catch (System.Exception ex)
            {
                Logger.LogError("something went wrrong: "+ ex.Message);
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpPost("/api/InterMN/search")]
        public async Task<IActionResult> SearchForLineAsync([FromBody] MN model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                var material = new Material()
                {
                    beton = new BetonModelEC(model.material.betonClass),
                    armatura = ReinforcementType
                    .GetArmatura()
                    .First(n => n.name == model.material.armtype),
                };

                var geometry = new ElementGeometryWithReinf()
                {
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    d2 = model.geometry.d2,
                    h = model.geometry.h,
                    As_1 = model.geometry.as1,
                    As_2 = model.geometry.as2,
                };

                var s = new Solver(material, geometry);
                await s.Calc();
                s.GetWorrnings(model.m, model.n);
                var r = s.List.IsMNValid(model.m, model.n);

                var listMaxMin = new List<CrossSectionStrains>
                {
                     s.List.Select(n => new { n, Mrd = Math.Abs(n.M_Rd - model.m) })
                              .OrderBy(p => p.Mrd)
                              .First().n,
                    s.List.Select(n => new { n, Mrd = Math.Abs(n.M_Rd - model.m) })
                              .OrderByDescending(p => p.Mrd)
                              .First().n,
                    s.List.Select(n => new { n, Nrd = Math.Abs(n.N_Rd - model.n) })
                              .OrderBy(p => p.Nrd)
                              .First().n,
                    s.List.Select(n => new { n, Nrd = Math.Abs(n.N_Rd - model.n) })
                              .OrderByDescending(p => p.Nrd)
                              .First().n,
            };

                return Ok(
                    new
                    {
                        extrims = listMaxMin,
                        isValid = s.List.IsMNValid(model.m, model.n),
                        worrnings = s.Worrnings,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class MN
        {
            public double m { get; set; }
            public double n { get; set; }
            public Geometry geometry { get; set; }
            public Material material { get; set; }


            public class Geometry
            {
                public double b { get; set; }
                public double d1 { get; set; }
                public double d2{ get; set; }
                public double h { get; set; }
                public double as1 { get; set; }
                public double as2 { get; set; } 
            }
            public class Material
            {
                public string betonClass { get; set; }
                public string armtype { get; set; }
            }
        }
    }
}
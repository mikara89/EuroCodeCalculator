using System.Linq;
using Microsoft.AspNetCore.Mvc;
using TabeleEC2;
using TabeleEC2.Model;
using InterDiagRCSection;
using CalcModels;
using System.Threading.Tasks;

namespace VGGS_Calculator.Controllers
{
    public class InteractivMNController : Controller 
    {
        [HttpPost("/api/InterMN")]
        public async Task<IActionResult> GetListOfAllLines([FromBody] MN model)
        {
            if (model == null)
            {
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
                return Ok(s.List.Select(x=>new { x=x.M_Rd, y=x.N_Rd }));   
            }
            catch (System.Exception ex)
            {

                return BadRequest(new { error = ex.Message });
            }

        }

        //[HttpPost("/api/InterMN/search")]
        //public IActionResult SearchForLine([FromBody] MN model)
        //{
        //    if (model == null)
        //    {
        //        throw new System.ArgumentNullException(nameof(model));
        //    }
        //    try
        //    {
        //        var material = new Material()
        //        {
        //            beton = new BetonModelEC(model.material.betonClass),
        //            armatura = ReinforcementType.GetArmatura().First(n => n.name == model.material.armtype),
        //        };
        //        var geometry = new ElementGeometry()
        //        {
        //            b = model.geometry.b,
        //            d1 = model.geometry.d1,
        //            h = model.geometry.h,
        //        };
        //        var w = new SymmetricalReinfByClassicMethod(material, geometry);
        //        w.Get_ω(model.mi, model.ni);
        //        return Ok(new { w = w.searchingOf_ω.ω, List = w.searchingOf_ω.ListOfDotsInLineOfDiagram, textResulte = w.TextResult() });
        //    }
        //    catch (System.Exception ex)
        //    {
        //        return BadRequest(new { error = ex.Message });
        //    }
        //}
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
using System.Linq;
using CalcModels;
using CalculatorEC2Logic;
using Microsoft.AspNetCore.Mvc;
using TabeleEC2;
using TabeleEC2.Model;

namespace VGGS_Calculator.Controllers
{
    public class SymmetricalReinfByClassicMethodController : Controller 
    {
        [HttpPost("/api/Symclassic")]
        public IActionResult GetListOfAllLines([FromBody] MiNi model)
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
                var geometry = new ElementGeometry()
                {
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    h = model.geometry.h,
                };
                return Ok(new SymmetricalReinfByClassicMethod(material, geometry).GetAllLines());
            }
            catch (System.Exception ex)
            {

               return BadRequest(new { error = ex.Message });
            }

        }

        [HttpPost("/api/Symclassic/search")]
        public IActionResult SearchForLine([FromBody] MiNi model) 
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
                var geometry = new ElementGeometry()
                {
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    h = model.geometry.h,
                };
                var w = new SymmetricalReinfByClassicMethod(material, geometry);
                w.Get_ω(model.mi, model.ni);
                return Ok( new { w =w.searchingOf_ω.ω, List= w.searchingOf_ω.ListOfDotsInLineOfDiagram, textResulte=w.TextResult() });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class MiNi
        {
            public double mi { get; set; }
            public double ni { get; set; }
            public Geometry geometry { get; set; }
            public Material material { get; set; } 


            public class Geometry
            {
                public double b { get; set; }
                public double d1 { get; set; }
                public double h { get; set; }
            }
            public class Material 
            {
                public string betonClass { get; set; } 
                public string armtype { get; set; }
            }
        }
    }
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalcModels;
using Microsoft.AspNetCore.Mvc;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class KofZaProracunPravougaonogPresekaController : Controller
    {
        [HttpGet("/api/KofZaProracunPravougaonogPreseka")]
        public async Task<List<CoeffForCalcRectCrossSectionModelEC>> GetKofZaProracunPravougaonogPresekaAsync([FromBody] MaterialModel model) 
        {
            var kofList = new List<CoeffForCalcRectCrossSectionModelEC>();
            var material = new Material()
            {
                beton = new BetonModelEC(model.betonClass),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == model.armtype),
            };
            var geometry = new ElementGeometry()
            {
                b = model.b,
                d1 = model.d1,
                d2 = model.d2,
                h = model.h,
            };
            var cService = new CoeffService(material, geometry);
            await Task.Run(() =>
            {
                kofList = cService.GetList();
            });

            return kofList;
        }
    }

   
}
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalculatorEC2Logic;
using Microsoft.AspNetCore.Mvc;
using TabeleEC2;

namespace VGGS_Calculator.Controllers
{
    public class KofZaProracunPravougaonogPresekaController : Controller
    {
        [HttpGet("/api/KofZaProracunPravougaonogPreseka")]
        public async Task<List<TabeleEC2.Model.KofZaProracunPravougaonogPresekaModelEC>> GetKofZaProracunPravougaonogPresekaAsync() 
        {
            var kofList = new List<TabeleEC2.Model.KofZaProracunPravougaonogPresekaModelEC>();
            await Task.Run(() =>
            {
                kofList = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetKofZaProracunPravougaonogPresekaList();
            });

            return kofList;
        }
    }
    public class SymmetricalReinfByClassicMethodController : Controller 
    {
        [HttpGet("/api/Symclassic")]
        public async Task<List<SymmetricalReinfByClassicMethod.μSd_And_νSdCollection>> GetKofZaProracunPravougaonogPresekaAsync()
        {

            var material = new Material()
            {
                beton = TabeleEC2.BetonClasses.GetBetonClassListEC().First(n => n.name == "C25/30"),
                armatura = ReinforcementType.GetArmatura().First(n => n.name == "B500B"),
            };
            var geometry = new ElementGeomety()
            {
                b = 20,
                d1 = 4,
                h = 40,
            };
            return new SymmetricalReinfByClassicMethod(material, geometry).GetAllLines();
        }
    }

}
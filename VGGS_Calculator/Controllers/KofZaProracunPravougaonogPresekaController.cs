using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VGGS_Calculator.Controllers
{
    public class KofZaProracunPravougaonogPresekaController : Controller
    {
        [HttpGet("/api/KofZaProracunPravougaonogPreseka")]
        public async Task<List<TabeleEC2.Model.CoeffForCalcRectCrossSectionModelEC>> GetKofZaProracunPravougaonogPresekaAsync() 
        {
            var kofList = new List<TabeleEC2.Model.CoeffForCalcRectCrossSectionModelEC>();
            await Task.Run(() =>
            {
                kofList = TabeleEC2.CoeffForCalcRectCrossSectionEC.GetList();
            });

            return kofList;
        }
    }

   
}
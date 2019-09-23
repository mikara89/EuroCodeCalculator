using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalcModels;
using Microsoft.AspNetCore.Mvc;
using VGGS_Calculator.Models;

namespace VGGS_Calculator.Controllers
{
    public class BetonClassController : Controller
    {
        [HttpGet("/api/betonclass")]
        public async Task<IEnumerable<BetonModelEC>> GetBetonClassesAsync()
        {
            IEnumerable<BetonModelEC> betonclass=null;
            await Task.Run(() =>
            {
                //betonclass = TabeleEC2.BetonClasses.GetBetonClassListEC();
                betonclass = BetonModelEC.ListOfBetonClasses();
            });

            return betonclass;
        } 
    }
}
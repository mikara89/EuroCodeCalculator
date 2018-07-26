using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VGGS_Calculator.Models;

namespace VGGS_Calculator.Controllers
{
    public class BetonClassController : Controller
    {
        [HttpGet("/api/betonclass")]
        public async Task<List<TabeleEC2.Model.BetonModelEC>> GetBetonClassesAsync()
        {
            var betonclass=new List<TabeleEC2.Model.BetonModelEC>();
            await Task.Run(() =>
            {
                betonclass = TabeleEC2.BetonClasses.GetBetonClassListEC();
            });

            return betonclass;
        } 
    }
}
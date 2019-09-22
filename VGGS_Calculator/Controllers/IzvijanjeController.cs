using CalcModels;
using CalculatorEC2Logic;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class IzvijanjeController : Controller
    {
        [HttpGet("/api/Izvijanje")]
        public ActionResult GetIzvijanje()  
        {
            var l = new List<IzvijanjeModel>();
            for (int i = 0; i < Enum.GetNames(typeof(Izvijanja)).Length; i++)
            {
                l.Add(new IzvijanjeModel()
                {
                    Name = OjleroviSlucajeviIzvijanja.GetName((Izvijanja)i),
                    Id = i,
                    K=OjleroviSlucajeviIzvijanja.GetK((Izvijanja)i),
                    Image= Enum.GetNames(typeof(Izvijanja))[i]+".jpg"
                });
            }
            return Ok(l);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class TransverzalneSileEC2Controller : Controller
    {
        private readonly ITransverzalSilaEc2Repository trans;

        public TransverzalneSileEC2Controller(ITransverzalSilaEc2Repository trans)
        {
            this.trans = trans;
        }
        [HttpPost("/api/transileec2")]
        public async Task<IActionResult> CalculateInitAsync([FromBody]TransverzalneSileEc2Model model) 
        {
            try
            {
                if (model == null) throw new ArgumentException("Pogresno uneti podaci!");
                var Result = new TransverzalneSileEc2ResultModel();
                await Task.Run(() =>
                {
                    Result = trans.CalculateInit(model);
                });

                //await Task.Delay(1000);
                return Ok(Result);
            }
            catch (Exception e)
            {
                return BadRequest(new { error="Greska: " + e.Message });  
            }
           
        }
    }
}
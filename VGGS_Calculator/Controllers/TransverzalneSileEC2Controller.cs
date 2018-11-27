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
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(
                    new
                    {
                        message = "Invalid model",
                        error = allErrors.Select(v => v.ErrorMessage),
                        modelExp = new
                        {
                            Vg = 35,
                            Vq = 50,
                            Ved = 0,
                            b = 25,
                            h = 40,
                            d1 = 4,
                            armLongitudinal = new
                            {
                                kom = 2,
                                diametar = 16,
                            },
                            armtype = "B500B",
                            betonClass = "C25/30",
                            s = 0,
                            m = 0,
                            u_diametar = 0,
                            addArm = new
                            {
                                kom = 1,
                                diametar = 16
                            }
                        }
                    });
            }
            try
            {
                if (model == null)
                    throw new ArgumentException("Pogresno uneti podaci!");
                var Result = new TransverzalneSileEc2ResultModel();
                await Task.Run(() =>
                {
                    Result = trans.CalculateInit(model);
                });

                return Ok(Result);
            }
            catch (Exception e)
            {
                return BadRequest(new { error="Greska: " + e.Message });  
            }
           
        }
        [HttpPost("/api/transileec2/list")]
        public async Task<IActionResult> CalculateInitAsync([FromBody]IEnumerable<TransverzalneSileEc2Model> model)
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(
                    new
                    {
                        message = "Invalid model",
                        error = allErrors.Select(v => v.ErrorMessage),
                        modelExp = new
                        {
                            Vg = 35,
                            Vq = 50,
                            Ved = 0,
                            b = 25,
                            h = 40,
                            d1 = 4,
                            armLongitudinal = new
                            {
                                kom = 2,
                                diametar = 16,
                            },
                            armtype = "B500B",
                            betonClass = "C25/30",
                            s = 0,
                            m = 0,
                            u_diametar = 0,
                            addArm = new
                            {
                                kom = 1,
                                diametar = 16
                            }
                        }
                    });
            }
            try
            {
                if (model == null)
                    throw new ArgumentException("Pogresno uneti podaci!");
                var Result = new List<TransverzalneSileEc2ResultModel>();
                await Task.Run(() =>
                {
                    model.ToList().ForEach(x =>
                    {
                        Result.Add(trans.CalculateInit(x));
                    });
                });

                return Ok(Result);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = "Greska: " + e.Message });
            }

        }
    }
}
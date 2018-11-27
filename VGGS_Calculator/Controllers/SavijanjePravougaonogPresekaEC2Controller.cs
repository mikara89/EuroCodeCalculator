using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using VGGS_Calculator.Core;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class SavijanjePravougaonogPresekaEC2Controller : Controller
    {
        private readonly ISavijanjePravougaonogPresekaEC2Repository sav; 

        public SavijanjePravougaonogPresekaEC2Controller(ISavijanjePravougaonogPresekaEC2Repository sav) 
        {
            this.sav = sav;
        }
        [HttpPost("/api/savPravPresEc")]
        public async Task<IActionResult> CalculateAsync([FromBody]SavijanjePravougaonogPresekaEC2Model model) 
        {
            if (!ModelState.IsValid)
            {
                var allErrors = ModelState.Values.SelectMany(v => v.Errors);
                return BadRequest(
                    new {
                        message = "Invalid model",
                        error = allErrors.Select(v=>v.ErrorMessage),
                        modelExp= new {
                            b= 25,
                            h= 40,
                            d1= 6,
                            d2= 4,
                            armtype= "B500B",
                            betonClass= "C25/30",
                            mg= 50,
                            mq= 30,
                            msd= 0,
                            ng= 50,
                            nq= 0,
                            nsd= 0,
                            mu=0
                        }
                    });
            }
                
            try
            {
                if (model == null) throw new ArgumentException("Pogresno uneti podaci!");
                
                var Result = new SavijanjePravougaonogPresekaEC2Model();
                await Task.Run(() =>
                {
                    Result = sav.Calculate(model); 
                });

                return Ok(Result);
            }
            catch (Exception e)
            {
                return BadRequest(new { error = e.Message });
            }

        }
    }
}
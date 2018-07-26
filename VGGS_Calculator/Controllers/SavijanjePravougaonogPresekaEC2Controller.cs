using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
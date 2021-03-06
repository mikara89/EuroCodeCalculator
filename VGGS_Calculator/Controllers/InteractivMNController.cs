﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using InterDiagRCSection;
using CalcModels;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Controllers
{
    public class InteractivMNController : Controller 
    {
        public InteractivMNController(ILogger<InteractivMNController> logger)
        {
            Logger = logger;
        }

        public ILogger<InteractivMNController> Logger { get; }

        [HttpPost("/api/InterMN")]
        public async Task<IActionResult> GetListOfAllLines([FromBody] MN model)
        {
            if (model == null)
            {
                Logger.LogError("model not valid");
                throw new System.ArgumentNullException(nameof(model)); 
            }
            try
            {
                var material = new Material()
                {
                    beton = new BetonModelEC(model.material.betonClass),
                    armatura = ReinforcementType.GetArmatura().First(n => n.name == model.material.armtype),
                };
                var geometry = new ElementGeometryWithReinfI()
                {
                    b_eff_top = model.geometry.b_eff,
                    h_f_top = model.geometry.h_f,
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    d2 = model.geometry.d2,
                    h = model.geometry.h,
                    As_1 = model.geometry.as1,
                    As_2 = model.geometry.as2,
                };
                var s = new Solver(material, geometry);
                await s.CalcAsync(0.5);
                Logger.LogInformation("API send interacion curves");
                return Ok(s.List.Select(x => new { x = x.M_Rd, y = x.N_Rd }));
            }
            catch (System.Exception ex)
            {
                Logger.LogError("something went wrrong: "+ ex.Message);
                return BadRequest(new { error = ex.Message });
            }

        }

        [HttpPost("/api/InterMN/search")]
        public async Task<IActionResult> SearchForLineAsync([FromBody] MN model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            try
            {
                var material = new Material()
                {
                    beton = new BetonModelEC(model.material.betonClass),
                    armatura = ReinforcementType
                    .GetArmatura()
                    .First(n => n.name == model.material.armtype),
                };

                var geometry = new ElementGeometryWithReinfI()
                {
                    b_eff_top = model.geometry.b_eff,
                    h_f_top = model.geometry.h_f,
                    b = model.geometry.b,
                    d1 = model.geometry.d1,
                    d2 = model.geometry.d2,
                    h = model.geometry.h,
                    As_1 = model.geometry.as1,
                    As_2 = model.geometry.as2,
                };

                var s = new Solver(material, geometry);
                await s.CalcAsync();
                //s.GetWorrnings(model.m, model.n);
                var isValid = s.List.IsMNValid(model.m, model.n);

                var listMaxMin = new List<SectionStrainsModel>();
                listMaxMin.AddRange(
                    s.List
                    .OrderBy(n => Math.Abs(n.M_Rd - model.m))
                    .Take(2)
                    );
                listMaxMin.AddRange(s.List.OrderBy(n => Math.Abs(n.N_Rd - model.n))
                              .Take(2));

                return Ok(
                    new
                    {
                        extrims = InfoDetailModel.Converts(listMaxMin.ToArray()),
                        isValid = isValid,
                        worrnings = s.Worrnings,
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class MN
        {
            public double m { get; set; }
            public double n { get; set; }
            public Geometry geometry { get; set; }
            public Material material { get; set; }


            public class Geometry
            {
                public double b_eff { get; set; }
                public double h_f { get; set; }
                public double b { get; set; }
                public double d1 { get; set; }
                public double d2{ get; set; }
                public double h { get; set; }
                public double as1 { get; set; }
                public double as2 { get; set; } 
            }
            public class Material
            {
                public string betonClass { get; set; }
                public string armtype { get; set; }
            }
        }


    }
}
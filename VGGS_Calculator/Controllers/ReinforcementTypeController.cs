using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CalcModels;
using Microsoft.AspNetCore.Mvc;

namespace VGGS_Calculator.Controllers
{
    public class ReinforcementTypeController : Controller
    {
        [HttpGet("/api/reinforcementtype")]
        public async Task<List<ReinforcementTypeModelEC>> GetReinforcementTypesAsync() 
        {
            var reinforcementTypes = new List<ReinforcementTypeModelEC>();
            await Task.Run(() =>
            {
                reinforcementTypes = ReinforcementType.GetArmatura();
            });

            return reinforcementTypes;  
        }
        [HttpGet("/api/reinforcementList")]
        public async Task<List<ReinforcementTabelModel>> GetReinforcementListsAsync()
        {
            var reinforcementList = new List<ReinforcementTabelModel>();
            await Task.Run(() =>
            {
                reinforcementList = ReinforcementType.GetAramturaList();
            });

            return reinforcementList;
        }
    }
}
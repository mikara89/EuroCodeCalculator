using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VGGS_Calculator.Controllers
{
    public class ReinforcementTypeController : Controller
    {
        [HttpGet("/api/reinforcementtype")]
        public async Task<List<TabeleEC2.Model.ReinforcementTypeModelEC>> GetReinforcementTypesAsync() 
        {
            var reinforcementTypes = new List<TabeleEC2.Model.ReinforcementTypeModelEC>();
            await Task.Run(() =>
            {
                reinforcementTypes = TabeleEC2.ReinforcementType.GetArmatura();
            });

            return reinforcementTypes;  
        }
        [HttpGet("/api/reinforcementList")]
        public async Task<List<TabeleEC2.Model.ReinforcementTabelModel>> GetReinforcementListsAsync()
        {
            var reinforcementList = new List<TabeleEC2.Model.ReinforcementTabelModel>();
            await Task.Run(() =>
            {
                reinforcementList = TabeleEC2.ReinforcementType.GetAramturaList();
            });

            return reinforcementList;
        }
    }
}
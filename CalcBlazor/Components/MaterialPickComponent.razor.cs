using CalcBlazor.Data.Models;
using CalcModels;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    public partial class MaterialPickComponent
    {
        private List<string> ListBeton;
        private List<string> ListArmatura;
        [Parameter]
        public MaterialModel MaterialModel { get; set; }

        protected override void OnInitialized() 
        {
            MaterialModel = new MaterialModel();

            ListBeton = new List<string>(betonService.GetNameList());
            ListArmatura = new List<string>(ReinforcementType.GetArmatura().Select(x => x.name));

            MaterialModel.SelectedReinf = ListArmatura.FirstOrDefault(x => x == "B500B");
            MaterialModel.SelectedConcrete = ListBeton.FirstOrDefault(x => x == "C25/30");
        }

        [Parameter]
        public EventCallback<MaterialModel> MaterialModelChanged { get; set; } 
    
        async Task PropertyConcreteChanged(ChangeEventArgs e)
        {
            MaterialModel.SelectedConcrete = e.Value as string;
            await MaterialModelChanged.InvokeAsync(MaterialModel);
        }
        async Task PropertyReinfChanged(ChangeEventArgs e) 
        {
            MaterialModel.SelectedReinf = e.Value as string;
            await MaterialModelChanged.InvokeAsync(MaterialModel);
        }
    }
}

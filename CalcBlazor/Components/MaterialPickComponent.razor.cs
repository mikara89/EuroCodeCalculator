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
        public MaterialModel model { get; set; }

        protected override async Task OnInitializedAsync()
        {
            model = new MaterialModel();

            ListBeton = new List<string>(betonService.GetNameList());
            ListArmatura = new List<string>(ReinforcementType.GetArmatura().Select(x => x.name));

            model.SelectedReinf = ListArmatura.FirstOrDefault(x => x == "B500B");
            model.SelectedConcrete = ListBeton.FirstOrDefault(x => x == "C25/30");
        }

        [Parameter]
        public EventCallback OnPropertyChanged { get; set; }
        async Task PropertyChanged()
        {
            await Task.Delay(1000);
            await OnPropertyChanged.InvokeAsync(null);
        }
    }
}

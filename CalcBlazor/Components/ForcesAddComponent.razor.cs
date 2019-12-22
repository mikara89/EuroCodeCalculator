using CalcBlazor.Data.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    public partial class ForcesAddComponent
    {
        public AddForcesModel model { get; set; } 
            = new AddForcesModel() { M = 30, N = -1600.0 };
        private ObservableCollection<AddForcesModel> TableData;
        private EventCallback<ObservableCollection<AddForcesModel>> OnForcesAdded;

        protected override void OnInitialized()
        {
            if (TableData == null)
                TableData = new ObservableCollection<AddForcesModel>();
            TableData.CollectionChanged += (s, e) =>
            {
                OnForcesAdded.InvokeAsync(TableData);
            };
        }

        private async Task HandleValidSubmit()
        {
            TableData.Add(model);
        }
    }
}

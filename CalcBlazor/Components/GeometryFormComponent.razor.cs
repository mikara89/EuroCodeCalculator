using CalcBlazor.Data.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    public partial class GeometryFormComponent
    {
        [Parameter]
        public GeometryModel GeometryModel { get; set; } = new GeometryModel
        { 
            b=30,
            h=30,
            d1=6,
            d2=6,
            As_1=6.8,
            As_2=6.8
        };

        [Parameter]
        public EventCallback OnPropertyChanged { get; set; }
        async Task PropertyChanged(FocusEventArgs e)
        {
            await Task.Delay(1000);
            await OnPropertyChanged.InvokeAsync(null);
        }
    
}
   
}

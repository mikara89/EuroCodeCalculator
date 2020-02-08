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
        public GeometryModel GeometryModel { get; set; }

        [Parameter]
        public EventCallback<GeometryModel> GeometryModelChanged { get; set; }
        async Task PropertyChanged()
        {
            await GeometryModelChanged.InvokeAsync(GeometryModel);
        }

        public void OnSectionChange(object item)
        {
            GeometryModel.SectionType = (SectionType)item;

            //Reseting after section type change
            switch (GeometryModel.SectionType)
            {
                case SectionType.Rectangle:
                    GeometryModel.h_f_bottom = 0;
                    GeometryModel.b_eff_bottom = 0;
                    GeometryModel.h_f_top = 0;
                    GeometryModel.b_eff_top = 0;
                    break;
                case SectionType.Simetrical_T:
                    GeometryModel.h_f_bottom = 0;
                    GeometryModel.b_eff_bottom = 0;
                    break;
                case SectionType.Simetrical_I:
                    break;
                default:
                    break;
            }
        }
    }
 
}

using CalcBlazor.Data.Models;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using ChartJs.Blazor.ChartJS.Common;

namespace CalcBlazor.Components
{
    public partial class ForcesAddComponent
    {
        public AddForcesModel model { get; set; }
            = new AddForcesModel() { M = 30, N = -1600.0 };

        /// <summary>
        /// Returns IEnumerable<Point>
        /// </summary>
        [Parameter]
        public EventCallback<Point> OnForcesAdded { get; set; }

        private void HandleValidSubmit()
        {
            OnForcesAdded.InvokeAsync(new Point { X=model.M, Y=model.N });
        }
    }
}

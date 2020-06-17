using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Data.Services
{
    public class ChartJSCustom
    {
        private readonly IJSRuntime _js;

        public ChartJSCustom(IJSRuntime js)
        {
            _js = js;
        }

        public async Task RevrseChartY(string id)
        {
            await _js.InvokeAsync<object>("chartJSCustom.revrseChartY", id);
        }
    }
}

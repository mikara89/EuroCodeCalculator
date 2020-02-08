using CalcBlazor.Data.Models;
using CalcBlazor.Data.Settings;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    public partial class SettingsComponent
    {

        [CascadingParameter(Name = "IDoSomethingParentControl")]
        private ModelDialogComponent CurrentParentControl { get; set; }
        private SettingsModel model { get; set; }

        private string text = "";

        protected override void OnInitialized()
        {
            model = new SettingsModel();
            model.alfa_cc = MaterialSettings.alfa_cc;
            model.alfa_ct = MaterialSettings.alfa_ct;
            model.gama_c = MaterialSettings.gama_c;
            model.gama_s = MaterialSettings.gama_s;
        }
        protected override void OnParametersSet()
        {

            if (CurrentParentControl != null)
            {
                Title = "Settings";
                CurrentParentControl._childControls = this;
            }
        }

        public override async Task SaveAsync()
        {
            text = "loading";
            await Task.Delay(1000);
            HandelSubmite();
            text = "Done!";
        }
        void HandelSubmite()
        {
            MaterialSettings.alfa_cc=model.alfa_cc ;
            MaterialSettings.alfa_ct=model.alfa_ct  ;
            MaterialSettings.gama_c=model.gama_c ;
            MaterialSettings.gama_s=model.gama_s  ;
        }
    }
}

using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Components
{
    public partial class ModelDialogComponent
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public Guid Guid = Guid.NewGuid();
        public string ModalDisplay = "none;";
        public string ModalClass = "";
        public bool ShowBackdrop = false;
        private bool IsSaving;

        public void Open()
        {
            ModalDisplay = "block;";
            ModalClass = "Show";
            ShowBackdrop = true;
        }

        public void Close()
        {
            ModalDisplay = "none";
            ModalClass = "";
            ShowBackdrop = false;
            StateHasChanged();
        }

        public IModelDialogChild _childControls;

        public async Task SaveHandler() 
        {
            IsSaving = true;
            await _childControls?.SaveAsync();
            IsSaving = false;
        }
    }

    public interface IModelDialogChild
    { 
        Task SaveAsync();
        string Title { get; set; }
    }
    public abstract class ModelDialogChildBase :ComponentBase, IModelDialogChild
    {
        public string Title { get; set; }

        public abstract Task SaveAsync();
    }
}

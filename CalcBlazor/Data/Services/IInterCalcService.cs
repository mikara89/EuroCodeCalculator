using CalcBlazor.Data.Models;
using ChartJs.Blazor.ChartJS.Common;
using InterDiagRCSection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace CalcBlazor.Data.Services
{
    public interface IInterCalcService
    {
        ObservableCollection<IEnumerable<SectionStrainsModel>> Results { get; }
        ObservableCollection<Point> ForcesPoints { get; }
        ObservableCollection<IEnumerable<Point>> ResultsAsPoint { get; }
        Task AddDiagramLines(MaterialModel materalModel, GeometryModel geometryModel, bool IsUpdate = false);
        void RemoveDiagramLines(int index);

        void SelectedItem(int index);
        Action<SectionStrainsModel> OnUpdatingItem { get; set; }
        void AddForcePoint(Point point);
        Task GetInfoForForcePoint(int indexPoint);
    }
}
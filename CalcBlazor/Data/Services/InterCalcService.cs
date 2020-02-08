using CalcBlazor.Data.Models;
using CalcModels;
using ChartJs.Blazor.ChartJS.Common;
using InterDiagRCSection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace CalcBlazor.Data.Services
{
    public class InterCalcService : IInterCalcService
    {
        private int SelectedItemIndex;
        private IMaterial Material;
        private IElementGeometryWithReinf Geometry;
        public ObservableCollection<IEnumerable<SectionStrainsModel>> Results { get; }
        public ObservableCollection<IEnumerable<Point>> ResultsAsPoint { get; }
        public ObservableCollection<Point> ForcesPoints { get; }
        private readonly IBetonService betonServices;

        public InterCalcService(IBetonService betonServices)
        {
            this.betonServices = betonServices;
            Results = new ObservableCollection<IEnumerable<SectionStrainsModel>>();
            ResultsAsPoint = new ObservableCollection<IEnumerable<Point>>();
            ForcesPoints = new ObservableCollection<Point>();
        }

        public async Task AddDiagramLines(MaterialModel materalModel, GeometryModel geometryModel, bool IsUpdate = false)
        {
            if (materalModel is null)
            {
                throw new ArgumentNullException(nameof(materalModel));
            }

            if (geometryModel is null)
            {
                throw new ArgumentNullException(nameof(geometryModel));
            }
            CreateMaterialAndGeometry(materalModel, geometryModel);

            var solver = new Solver(Material, Geometry);
            await solver.CalcAsync(0.5);

            if (!IsUpdate)
            {
                Results.Add(solver.List);
                ResultsAsPoint.Add(new List<Point>(solver.List.Select(c => new Point { X = c.M_Rd, Y = c.N_Rd })));
            }
            else
            {
                Results[SelectedItemIndex] = solver.List;
                ResultsAsPoint[SelectedItemIndex] = new List<Point>(solver.List.Select(c => new Point { X = c.M_Rd, Y = c.N_Rd }));
            }

        }
        public void AddForcePoint(Point point) => 
            ForcesPoints.Add(point);
        public async Task GetInfoForForcePoint(int indexPoint) =>
            throw new NotImplementedException();

        public void RemoveDiagramLines(int index)
        {
            Results.Remove(Results[index]);
        }

        private void CreateMaterialAndGeometry(MaterialModel materalModel, GeometryModel geometryModel)
        {
            Material = new Material
            {
                armatura = ReinforcementType.GetArmatura().Find(x => x.name == materalModel.SelectedReinf),
                beton = betonServices.GetNew(materalModel.SelectedConcrete),
            };

            Material.beton.αcc = Settings.MaterialSettings.alfa_cc;

            Geometry = new ElementGeometryWithReinfI
            {
                b = geometryModel.b,
                h = geometryModel.h,
                d1 = geometryModel.d1,
                d2 = geometryModel.d2,
                As_1 = geometryModel.As_1,
                As_2 = geometryModel.As_2,
                b_eff_top = geometryModel.b_eff_top,
                h_f_top = geometryModel.h_f_top,
                b_eff_bottom = geometryModel.b_eff_bottom,
                h_f_bottom = geometryModel.h_f_bottom,
            };
        }

        public Action<SectionStrainsModel> OnUpdatingItem { get; set; }

        public void SelectedItem(int index)
        {
            SelectedItemIndex = index;
            OnUpdatingItem?.Invoke(index != -1 ? Results[index].FirstOrDefault() : null);
        }
    }
}

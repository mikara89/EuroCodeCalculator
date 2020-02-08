using CalcModels;
using InterDiagRCSection;
using System;

namespace CalcBlazor.Data.Models
{
    public static class ModelExtensions
    {
        public static void GeometryModelFromSectionStrainsModel(this GeometryModel geometry, SectionStrainsModel strainsModel)
        {
            if (strainsModel.geometry is ElementGeometryWithReinfI)
            {
                strainsModel.geometry.Invert(false);
                var geo = strainsModel.geometry as ElementGeometryWithReinfI;

                geometry.b = geo.b;
                geometry.h = geo.h;
                geometry.As_1 = geo.As_1;
                geometry.As_2 = geo.As_2;
                geometry.d1 = geo.d1;
                geometry.d2 = geo.d2;
                geometry.b_eff_top = geo.b_eff_top;
                geometry.h_f_top = geo.h_f_top;
                geometry.b_eff_bottom = geo.b_eff_bottom;
                geometry.h_f_bottom = geo.h_f_bottom;

                if (geometry.b_eff_bottom==0)
                {
                    if (geometry.b_eff_top == 0)
                        geometry.SectionType = SectionType.Rectangle;
                    else
                        geometry.SectionType = SectionType.Simetrical_T;
                }
                else
                    geometry.SectionType = SectionType.Simetrical_I;
            }
            else
                throw new NotImplementedException("Section not implemented");
         
        }
        public static void MaterialModelFromSectionStrainsModel(this MaterialModel material, SectionStrainsModel strainsModel)
        {
            material.SelectedConcrete = strainsModel.material.beton.name;
            material.SelectedReinf = strainsModel.material.armatura.name;
        }
    }
}


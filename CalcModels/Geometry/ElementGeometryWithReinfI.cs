using System;
using System.Collections.Generic;

namespace CalcModels
{
    public class ElementGeometryWithReinfI : ElementGeometryWithReinfBase
    {
        public double b_eff_top { get; set; }
        public double h_f_top { get; set; }
        public double b_eff_bottom { get; set; }
        public double h_f_bottom { get; set; }

        public override bool IsInverted { get; internal set; } = false;

        public override double calc_I()
        {
            var A_b = (h - h_f_top - h_f_bottom) * b; 
            var A_top = h_f_top * b_eff_top;
            var A_bottom = h_f_bottom * b_eff_bottom; 

            var y_b = h_f_bottom + (h - h_f_top - h_f_bottom) / 2;
            var y_top = h - (h_f_top / 2);
            var y_bottom = h_f_bottom / 2;

            var I_b = 1.0 / 12.0 * b * Math.Pow(h - h_f_top - h_f_bottom, 3);
            var I_top = 1.0 / 12.0 * b_eff_top * Math.Pow(h_f_top, 3);
            var I_bottom = 1.0 / 12.0 * b_eff_bottom * Math.Pow(h_f_bottom, 3);

            var y = calc_y1();

            List<Tuple<double, double, double>> listTuple = new List<Tuple<double, double, double>>();
            listTuple.Add(new Tuple<double, double, double>(I_b, A_b, y_b));
            listTuple.Add(new Tuple<double, double, double>(I_top, A_top, y_top));
            listTuple.Add(new Tuple<double, double, double>(I_bottom, A_bottom, y_bottom));

            double result = 0.0;
            foreach (var item in listTuple)
            {
                result += item.Item1 + (item.Item2 * Math.Pow(y - item.Item3,2));
            }
            return result;
        }

        public override double calc_y1()
        {
            var Ac1 = (h - h_f_top- h_f_bottom) * b;
            var Ac2 = h_f_top * b_eff_top;
            var Ac3 = h_f_bottom * b_eff_bottom; 
            var yc1 = h_f_bottom+(h - h_f_top - h_f_bottom) / 2;
            var yc2 = h - (h_f_top / 2);
            var yc3 = h_f_bottom / 2;
            return (Ac1 * yc1 + Ac2 * yc2 + Ac3 * yc3) / (Ac1 + Ac2 + Ac3);
        }

        public override double calc_y2()
        {
            return h - y1;
        }

        public override double Get_b(double z)
        {
            if (h_f_top == 0 && h_f_bottom == 0)
                return b;
            else
            {
                if (z > h - h_f_top)
                    return b_eff_top;
                else if (z > h_f_bottom && z < h - h_f_top)
                    return b;
                else return b_eff_bottom;
            }
        }

        public override void Invert(bool isInverted=true)
        {
            if (IsInverted == isInverted)
                return;
            IsInverted = isInverted;
            var newAs1 = As_2;
            var newAs2 = As_1;
            var new_d1 = d2;
            var new_d2 = d1;
            var newB_eff_t = b_eff_bottom;
            var newB_eff_b = b_eff_top;
            var newh_t = h_f_bottom;
            var newh_b = h_f_top;

            As_1 = newAs1;
            As_2 = newAs2;
            d2 = new_d2;
            d1 = new_d1;
            b_eff_bottom = newB_eff_b;
            b_eff_top = newB_eff_t;
            h_f_bottom = newh_b;
            h_f_top = newh_t;
        }
    }
    public class ElementGeometryStiffnessI : ElementGeometryStiffnessBase
    {
        public double b_eff_top { get; set; }
        public double h_f_top { get; set; }
        public double b_eff_bottom { get; set; }
        public double h_f_bottom { get; set; }


        public override double calc_I()
        {
            var A_b = (h - h_f_top - h_f_bottom) * b;
            var A_top = h_f_top * b_eff_top;
            var A_bottom = h_f_bottom * b_eff_bottom;

            var y_b = h_f_bottom + (h - h_f_top - h_f_bottom) / 2;
            var y_top = h - (h_f_top / 2);
            var y_bottom = h_f_bottom / 2;

            var I_b = 1.0 / 12.0 * b * Math.Pow(h - h_f_top - h_f_bottom, 3);
            var I_top = 1.0 / 12.0 * b_eff_top * Math.Pow(h_f_top, 3);
            var I_bottom = 1.0 / 12.0 * b_eff_bottom * Math.Pow(h_f_bottom, 3);

            var y = calc_y1();

            List<Tuple<double, double, double>> listTuple = new List<Tuple<double, double, double>>();
            listTuple.Add(new Tuple<double, double, double>(I_b, A_b, y_b));
            listTuple.Add(new Tuple<double, double, double>(I_top, A_top, y_top));
            listTuple.Add(new Tuple<double, double, double>(I_bottom, A_bottom, y_bottom));

            double result = 0.0;
            foreach (var item in listTuple)
            {
                result += item.Item1 + (item.Item2 * Math.Pow(y - item.Item3, 2));
            }
            return result;
        }

        public override double calc_y1()
        {
            var Ac1 = (h - h_f_top - h_f_bottom) * b;
            var Ac2 = h_f_top * b_eff_top;
            var Ac3 = h_f_bottom * b_eff_bottom;
            var yc1 = h_f_bottom + (h - h_f_top - h_f_bottom) / 2;
            var yc2 = h - (h_f_top / 2);
            var yc3 = h_f_bottom / 2;
            return (Ac1 * yc1 + Ac2 * yc2 + Ac3 * yc3) / (Ac1 + Ac2 + Ac3);
        }

        public override double calc_y2()
        {
            return h - y1;
        }
    }
}

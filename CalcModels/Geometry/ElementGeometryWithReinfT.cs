namespace CalcModels
{
    public class ElementGeometryWithReinfT : ElementGeometryWithReinfBase
    {
        public double b_eff { get; set; }
        public double h_f { get; set; }
        public override bool IsInverted { get ; internal set ; }

        public override double calc_I()
        {
            throw new System.NotImplementedException();
        }

        public override double calc_y1()
        {
            var Ac1 = (h - h_f) * b;
            var Ac2 = h_f * b_eff;
            var yc1 = (h - h_f) / 2;
            var yc2 = h - (h_f / 2);
            return (Ac1 * yc1 + Ac2 * yc2) / (Ac1 + Ac2);
        }

        public override double calc_y2()
        {
            return  h - y1;
        }

        public override double Get_b(double z)
        {
            if (h_f == 0)
                return b;
            else
                return z > h - h_f ? b_eff : b;
        }

        public override void Invert(bool isInvert=true)
        {
            throw new System.NotImplementedException();
     
        }
    }
}

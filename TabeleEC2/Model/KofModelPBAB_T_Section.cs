namespace TabeleEC2.Model
{
    public class KofModelPBAB_T_Section: KofZaProracunPravougaonogPresekaModelPBAB
    {
        private KofModelPBAB_T_Section(double s, double s_old)
        {
            var k = SetBy_S_PBAB(s);
            εb = k.εb;
            εa1 = k.εa1;
            Set_εbd(s, s_old);
        }
        public double εbd { get; set; }
        public double αb2
        {
            get
            {
                return alfa(εbd);
            }
        }
        public double η2
        {
            get
            {
                return calc_η(εbd); 
            }
        }
        public static KofModelPBAB_T_Section SetBy(double s,  double s_old)
        {
            return new KofModelPBAB_T_Section(s, s_old);
        }
        /// <summary>
        /// Calculate sigma by given paramiters
        /// </summary>
        /// <param name="εa">in 0/00</param>
        /// <param name="Ea">MPa(kN/cm2)</param>
        /// <param name="sigma_v">MPa(kN/cm2)</param>
        /// <returns>value in MPa(kN/cm2)</returns>
        public static double GetSigma(double εa, double Ea, double sigma_v)
        {
            εa = System.Math.Abs(εa);
            return εa/1000*Ea>= sigma_v? sigma_v: εa / 1000 * Ea;
        }
        public static KofModelPBAB_T_Section SetBy(double s, double d,double dp, double a1, double a2) 
        {
            return new KofModelPBAB_T_Section(s, s_old:dp/(d-a1));
        }
        protected void Set_εbd(double s, double s_old)
        {
            εbd=(s - s_old) / s * εb;
        }
    }
}

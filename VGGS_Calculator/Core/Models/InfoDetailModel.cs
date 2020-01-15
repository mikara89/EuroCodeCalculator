using InterDiagRCSection;
using System.Collections.Generic;

namespace VGGS_Calculator.Core.Models
{
    public class InfoDetailModel
    {
        public double Fc { get; set; }
        public double Fs2 { get; set; }
        public double Fs1 { get; set; }
        public double N_Rd { get; set; }
        public double M_Rd { get; set; }
        public double eps_c2 { get; set; }
        public double eps_c1 { get; set; }
        public double eps_s1 { get; set; }
        public double eps_s2 { get; set; }
        public double sig_c { get; set; }
        public double sig_s1 { get; set; }
        public double sig_s2 { get; set; }
        public double x { get; set; }

        private InfoDetailModel(ICrossSectionStrains crossSection)
        {
            Fc = crossSection.F_c;
            Fs2 = crossSection.F_s2;
            Fs1 = crossSection.F_s1;
            N_Rd = crossSection.N_Rd;
            M_Rd = crossSection.M_Rd;
            eps_c1 = crossSection.eps_c1;
            eps_c2 = crossSection.eps_c; 
            eps_s1 = crossSection.eps_s1;
            eps_s2 = crossSection.eps_s2;
            sig_c = crossSection.sig_c;
            sig_s1 = crossSection.sig_s1;
            sig_s2 = crossSection.sig_s2;
            x = crossSection.x;
        }
        public static InfoDetailModel Convert(ICrossSectionStrains crossSection) =>
            new InfoDetailModel(crossSection);
        public static InfoDetailModel[] Converts(ICrossSectionStrains[] crossSections)
        {
            List<InfoDetailModel> result = new List<InfoDetailModel>();
            foreach (var item in crossSections)
            {
                result.Add(Convert(item));
            }
            return result.ToArray();
        }

    }
}

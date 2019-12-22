﻿using InterDiagRCSection;
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

        private InfoDetailModel(CrossSectionStrains crossSection)
        {
            Fc = crossSection.Fc;
            Fs2 = crossSection.Fs2;
            Fs1 = crossSection.Fs1;
            N_Rd = crossSection.N_Rd;
            M_Rd = crossSection.M_Rd;
            eps_c1 = crossSection.εc1;
            eps_c2 = crossSection.εc2;
            eps_s1 = crossSection.εs1;
            eps_s2 = crossSection.εs2;
            sig_c = crossSection.sig_c;
            sig_s1 = crossSection.sig_s1;
            sig_s2 = crossSection.sig_s2;
            x = crossSection.x;
        }
        public static InfoDetailModel Convert(CrossSectionStrains crossSection) =>
            new InfoDetailModel(crossSection);
        public static InfoDetailModel[] Converts(CrossSectionStrains[] crossSections)
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
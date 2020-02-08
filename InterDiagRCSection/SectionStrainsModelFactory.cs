namespace InterDiagRCSection
{
    public class SectionStrainsModelFactory
    {
        public static SectionStrainsModel Create(RCSectionCalc rCSectionCalc)
        {
            return new SectionStrainsModel
            {
                M_Rd = rCSectionCalc.M_Rd,
                N_Rd = rCSectionCalc.N_Rd,
                c = rCSectionCalc.sectionStrains.c,
                sig_c=rCSectionCalc.Forces["Fc"].Sigma,
                F_c = rCSectionCalc.Forces["Fc"].F,
                z_c = rCSectionCalc.Forces["Fc"].Z,
                sig_s1 = rCSectionCalc.Forces["Fs1"].Sigma,
                F_s1 = rCSectionCalc.Forces["Fs1"].F,
                z_s1 = rCSectionCalc.Forces["Fs1"].Z,
                sig_s2 = rCSectionCalc.Forces["Fs2"].Sigma,
                F_s2 = rCSectionCalc.Forces["Fs2"].F,
                z_s2 = rCSectionCalc.Forces["Fs2"].Z,
                x=rCSectionCalc.sectionStrains.x,
                eps_c=rCSectionCalc.sectionStrains.eps_c,
                eps_c1=rCSectionCalc.sectionStrains.eps_c1,
                eps_s1= rCSectionCalc.sectionStrains.eps_s1,
                eps_s2= rCSectionCalc.sectionStrains.eps_s2,
                material=rCSectionCalc.sectionStrains.material,
                geometry=rCSectionCalc.sectionStrains.geometry
            };
        }
    }
}

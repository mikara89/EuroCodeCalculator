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
                Fc = rCSectionCalc.Forces["Fc"].F,
                Zc = rCSectionCalc.Forces["Fc"].Z,
                sig_s1 = rCSectionCalc.Forces["Fs1"].Sigma,
                Fs1 = rCSectionCalc.Forces["Fs1"].F,
                Zs1 = rCSectionCalc.Forces["Fs1"].Z,
                sig_s2 = rCSectionCalc.Forces["Fs1"].Sigma,
                Fs2 = rCSectionCalc.Forces["Fs1"].F,
                Zs2 = rCSectionCalc.Forces["Fs1"].Z,
                x=rCSectionCalc.sectionStrains.x,
            };
        }
    }
}

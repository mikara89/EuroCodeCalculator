using System;
using System.Collections.Generic;
using System.Text;
using TabeleEC2.Model;

namespace CalculatorEC2Logic.SavijanjePreseka.EC2
{

    public class SavijanjePravougaonogPresekaEC2_V2
    {
        public double h { get; set; } 
        public KofZaProracunPravougaonogPresekaModelEC Kof { get; set; }
        public TipDimenzionisanja TipDim { get; set; } = TipDimenzionisanja.Vezano;
        public bool IsCalcDone { get; set; } 
        private bool IsMsdNegativ = false;
        public StepHolder_EC2 Steper { get; set; }  
        public double Msd { get; private set; }
        public double Mrd_limit { get; private set; }
        public double Nsd { get; private set; }
        public double Msds { get; private set; }
        public BetonModelEC beton { get; set; }
        public ReinforcementTypeModelEC armatura { get; set; }
        public double Mg { get; }
        public double Mq { get; }
        public double Ng { get; }
        public double Nq { get; }
        public double b { get; private set; }

        public double d1 { get; private set; }
        public double d2 { get; private set; }
        public double d { get { return h - d1; } }

        public double μSd { get; private set; }
        public bool IsCalcValid { get; private set; }

        public double As1_pot { get; private set; }
        public double As2_pot { get; private set; }
        /// <summary>
        /// ρ_max = 4%
        /// </summary>
        public double ρ_max => 0.04;

        public double ρ { get; private set; }

        public SavijanjePravougaonogPresekaEC2_V2(double b, double h,
            double d1, double d2,
            BetonModelEC beton, ReinforcementTypeModelEC armatura,
            double Mg, double Mq, double Ng = 0, double Nq = 0, KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            this.b = b;
            this.h = h;
            if (h == 0) TipDim = TipDimenzionisanja.Slobodno;
            this.d1 = d1;
            this.d2 = d2;
            this.beton = beton;
            this.armatura = armatura;
            this.Mg = Mg;
            this.Mq = Mq;
            this.Ng = Ng;
            this.Nq = Nq;
            Kof = kof;

            InitValidation();
            //Calc();

        }
        public SavijanjePravougaonogPresekaEC2_V2(double b, double h,
    double d1, double d2,
    BetonModelEC beton, ReinforcementTypeModelEC armatura,
    double Msd, double Nsd = 0, KofZaProracunPravougaonogPresekaModelEC kof = null)
        {
            this.b = b;
            this.h = h;
            if (h == 0) TipDim = TipDimenzionisanja.Slobodno;
            this.d1 = d1;
            this.d2 = d2;
            this.beton = beton;
            this.armatura = armatura;
            this.Msd = Msd;
            this.Nsd = Nsd;
            Kof = kof;

            InitValidation();
            //Calc();

        }
        public void Calc()
        {
            Steper = new StepHolder_EC2();
            try
            {
                Calc_f();
                Calc_MsdNsd();
                Calc_Msds();
                Calc_μSd();
                Calc_Mrd_lim();
                Calc_As();
                As_validation();
                IsCalcDone = true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Calulation got error: {ex.Message}");
            }
        }

        private void InitValidation()
        {
            if (b <= 0)
                throw new Exception("b must be greater 0");
            if (h < 0)
                throw new Exception("h must be greater or equal to 0");
            if (d1 <= 0)
                throw new Exception("d1 must be greater 0");
            if (d2 <= 0)
                throw new Exception("d2 must be greater 0");
            if (2 * d1 >= h && h != 0)
                throw new Exception("2 x d1 must be smoller then h");
            if (beton == null)
                throw new Exception("Beton not defined!");
            if (armatura == null)
                throw new Exception("Armatura not defined!");
        }

        private void Calc_f()
        {
            Steper.AddStep($"fcd=0.85*fck/1.5=0.85*{beton.fck/10}[KNcm]/1.5= {Math.Round((beton.fcd/10),2)}[KNcm]","Racunska cvrstoca pri pritisku betona");
            Steper.AddStep($"fyd=fyk/1.15={Math.Round(armatura.fyk,2)}[KNcm]/1.15= {Math.Round(armatura.fyd,2)}[KNcm]", "Racunska vrednost granice razvlacenja armature");
        }
        private void Calc_MsdNsd()
        {
            if (Msd != 0) return;
            Msd = 1.35 * Mg + 1.5 * Mq;
            Steper.AddStep($"Msd=1.35*Mg[KNm]+1.5*Mq[KNm]={Math.Round(Msd,2)}[KNm];", "Racunski momenat savijanja");
            Nsd = 1.35 * Ng + 1.5 * Nq;
            Steper.AddStep($"Nsd=1.35*Ng[KNm]+1.5*Nq[KNm]={Math.Round(Nsd,2)}[KNm];", "Racunska normalna sila");
        }
        private void Calc_Msds()
        {
            Msds = Msd + Nsd * (h / 100 / 2 - d1 / 100);
            Steper.AddStep($"Msds=Msd[KNm]+Nsd[KN]*(h[m]/2-d1[m])={Math.Round(Msd,2)}+{Math.Round(Nsd,2)}*({h / 100 / 2}-{d1 / 100})[KNm]= {Math.Round(Msds,2)}[KNm];", "Racunski momenat savijanja u odnosu na teziste zategnute armature");
        }
        private void Calc_μSd()
        {
            var lim = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetLimitKofZaProracunPravougaonogPresekaEC(beton);
            if (TipDim == TipDimenzionisanja.Vezano)
            {
                μSd = TabeleEC2.KofZaProracunPravougaonogPresekaEC.GetμSd(Msds, b, d, beton.fcd / 10);
                Kof = TabeleEC2.KofZaProracunPravougaonogPresekaEC.Get_Kof_From_μ(μSd);
                Steper.AddStep($"μSd=Msd[KNcm] / (b[cm] * d[cm]^2 * fcd[KNcm])= {Math.Round(Kof.μRd,3)}", "Bezdimenziona vrednost Momenta savijanja");
            }
            else
            {
                Steper.AddStep($"μSd= {Math.Round(Kof.μRd,3)}", "Bezdimenziona vrednost Momenta savijanja odabrana za slobodno armiranje");
            }
              

            if (μSd >= lim.μRd)
            {
                Steper.AddStep($"μSd ≥ μlim= {Math.Round(lim.μRd,3)}; => μSd={Math.Round(lim.μRd,3)}", "Dvojno armiranje");
                Kof = lim;
            }
            else {
                Steper.AddStep($"μSd < μlim= {Math.Round(lim.μRd,3)}; => μSd={Math.Round(Kof.μRd,3)}", "Dvojno armiranje");
            }

            Steper.AddStep($"{nameof(Kof.εc)}= {Math.Round(Kof.εc,2)}[‰]; {nameof(Kof.εs1)}= {Math.Round(Kof.εs1,3)}[‰]; {nameof(Kof.ω)}= {Math.Round(Kof.ω,3)}; {nameof(Kof.ζ)}= {Math.Round(Kof.ζ,3)}", "Diletacija po betonu i armaturi i mehanicki kofeficijen armiranja");
        }
  
      
        private void Calc_Mrd_lim()
        {
            Mrd_limit = (Kof.μRd * b * Math.Pow(d, 2) * beton.fcd / 10)/100;
            Steper.AddStep($"Mrd_limit=μSd*b*d^2+fcd= {Math.Round(Kof.μRd,3)}*{b}*{d}^2*{Math.Round(beton.fcd / 10,2)}= {Math.Round(Mrd_limit,2) *100}[KNcm]= {Math.Round(Mrd_limit,2)}[KNm];",
                "Maksimalni moment savijanja koji jednostruko armirani presek moze da prihvati");
        }
        private void Calc_As() 
        {
            As1_pot = Mrd_limit * 100 / (Kof.ζ * d * armatura.fyd) - (Nsd / armatura.fyd) + (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);
            As2_pot = (Msds * 100 - Mrd_limit * 100) / ((d - d2) * armatura.fyd);

            Steper.AddStep($"As1=Mrd_limit/(ζ*d*fyd)-Nsd/fyd+(Mrd_limit-Msds)/((d-d2)*fyd)= {Math.Round(Mrd_limit * 100,2)} / ({Math.Round(Kof.ζ,3)} * {d} * {Math.Round(armatura.fyd,2)}) - ({Math.Round(Nsd,2)} / {Math.Round(armatura.fyd,2)}) + ({Math.Round(Msds * 100,2)} - {Math.Round(Mrd_limit * 100,2)}) / (({d} - {d2}) * {Math.Round(armatura.fyd,2)})= {Math.Round(As1_pot,2)}[cm2];",
                "Potrebna armatura u zoni zatezanja");
            Steper.AddStep($"As2=(Msds - Mrd_limit) / ((d - d2) * fyd)= ({Math.Round(Msds * 100,2)} - {Math.Round(Mrd_limit * 100,2)}) / (({d} - {d2}) * {Math.Round(armatura.fyd,2)})= {Math.Round(As2_pot,2)} [cm2]",
                "Potrebna armatura u zoni pritiska");
        }
        private void Calc_As_min() 
        {
            As1_pot = 0.26 * beton.fctm / armatura.fyd * d * b;
            if (As1_pot < beton.ρ * b * d) As1_pot = beton.ρ * b * d;

            Steper.AddStep($"As1=Mrd_limit/(ζ*d*fcd)-Nsd/fyd+(Mrd_limit-Msds)/((d-d2)*fyd)= {Mrd_limit * 100} / ({Kof.ζ} * {d} * {armatura.fyd}) - ({Nsd} / {armatura.fyd}) + ({Msds * 100} - {Mrd_limit * 100}) / (({d} - {d2}) * {armatura.fyd})= {As1_pot}[cm2];",
                "Potrebna armatura u zoni zatezanja");
            Steper.AddStep($"As2 = (Msds - Mrd_limit) / ((d - d2) * fyd)= ({Msds * 100} - {Mrd_limit * 100}) / (({d} - {d2}) * {armatura.fyd})",
                "Potrebna armatura u zoni pritiska");
        }

        private void As_validation()
        {
            ρ = (Math.Round(As1_pot,2) + Math.Round(As2_pot,2)) / b / h;
            Steper.AddStep($"ρ = (As1 + As2) / (b * h)= ({Math.Round(As1_pot,2)} + {Math.Round(As2_pot,2)}) / {b} / {h}= {ρ}; ρ_max= {Math.Round(ρ_max,2)}",
                          "Provera maksimalnog procenta armiranja");
        }

    }
    public interface IStepHolder
    {
        List<Step> Steps { get; set; }
    }
    public class StepHolder_EC2 : IStepHolder  
    {
        public List<Step> Steps { get; set ; }

        public void AddStep(string TextString, string Description)
        {
            if (TextString == null) throw new NullReferenceException(message: $"{nameof(TextString)}: {TextString}");
            if (Steps == null)
                Steps = new List<Step>();
            Steps.Add(new Step() { StepText = TextString, StepNumber = Steps.Count + 1, Description= Description });
        }
    }
    public class Step 
    {
        public int StepNumber { get; set; }
        public string StepText { get; set; } 
        public string Description { get; set; }
    }
}

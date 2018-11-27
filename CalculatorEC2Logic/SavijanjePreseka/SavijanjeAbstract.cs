using System;
using TabeleEC2.Model;

namespace CalculatorEC2Logic.SavijanjePreseka
{
    public abstract class SavijanjeAbstract :ISavijanje,IDisposable 
    {
        //protected double h { get; set; }
        //protected KofZaProracunPravougaonogPresekaModel KofZaProracunPravougaonogPreseka { get; set; }
        //protected TipDimenzionisanja TipDim { get; set; }

        public abstract void Start();
        //{
        //    TipDim = h == 0 ? TipDimenzionisanja.Slobodno : TipDimenzionisanja.Vezano;
        //    if (KofZaProracunPravougaonogPreseka == null) SetKof();
        //    if (TipDim == TipDimenzionisanja.Slobodno) SlobodnoDim(); else VezanoDim();
        //}
       

        public abstract void VezanoDim();
        public abstract void SlobodnoDim();
        public abstract void SetKof();
        public abstract void Armiranje();
        public abstract void DvostrukoArmiranje();
        public abstract void sethSlobodnoDim();

        public virtual void InitValidations(double b, double h, IBetonModel beton, ReinforcementTypeModelEC armatura, double d1, double d2)
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

        public double Round(double d, int i = 2)
        {
            return Math.Round(d, i);
        }

        public void Dispose()
        {
            GC.Collect();
        }

        
    }

}

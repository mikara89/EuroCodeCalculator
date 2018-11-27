using TabeleEC2.Model;

namespace CalculatorEC2Logic.SavijanjePreseka
{
    public enum TipDimenzionisanjaV2 : int
    {
        Slobodno = 1,
        Vezano = 2,
    }
    public enum Standard
    {
        EuroCode,
        PBAB
    }
    public interface ISavijanje
    {
        void Start();
        double Round(double d, int i = 2);
        void InitValidations(double b, double h, IBetonModel beton, ReinforcementTypeModelEC armatura, double d1, double d2);
        void VezanoDim();
        void sethSlobodnoDim();
        void SlobodnoDim();
        void SetKof();
        void Armiranje();
        void DvostrukoArmiranje();
    }

}

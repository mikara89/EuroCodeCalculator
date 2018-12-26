namespace CalculatorEC2Logic.Transversal
{
    public interface IForcesTransversal
    {
        double Ved { get; set; }
        double Vg { get; set; } 
        double Vq { get; set; }
        double Ned { get; set; }
        double Ng { get; set; } 
        double Nq { get; set; }
    }
    public class ForcesTransversal : IForcesTransversal
    {
        private double _ved;
        private double _ned;

        public double Ved
        {
            get
            {
                if (_ved == 0)
                    return 1.35 * Vg + 1.5 * Vg;
                return _ved;
            }
            set
            {
                _ved = value;
            }
        }
        public double Vg { get; set; }
        public double Vq { get; set; }
        public double Ned
        {
            get
            {
                if (_ned == 0)
                    return 1.35 * Ng + 1.5 * Nq;
                return _ned;
            }
            set
            {
                _ned = value;
            }
        }
        public double Ng { get; set; }
        public double Nq { get; set; }
    }
}

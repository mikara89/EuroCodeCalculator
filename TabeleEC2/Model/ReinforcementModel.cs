using System.Linq;

namespace TabeleEC2.Model
{
    public interface IReinforcementModel
    {
        int Ø { get; set; }
        int Number { get; set; }
        double weight_total { get; }
        double cm2_total { get; }
    }
    public class ReinforcementModelEC: IReinforcementModel
    {
        public int Ø { get; set; }
        public int Number { get; set; }
        /// <summary>
        /// Set as First B500B
        /// </summary>
        public ReinforcementTypeModelEC Type { get; set; }
        public double weight_total { get {return armatura.weight_per_meter * Number; } } 
        public double cm2_total { get { return armatura.cm2 * Number; } } 
        public ReinforcementTabelModel armatura { get; set; }

        public ReinforcementModelEC(int Ø, int Number, ReinforcementTypeModelEC Type = null)
        {
            this.Ø = Ø;
            armatura = ReinforcementType.GetAramturaList().Single(n => n.diameter == Ø);
            this.Number = Number;
            if(Type==null)
                this.Type = ReinforcementType.GetArmatura().First();
        }
        public ReinforcementModelEC(ReinforcementTabelModel armatura, int Number, ReinforcementTypeModelEC Type = null) 
        {
            this.Ø = armatura.diameter;
            armatura = ReinforcementType.GetAramturaList().Single(n => n.diameter == Ø);
            
            this.armatura = armatura;
            this.Number = Number;
            if (Type == null)
                this.Type = ReinforcementType.GetArmatura().First();
        }
    }
    public class ReinforcementModelPBAB : IReinforcementModel
    {
        public int Ø { get; set; }
        public int Number { get; set; }
        /// <summary>
        /// Set as First B500B
        /// </summary>
        public ReinforcementTypeModelEC Type { get; set; }
        public double weight_total { get { return armatura.weight_per_meter * Number; } }
        public double cm2_total { get { return armatura.cm2 * Number; } }
        public ReinforcementTabelModel armatura { get; set; }

        public ReinforcementModelPBAB(int Ø, int Number)
        {
            this.Ø = Ø;
            armatura = ReinforcementType.GetAramturaList().Single(n => n.diameter == Ø);
            this.Number = Number;
            this.Type = ReinforcementType.GetArmatura().First();
        }
        public ReinforcementModelPBAB(ReinforcementTabelModel armatura, int Number)
        {
            this.Ø = armatura.diameter;
            this.armatura = armatura;
            this.Number = Number;
            this.Type = ReinforcementType.GetArmatura().First();
        }
    }
}

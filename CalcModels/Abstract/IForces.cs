namespace CalcModels
{

        public interface IForces
        { 
            double N { get; set; }
            double M1 { get; set; }
            double M2 { get; set; }
            double M3 { get; set; }
            double T2 { get; set; } 
            double T3 { get; set; }
        }
        public class Forces: IForces
        {
            public double N { get; set; }
            public double M1 { get; set; }
            public double M2 { get; set; }
            public double M3 { get; set; }
            public double T2 { get; set; }
            public double T3 { get; set; }
        }
    
}




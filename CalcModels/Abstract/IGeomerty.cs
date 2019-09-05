namespace CalcModels
{

        public interface IGeomerty
        {
            double A1 { get; set; }
            double A2 { get; set; }
            double h { get; }
            double a1 { get; set; }
            double a2 { get; set; }
            double d { get; set; }
            double b { get; set; }
            double B { get; set; } 
            double dp { get; set; }
        }
        public class Geomerty: IGeomerty
        {
            public double A1 { get; set; }
            public double A2 { get; set; }
            public double h { get => d - a1; } 
            public double d { get; set; }
            public double a1 { get; set; }
            public double a2 { get; set; }
            public double b { get; set; } 
            public double B { get; set; }
            public double dp { get; set; }
            public override string ToString()
            {
                return $@"Geometry:
    {nameof(A1)}:{A1}
    {nameof(A2)}:{A2}
    {nameof(d)}:{d}
    {nameof(b)}:{b}
    {nameof(B)}:{B}
    {nameof(h)}:{h}
    {nameof(a1)}:{a1}
    {nameof(a2)}:{a2}
    {nameof(dp)}:{dp}
    ";
            }
        }
       
    
}




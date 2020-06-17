using System;

namespace SectionDesign
{
    public class Point:IComparable
    {
        public double X { get; set; }
        public double Y { get; set; }
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            var other = obj as Point;

            if (other != null)
                if (this.X.CompareTo(other.X) == 0 && this.Y.CompareTo(other.Y) == 0)
                {
                    return 0;
                }
                else return 1;
            else
                throw new ArgumentException("Object is not a Point");
        }
        public override string ToString()
        {
            return $"{nameof(X)}={X}/{nameof(Y)}={Y}";
        }
    }
}

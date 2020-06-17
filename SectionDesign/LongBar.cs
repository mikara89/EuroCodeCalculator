using System;

namespace SectionDesign
{
    public class LongBar :Point, IComparable
    {
        /// <summary>
        /// In [mm]
        /// </summary>
        public int diametar { get; set; }
        public bool IsLinked { get; set; }

        public override string ToString()
        {
            return $"{nameof(X)}={X}/{nameof(Y)}={Y}; fi={diametar}";
        }
    }
}

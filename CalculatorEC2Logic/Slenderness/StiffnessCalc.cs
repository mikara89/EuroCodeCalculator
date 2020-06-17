using CalcModels;
using System;
using System.Collections.Generic;

namespace CalculatorEC2Logic
{
    public class StiffnessCalc
    {
        public StiffnessCalc()
        {

        }

        //public double CalcBraced(List<Tuple<IElementGeometryStiffness, IBetonModel>> list)
        //{
             
        //}
         private double calcEIperL(Tuple<IElementGeometryStiffness, IBetonModel> element)
        {
            return element.Item2.Ecm / 10000 * element.Item1.I / element.Item1.L;
        }
        private double k1(List<Tuple<IElementGeometryStiffness, IBetonModel>> list)
        {
            double result = 0.0;
            var column = list.Find(x => x.Item1.Lvl == NodeStiffness.Column);
            foreach (var item in list)
            {
                if (item.Item1.Lvl == NodeStiffness.Story1)
                {
                    result +=calcEIperL(item);
                }
            }
            return calcEIperL(column)/result<0.1?0.1: calcEIperL(column) / result; 
        }
        private double k2(List<Tuple<IElementGeometryStiffness, IBetonModel>> list)
        {
            double result = 0.0;
            var column = list.Find(x => x.Item1.Lvl == NodeStiffness.Column);
            foreach (var item in list)
            {
                if (item.Item1.Lvl == NodeStiffness.Story2)
                {
                    result += calcEIperL(item);
                }
            }
            return calcEIperL(column) / result < 0.1 ? 0.1 : calcEIperL(column) / result;
        }

    }

}

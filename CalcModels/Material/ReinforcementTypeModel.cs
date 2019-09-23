using CalcModels;
using System;
using System.Collections.Generic;

namespace CalcModels
{

    public class ReinforcementTypeModelEC : IReinforcementTypeModel
    {
        public string name { get; set; }
        /// <summary>
        /// fyd=fyk/1,15 kN/cm2 =>fyd*10[Mpa]
        /// </summary>
        public double fyd { get; set; }

        public double fyk { get; set; }
        /// <summary>
        /// in GPa
        /// </summary>
        public int Es { get; set; }
        public string Symbol { get; set; }
        public List<ReinforcementTabelModel> ListOfArmatura
        {
            get
            {
                return ReinforcementType.GetAramturaList();
            }
        }
        public List<int> ListOfNum
        {
            get
            {
                var list = new List<int>();
                for (int i = 1; i < 16; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        public double eps_ud { get; internal set; }

        public double lim_ξ { get; set; } = 0.45;

        public override string ToString()
        {
            return $"{name}; fyd: {Math.Round(fyd, 2)}MPa; fyk: {Math.Round(fyk, 2)}MPa; Es: {Es}GPa";
        }
    }
    public class ReinforcementTypeModelPBAB : IReinforcementTypeModel
    {


        public string name { get; set; }
        /// <summary> 
        /// fyd=fyk/1,15 kN/cm2 =>fyd*10[Mpa]
        /// </summary>
        public double fyd{ get; set; }

        public double fyk { get; set; }
        /// <summary>
        /// in GPa
        /// </summary>
        public int Es { get; set; }
        public string Symbol { get; set; }
        public List<ReinforcementTabelModel> ListOfArmatura
        {
            get
            {
                return ReinforcementType.GetAramturaList();
            }
        }
        public List<int> ListOfNum
        {
            get
            {
                var list = new List<int>();
                for (int i = 1; i < 16; i++)
                {
                    list.Add(i);
                }
                return list;
            }
        }

        public double eps_ud { get; internal set; }

        public double lim_ξ { get; set; } = 0.259;
    }
}

using System;
using System.Collections.Generic;

namespace TabeleEC2.Model
{
    public interface IReinforcementTypeModel
    {
        string name { get; set; }
        int Es { get; set; }
        string Symbol { get; set; }
        List<ReinforcementTabelModel> ListOfArmatura{get;}
        List<int> ListOfNum{get;}
    }
    public class ReinforcementTypeModelEC: IReinforcementTypeModel
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
        public string Symbol { get;  set; }
        public List<ReinforcementTabelModel> ListOfArmatura { get
            {
                return ReinforcementType.GetAramturaList();
            } }
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
        public override string ToString()
        {
            return $"{name}; fyd: {Math.Round(fyd,2)}MPa; fyk: {Math.Round(fyk,2)}MPa; Es: {Es}GPa";
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
    }
}

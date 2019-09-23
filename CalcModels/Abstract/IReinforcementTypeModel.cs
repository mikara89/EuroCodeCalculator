using System.Collections.Generic;

namespace CalcModels
{
    public interface IReinforcementTypeModel
    {
        double eps_ud { get; }
        int Es { get; set; }
        double fyd { get; set; }
        double fyk { get; set; }
        double lim_ξ { get; set; }
        List<ReinforcementTabelModel> ListOfArmatura { get; }
        List<int> ListOfNum { get; }
        string name { get; set; }
        string Symbol { get; set; }

        string ToString();
    }
}
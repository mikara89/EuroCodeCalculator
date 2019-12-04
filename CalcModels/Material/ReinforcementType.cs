
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CalcModels 
{

    public static class ReinforcementType
    {
        /// <summary>
        /// fyd i fyk u [kN/cm2]
        /// </summary>
        /// <returns></returns>
        public static List<IReinforcementTypeModel> GetArmatura()
        {
            return new List<IReinforcementTypeModel>
            {
                new ReinforcementTypeModelEC
                {
                    name="B500B",
                    fyk=50,
                    fyd=50/1.15,
                    Es=200,
                    Symbol="B",
                    eps_ud=20,
                },
                new ReinforcementTypeModelEC
                {
                    name="B460B",
                    fyk=46,
                    fyd=46/1.15,
                    Es=200,
                    Symbol="B",
                    eps_ud=20,
                },
                new ReinforcementTypeModelEC
                {
                    name="B420B",
                    fyk=42,
                    fyd=42/1.15,
                    Es=200,
                    Symbol="B",
                    eps_ud=20,
                },
                new ReinforcementTypeModelEC
                {
                    name = "B250B",
                    fyk = 25,
                    fyd = 25 / 1.15,
                    Es = 200,
                    Symbol = "B",
                    eps_ud=20,
                },
                new ReinforcementTypeModelEC
                {
                    name = "B220B",
                    fyk = 22,
                    fyd = 22 / 1.15,
                    Es = 200,
                    Symbol = "B",
                    eps_ud=20,
                },
                new ReinforcementTypeModelPBAB
                {
                    name = "RA 400/500",
                    fyk = 50,
                    fyd =40,
                    Es = 210,
                    Symbol = "R",
                    eps_ud=10,
                    lim_ξ=0.538
                }
            };
        }


        public static List<ReinforcementTabelModel> GetAramturaList()
        {
            try
            {
                List<ReinforcementTabelModel> items;
                items = JsonConvert.DeserializeObject<List<ReinforcementTabelModel>>(armTabJson);
                return items;
            }
            catch (Exception)
            {

                throw;
            }


        }

        private static string armTabJson = @"[
 {
   'diameter': 6,
   'weight_per_meter': 0.222,
   'Opseg': 1.89,
   'cm2': 0.28
 },
 {
   'diameter': 8,
   'weight_per_meter': 0.395,
   'Opseg': 2.51,
   'cm2': 0.5
 },
 {
   'diameter': 10,
   'weight_per_meter': 0.617,
   'Opseg': 3.14,
   'cm2': 0.79
 },
 {
   'diameter': 12,
   'weight_per_meter': 0.888,
   'Opseg': 3.77,
   'cm2': 1.13
 },
 {
   'diameter': 14,
   'weight_per_meter': 1.208,
   'Opseg': 4.4,
   'cm2': 1.54
 },
 {
   'diameter': 16,
   'weight_per_meter': 1.578,
   'Opseg': 5.03,
   'cm2': 2.01
 },
 {
   'diameter': 18,
   'weight_per_meter': 1.998,
   'Opseg': 5.65,
   'cm2': 2.54
 },
 {
   'diameter': 20,
   'weight_per_meter': 2.466,
   'Opseg': 6.28,
   'cm2': 3.14
 },
 {
   'diameter': 22,
   'weight_per_meter': 2.984,
   'Opseg': 6.91,
   'cm2': 3.8
 },
 {
   'diameter': 25,
   'weight_per_meter': 3.951,
   'Opseg': 7.85,
   'cm2': 4.91
 },
 {
   'diameter': 28,
   'weight_per_meter': 4.834,
   'Opseg': 8.8,
   'cm2': 6.15
 },
 {
   'diameter': 32,
   'weight_per_meter': 6.313,
   'Opseg': 10.05,
   'cm2': 8.04
 }
]";
    }
}

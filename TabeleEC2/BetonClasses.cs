using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TabeleEC2.Model;

namespace TabeleEC2
{
    public static class BetonClasses
    {
        public static List<string> GetBetonClassNamesEC()
        {
            List<BetonModelEC> items;
            items = JsonConvert.DeserializeObject<List<BetonModelEC>>(betonJson);
            return items.Select(n=>n.name).ToList();
        }
        public static List<BetonModelEC> GetBetonClassListEC() 
        {
            try
            {
                List<BetonModelEC> items;
                items = JsonConvert.DeserializeObject<List<BetonModelEC>>(betonJson);
                return items;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static List<string> GetBetonClassNamesPBAB()
        {
            List<BetonModelPBAB> items;
            items = JsonConvert.DeserializeObject<List<BetonModelPBAB>>(markaBetonaPBAB);
            return items.Select(n => n.name).ToList();
        }
        public static List<BetonModelPBAB> GetBetonClassListPBAB()
        {
            try
            {
                List<BetonModelPBAB> items;
                items = JsonConvert.DeserializeObject<List<BetonModelPBAB>>(markaBetonaPBAB);
                return items;
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static string betonJson = @"
[
 {
   'name': 'C12/15',
   'fck': 12,
   'fck_cube': 15,
   'fcm': 20,
   'fctm': 1.6,
   'fctk005': 1.1,
   'fctk095': 2,
   'Ecm': 27,
   'εc1': 1.8,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0007
 },
 {
   'name': 'C16/20',
   'fck': 16,
   'fck_cube': 20,
   'fcm': 24,
   'fctm': 1.9,
   'fctk005': 1.3,
   'fctk095': 2.5,
   'Ecm': 29,
   'εc1': 1.9,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0007
 },
 {
   'name': 'C20/25',
   'fck': 20,
   'fck_cube': 25,
   'fcm': 28,
   'fctm': 2.2,
   'fctk005': 1.5,
   'fctk095': 2.9,
   'Ecm': 30,
   'εc1': 2,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0007
 },
 {
   'name': 'C25/30',
   'fck': 25,
   'fck_cube': 30,
   'fcm': 33,
   'fctm': 2.6,
   'fctk005': 1.8,
   'fctk095': 3.3,
   'Ecm': 31,
   'εc1': 2.1,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0011
 },
 {
   'name': 'C30/37',
   'fck': 30,
   'fck_cube': 37,
   'fcm': 38,
   'fctm': 2.9,
   'fctk005': 2,
   'fctk095': 3.8,
   'Ecm': 33,
   'εc1': 2.2,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0011
 },
 {
   'name': 'C35/45',
   'fck': 35,
   'fck_cube': 45,
   'fcm': 43,
   'fctm': 3.2,
   'fctk005': 2.2,
   'fctk095': 4.2,
   'Ecm': 34,
   'εc1': 2.25,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0011
 },
 {
   'name': 'C40/50',
   'fck': 40,
   'fck_cube': 50,
   'fcm': 48,
   'fctm': 3.5,
   'fctk005': 2.5,
   'fctk095': 4.6,
   'Ecm': 35,
   'εc1': 2.3,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0013
 },
 {
   'name': 'C45/55',
   'fck': 45,
   'fck_cube': 55,
   'fcm': 53,
   'fctm': 3.8,
   'fctk005': 2.7,
   'fctk095': 4.9,
   'Ecm': 36,
   'εc1': 2.4,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0013
 },
 {
   'name': 'C50/60',
   'fck': 50,
   'fck_cube': 60,
   'fcm': 58,
   'fctm': 4.1,
   'fctk005': 2.9,
   'fctk095': 5.3,
   'Ecm': 37,
   'εc1': 2.45,
   'εcu1': 3.5,
   'εc2': 2,
   'εcu2': 3.5,
   'n': 2,
   'εc3': 1.75,
   'εcu3': 3.5,
   'ρ': 0.0013
 }

]";
        private static string markaBetonaPBAB = @"{
  'fbk': 10
},
{
  'fbk': 15
},
{
  'fbk': 20
},
{
  'fbk': 25
},
{
  'fbk': 30
},
{
  'fbk': 35
},
{
  'fbk': 40
},
{
  'fbk': 45
},
{
  'fbk': 50
},
{
  'fbk': 55
},
{
  'fbk': 60
},
";

    }



}
 //{
 //  'name': 'C55/67',
 //  'fck': 55,
 //  'fck_cube': 67,
 //  'fcm': 63,
 //  'fctm': 4.2,
 //  'fctk005': 3,
 //  'fctk095': 5.5,
 //  'Ecm': 38,
 //  'εc1': 2.5,
 //  'εcu1': 3.2,
 //  'εc2': 2.2,
 //  'εcu2': 3.1,
 //  'n': 1.75,
 //  'εc3': 1.8,
 //  'εcu3': 3.1,
 //  'ρ': null
 //},
 //{
 //  'name': 'C60/75',
 //  'fck': 60,
 //  'fck_cube': 75,
 //  'fcm': 68,
 //  'fctm': 4.4,
 //  'fctk005': 3.1,
 //  'fctk095': 5.7,
 //  'Ecm': 39,
 //  'εc1': 2.6,
 //  'εcu1': 3,
 //  'εc2': 2.3,
 //  'εcu2': 2.9,
 //  'n': 1.6,
 //  'εc3': 1.9,
 //  'εcu3': 2.9,
 //  'ρ': null
 //},
 //{
 //  'name': 'C70/85',
 //  'fck': 70,
 //  'fck_cube': 85,
 //  'fcm': 78,
 //  'fctm': 4.6,
 //  'fctk005': 3.2,
 //  'fctk095': 6,
 //  'Ecm': 41,
 //  'εc1': 2.7,
 //  'εcu1': 2.8,
 //  'εc2': 2.4,
 //  'εcu2': 2.7,
 //  'n': 1.45,
 //  'εc3': 2,
 //  'εcu3': 2.7,
 //  'ρ': null
 //},
 //{
 //  'name': 'C80/95',
 //  'fck': 80,
 //  'fck_cube': 95,
 //  'fcm': 88,
 //  'fctm': 4.8,
 //  'fctk005': 3.4,
 //  'fctk095': 6.3,
 //  'Ecm': 42,
 //  'εc1': 2.8,
 //  'εcu1': 2.8,
 //  'εc2': 2.5,
 //  'εcu2': 2.6,
 //  'n': 1.4,
 //  'εc3': 2.2,
 //  'εcu3': 2.6,
 //  'ρ': null
 //},
 //{
 //  'name': 'C90/105',
 //  'fck': 90,
 //  'fck_cube': 105,
 //  'fcm': 98,
 //  'fctm': 5,
 //  'fctk005': 3.5,
 //  'fctk095': 6.6,
 //  'Ecm': 44,
 //  'εc1': 2.8,
 //  'εcu1': 2.8,
 //  'εc2': 2.6,
 //  'εcu2': 2.6,
 //  'n': 1.4,
 //  'εc3': 2.3,
 //  'εcu3': 2.6,
 //  'ρ': null
 //}
using System;
using System.Collections.Generic;

namespace CalcModels
{
    public class BetonService : IBetonService
    {
        public IBetonModel GetNew(string name)
        {
            if (name.Contains("MB"))
                return GetPBABA(name);
            else if (name.Contains("C"))
                return GetEC(name);
            else return null;
        }
        public IBetonModel[] GetList()
        {
            var r = new List<IBetonModel>();
            foreach (var item in Enum.GetNames(typeof(BetonClassTypeEC)))
            {
                var i = item.Replace("_", "/");
                r.Add(new BetonModelEC(i));
            }
            foreach (var item in Enum.GetNames(typeof(BetonClassTypePBAB)))
            {
                r.Add(new BetonModelEC(item));
            }
            return r.ToArray();
        }
        public string[] GetNameList()
        {
            var r = new List<string>();
            foreach (var item in Enum.GetNames(typeof(BetonClassTypeEC)))
            {
                var i = item.Replace("_", "/");
                r.Add(i);
            }
            foreach (var item in Enum.GetNames(typeof(BetonClassTypePBAB)))
            {
                r.Add(item);
            }
            return r.ToArray();
        }
        private IBetonModel GetEC(string name)
        {
            return new BetonModelEC(name);
        }
        private IBetonModel GetPBABA(string name)
        {
            return new BetonModelPBAB(name);
        }
    }
}

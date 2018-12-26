using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VGGS_Calculator.Core.Models;

namespace VGGS_Calculator.Core
{
    public interface ITransverzalSilaEc2Repository
    {
        TransverzalneSileEc2ResultModel CalculateInit(TransverzalneSileEc2Model trans);
    }
}

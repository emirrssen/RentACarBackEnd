using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.AspectResults
{
    public class AspectRules
    {
        public static IResult Check(params IResult[] aspects)
        {
            foreach (var aspect in aspects)
            {
                if (!aspect.Success)
                {
                    return aspect;
                }
            }

            return null;
        }
    }
}

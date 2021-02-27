using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HomeBudgetManagement.Domain
{
    public static class Extensions
    {
        public static double CustomSum<T>(this IEnumerable<T> selector)
        {
            double retval = default;

            var numbers = selector.Select(e => Convert.ToDouble(e));

            foreach (double item in numbers)
            {
                retval += item;
            }

            return retval;
        }
    }
}
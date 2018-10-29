using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWCF.Services
{
    public static class DbSetExtensions
    {
        public static bool In<T>(this IEnumerable<T> ts, T find)
        {
            foreach (var obj in ts)
                if (find.Equals(obj))
                    return true;
            return false;
        }
    }
}

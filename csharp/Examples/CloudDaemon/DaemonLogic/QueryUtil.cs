using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DaemonLogic
{
    public class QueryUtil
    {
        public static T First<T>(IQueryable<T> queryable)
        {
            if (queryable.Count<T>() == 0)
            {
                return default(T);
            }
            else
            {
                return queryable.First<T>();
            }
        }

    }
}

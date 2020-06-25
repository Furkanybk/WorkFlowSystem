using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WFS.web.Services
{
    public class ClearCache
    {
        public static void clear()
        {
            IDictionaryEnumerator enumerator = HttpContext.Current.Cache.GetEnumerator();

            while (enumerator.MoveNext())
            {

                HttpContext.Current.Cache.Remove((string)enumerator.Key);

            }
        }
    }
}
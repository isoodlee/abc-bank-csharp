using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace abc_bank
{
    //public class DateProvider
    //{
    //    private static DateProvider instance = null;

    //    public static DateProvider getInstance()
    //    {
    //        if (instance == null)
    //            instance = new DateProvider();
    //        return instance;
    //    }

    //    public DateTime Now()
    //    {
    //        return DateTime.Now;
    //    }
    //}

    public sealed class DateProvider
    {
        private DateProvider()
        {

        }

        public static DateProvider getInstance
        {
            get { return NestedDate.Instance; }
        }

        private class NestedDate
        {
            static NestedDate()
            {

            }

            internal static readonly DateProvider Instance = new DateProvider();
        }
        public DateTime Now()
        {
            return DateTime.Now;
        }
    }
}

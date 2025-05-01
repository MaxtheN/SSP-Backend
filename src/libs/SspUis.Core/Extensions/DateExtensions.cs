using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SspUis
{
    public static class DateExtensions
    {
        public static DateOnly AsDateOnly(this DateTime dateTime)
        {
            return DateOnly.FromDateTime(dateTime);
        }

        public static DateTime AsDateTime(this DateOnly dateOnly)
        {
            return dateOnly.ToDateTime(new TimeOnly());
        }


    }
}

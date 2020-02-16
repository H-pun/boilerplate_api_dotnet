using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public class DateHelper
    {
        public static int GetMonthDifference(DateTime startDate, DateTime endDate)
        {
            int monthsApart = 12 * (startDate.Year - endDate.Year) + startDate.Month - endDate.Month;
            return Math.Abs(monthsApart);
        }
    }
}

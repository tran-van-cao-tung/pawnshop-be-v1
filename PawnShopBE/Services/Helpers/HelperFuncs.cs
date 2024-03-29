﻿using PawnShopBE.Core.Const;
using PawnShopBE.Core.Interfaces;
using PawnShopBE.Infrastructure.Helpers;

namespace PawnShopBE.Helpers
{
    public class HelperFuncs
    {    
        public static List<Tuple<DateTime, DateTime>> DivideTimePeriodIntoPeriods(DateTime startDate, DateTime endDate, int numberOfPeriods)
        {
            // Calculate the total time span between the start and end dates
            TimeSpan totalTimeSpan = endDate - startDate;

            // Calculate the number of days in each period
            int daysPerPeriod = (int)Math.Ceiling(totalTimeSpan.TotalDays / numberOfPeriods);

            // Calculate the start and end dates for each period and store them in a list
            List<Tuple<DateTime, DateTime>> periods = new List<Tuple<DateTime, DateTime>>();

            for (int i = 0; i < numberOfPeriods; i++)
            {
                DateTime periodStartDate = startDate.AddDays(i * daysPerPeriod);
                DateTime periodEndDate = periodStartDate.AddDays(daysPerPeriod - 1);

                // Make sure the end date of the last period doesn't exceed the overall end date
                if (periodEndDate > endDate)
                {
                    periodEndDate = endDate;
                }

                periods.Add(new Tuple<DateTime, DateTime>(periodStartDate, periodEndDate));
            }

            return periods;
        }
    }
}

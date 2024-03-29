﻿namespace PetStore.Common
{
    using System;

    public struct SpawnTimeCalculator
    {
        public SpawnTimeCalculator(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            this.Years = years;
            this.Months = months;
            this.Days = days;
            this.Hours = hours;
            this.Minutes = minutes;
            this.Seconds = seconds;
            this.Milliseconds = milliseconds;
        }

        private enum Phase
        {
            Years,
            Months,
            Days,
            Done,
        }

        public int Years { get; }

        public int Months { get; }

        public int Days { get; }

        public int Hours { get; }

        public int Minutes { get; }

        public int Seconds { get; }

        public int Milliseconds { get; }

        public static SpawnTimeCalculator CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            SpawnTimeCalculator span = new SpawnTimeCalculator();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }

                        break;

                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                            {
                                current = current.AddDays(officialDay - current.Day);
                            }
                        }
                        else
                        {
                            months++;
                        }

                        break;

                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new SpawnTimeCalculator(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }

                        break;
                }
            }

            return span;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelper.Models
{
    class Budget
    {
        [SQLite.PrimaryKey]
        public int Id { get; set; }
        public DateTime DateCreated { get; set;}
        public string Name { get; set; }
        public float PaycheckAmount { get; set; }
        public PaycheckFrequency Frequency { get; set; }
    }

    public enum PaycheckFrequency
    {
        Monthly = 1, SemiMonthly = 2, BiWeekly
    }

    public static class Extensions
    {
        public static float MonthlyAmount(this PaycheckFrequency frequency, float paycheckAmount)
        {
            if (frequency != PaycheckFrequency.BiWeekly)
            {
                return ((float) frequency) * paycheckAmount;
            }
            else
            {
                float annual = paycheckAmount * 26.0f;
                return annual / 12.0f;
            }
        }
    }
}

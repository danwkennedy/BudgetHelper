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
        public float MonthlyTakeHome { get; set; }
        public PaycheckFrequency Frequency { get; set; }
    }

    public enum PaycheckFrequency
    {
        Monthly, SemiMonthly, BiWeekly
    }
}

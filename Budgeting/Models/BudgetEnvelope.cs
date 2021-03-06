﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetHelper.Models
{

    class BudgetEnvelope
    {
        [PrimaryKey, AutoIncrement, Unique]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PercentageOfBudget { get; set; }
        public string BudgetId { get; set; }
    }
}

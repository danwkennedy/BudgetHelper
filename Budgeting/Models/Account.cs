using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetHelper.Models
{
    class Account
    {
        [PrimaryKey, AutoIncrementAttribute, Unique]
        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public AccountType Type { get; set; }
        public string Description { get; set; }
        public float Amount { get; set; }
        public bool IsRecurrent { get; set; }
        public string RecurrenceRule { get; set; }
    }

    public enum AccountType
    {
        Checking, Savings
    }

}

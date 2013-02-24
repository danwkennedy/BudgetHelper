using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetHelper.Models
{
    class Transaction : Model
    {

        //[PrimaryKey, AutoIncrement, Unique]
        //public int Id { get; set; }
        public TransactionType Type { get; set; }
        public double Amount { get; set; }
        public int SourceAccount { get; set; }
        public int DestinationAccount { get; set; }
        public string Notes { get; set; }
        public bool IsRecurrent { get; set; }
        public string RecurrenceRule { get; set; }
    }

    public enum TransactionType
    {
        Debit, Credit
    }
}

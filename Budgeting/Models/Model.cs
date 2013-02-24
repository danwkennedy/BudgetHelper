using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace BudgetHelper.Models
{
    public class Model
    {
        
        [PrimaryKey, AutoIncrement, Unique]
        public int Id
        {
            get;
            set;
        }

        public Model() { }
    }
}

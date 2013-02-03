using BudgetHelper.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace BudgetHelper.ViewModels
{
    public class BudgetViewModel : ViewModel
    {

        #region Properties

        public int Id { get; set; }
        public DateTime DateCreated { get; set; }
        public string Name { get; set; }
        public float MonthlyTakeHome { get; set; }
        public PaycheckFrequency Frequency { get; set; }

        public string Title
        {
            get
            {
                return Name;
            }
        }

        public string Subtitle
        {
            get
            {
                return "This is the Subtitle";
            }
        }

        #endregion

        private App app = (Application.Current as App);

        public BudgetViewModel getBudget(int budgetId)
        {
            BudgetViewModel budget = new BudgetViewModel();

            using (var db = new SQLite.SQLiteConnection(app.DBPath))
            {
                Budget _budget = db.Table<Budget>().Where(b => b.Id == budgetId).Single();
                budget.Id = _budget.Id;
                budget.Name = _budget.Name;
                budget.DateCreated = _budget.DateCreated;
                budget.MonthlyTakeHome = _budget.PaycheckAmount;
                budget.Frequency = _budget.Frequency;
            }

            return budget;
        }

        public ObservableCollection<BudgetViewModel> getBudgets()
        {
            ObservableCollection<BudgetViewModel> budgets = new ObservableCollection<BudgetViewModel>();

            using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(app.DBPath))
            {
                var results = db.Table<Budget>().OrderBy(b => b.Name);

                foreach (Budget _budget in results)
                {
                    BudgetViewModel budget = new BudgetViewModel();
                    budget.Id = _budget.Id;
                    budget.Name = _budget.Name;
                    budget.DateCreated = _budget.DateCreated;
                    budget.MonthlyTakeHome = _budget.PaycheckAmount;
                    budget.Frequency = _budget.Frequency;

                    budgets.Add(budget);
                }
            }

            return budgets;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetHelper.ViewModels
{
    public abstract class ViewModel
    {

        public ViewModel find(int modelId)
        {
            return null;
        }

        public bool save(ViewModel viewModel)
        {
            return false;
        }

        protected bool insert(ViewModel viewModel)
        {
            return false;
        }

        protected bool update(ViewModel viewModel)
        {
            return false;
        }

    }
}

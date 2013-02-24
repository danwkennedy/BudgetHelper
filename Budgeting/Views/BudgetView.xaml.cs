using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BudgetHelper.ViewModels;
using BudgetHelper.Models;
using System.Collections.ObjectModel;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace BudgetHelper.Views
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class BudgetView : BudgetHelper.Common.LayoutAwarePage
    {

        //protected BudgetViewModel Budget { get; set; }

        public BudgetView()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            BudgetViewModel budgets = new BudgetViewModel();
            BudgetViewModel Budget = budgets.getBudgets()[0];

            ObservableCollection<ListItem> generalList = new ObservableCollection<ListItem>();
            generalList.Add(new ListItem("Paycheck Amount", String.Format("{0:C2}", Budget.MonthlyTakeHome)));
            generalList.Add(new ListItem("Frequency", Budget.Frequency.ToString()));
            generalList.Add(new ListItem("Take Home", String.Format("{0:C2} per month", Budget.Frequency.MonthlyAmount(Budget.MonthlyTakeHome))));

            GeneralInfoList.Items.Add(new ListItem("Paycheck Amount", String.Format("{0:C2}", Budget.MonthlyTakeHome)));
            GeneralInfoList.Items.Add(new ListItem("Frequency", Budget.Frequency.ToString()));
            GeneralInfoList.Items.Add(new ListItem("Take Home", String.Format("{0:C2} per month", Budget.Frequency.MonthlyAmount(Budget.MonthlyTakeHome))));

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class ListItem
    {
        public string Label { get; set; }
        public string Value { get; set; }

        public ListItem(string label, string value)
        {
            Label = label;
            Value = value;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BudgetHelper.ViewModels;

// The Items Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234233

namespace BudgetHelper.Views
{
    /// <summary>
    /// A page that displays a collection of item previews.  In the Split Application this page
    /// is used to display and select one of the available groups.
    /// </summary>
    public sealed partial class MainPage : BudgetHelper.Common.LayoutAwarePage
    {
        public MainPage()
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
            BudgetViewModel budgetViewModel = new BudgetViewModel();

            ObservableCollection<NavigationItem> collection = new ObservableCollection<NavigationItem>();
            Object obj = typeof(BudgetView);
            collection.Add(new NavigationItem("Budget", typeof(BudgetView)));
            collection.Add(new NavigationItem("Accounts", typeof(AccountsView)));
            collection.Add(new NavigationItem("Add Revenue", typeof(AddRevenue)));
            collection.Add(new NavigationItem("Add an Expense", typeof(AddExpense)));
            DefaultViewModel["Items"] = collection;
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                Frame.Navigate(((NavigationItem) e.ClickedItem).Target, e.ClickedItem);
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class NavigationItem
    {

        #region Properties

        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string Description { get; set; }
        public System.Type Target { get; set; }

        #endregion

        public NavigationItem(string title, System.Type target)
        {
            Title = title;
            Target = target;
        }
        
    }

}

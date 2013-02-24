using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using BudgetHelper.ViewModels;
using BudgetHelper.Models;

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

            List<ListItem> transactionTypes = new List<ListItem>();
            transactionTypes.Add(new ListItem(Models.TransactionType.Credit.ToString(), ((int)Models.TransactionType.Credit).ToString()));
            transactionTypes.Add(new ListItem(Models.TransactionType.Debit.ToString(), ((int)Models.TransactionType.Debit).ToString()));
            DefaultViewModel["TransactionTypes"] = transactionTypes;

            DefaultViewModel["Accounts"] = new List<ListItem>();
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            

            if (e.ClickedItem != null)
            {
                NavigationItem clickedItem = (NavigationItem)e.ClickedItem;

                if (clickedItem.Target == typeof(AddRevenue))
                {
                    DisplayTransactionPopup(Models.TransactionType.Credit);
                }
                else if (clickedItem.Target == typeof(AddExpense))
                {
                    DisplayTransactionPopup(Models.TransactionType.Debit);
                }
                else
                {
                    Frame.Navigate(((NavigationItem)e.ClickedItem).Target, e.ClickedItem);
                }
            }
        }

        private void DisplayTransactionPopup(Models.TransactionType type)
        {
            double width = pageRoot.ActualWidth;

            ((Grid)AddTransactionPopup.Child).Height = Window.Current.Bounds.Height;
            AddTransactionPopup.HorizontalOffset = Window.Current.Bounds.Width - AddTransactionPopup.ActualWidth;

            foreach (ListItem item in (List<ListItem>)DefaultViewModel["TransactionTypes"])
            {
                if (item.Label.Equals(type.ToString()))
                {
                    TransactionType.SelectedItem = item;
                }
            }

            if (((List<ListItem>)DefaultViewModel["Accounts"]).Count == 0)
            {
                List<Account> accounts = (new ViewModel<Account>()).FetchAll();

                List<ListItem> items = new List<ListItem>(accounts.Count);
                foreach (Account account in accounts)
                {
                    items.Add(new ListItem(account.Name, account.Id.ToString()));
                }

                DefaultViewModel["Accounts"] = items;
            }

            AddTransactionPopup.IsOpen = true;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        

        private void CancelRevenueButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddTransactionPopup.IsOpen)
            {
                AddTransactionPopup.IsOpen = false;
            }
        }

        private void SaveTransactionButton_Click(object sender, RoutedEventArgs e)
        {
            Transaction newTransaction = new Transaction();

            double amount;
            string amountString = TransactionAmount.Text.Replace("$", "").TrimStart();

            if (Double.TryParse(amountString, out amount))
            {
                newTransaction.Amount = amount;
            }

            string destinationString = ((ListItem)TransactionDestination.SelectedItem).Value;
            int destination = int.Parse(destinationString);

            newTransaction.DestinationAccount = destination;
            newTransaction.IsRecurrent = false;
            newTransaction.RecurrenceRule = null;
            newTransaction.Notes = TransactionNotes.Text;

            ViewModel<Transaction> transactionVM = new ViewModel<Transaction>();
            transactionVM.Insert(newTransaction);

            List<Transaction> transactions = transactionVM.FetchAll();
        }

        private void Amount_GotFocus(object sender, RoutedEventArgs e)
        {

            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;

                if (!String.IsNullOrWhiteSpace(box.Text))
                {
                    box.Text = box.Text.Replace("$", "").TrimStart();
                    box.SelectAll();
                }
            }            
        }

        private void Amount_LostFocus(object sender, RoutedEventArgs e)
        {

            if (sender is TextBox)
            {
                TextBox box = (TextBox)sender;
                double amount;

                if (Double.TryParse(box.Text, out amount))
                {
                    box.Text = Double.Parse(box.Text).ToString("C");
                }
                else
                {
                    box.Text = "";
                }
            }
            
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

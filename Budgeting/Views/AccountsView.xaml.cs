using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using BudgetHelper.ViewModels;
using BudgetHelper.Models;

// The Split Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234234

namespace BudgetHelper.Views
{
    /// <summary>
    /// A page that displays a group title, a list of items within the group, and details for
    /// the currently selected item.
    /// </summary>
    public sealed partial class AccountsView : BudgetHelper.Common.LayoutAwarePage
    {
        public AccountsView()
        {
            this.InitializeComponent();
        }

        #region Page state management

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
            // TODO: Assign a bindable group to this.DefaultViewModel["Group"]
            // TODO: Assign a collection of bindable items to this.DefaultViewModel["Items"]

            //AccountsGroup group = new AccountsGroup();
            //group.Title = "My Accounts";
            //this.DefaultViewModel["Group"] = group;

            //AccountViewModel accounts = new AccountViewModel();
            //this.DefaultViewModel["Items"] = accounts.getAccounts();
            InitializeGroups();

            List<ListItem> accountTypes = new List<ListItem>();
            accountTypes.Add(new ListItem(Models.AccountType.Checking.ToString(), ((int) Models.AccountType.Checking).ToString()));
            accountTypes.Add(new ListItem(Models.AccountType.Savings.ToString(), ((int)Models.AccountType.Savings).ToString()));

            this.DefaultViewModel["AccountTypes"] = accountTypes;

            //AccountType.Items.Add(Models.AccountType.Checking.ToString());
            //AccountType.Items.Add(Models.AccountType.Savings.ToString());

            if (pageState == null)
            {
                // When this is a new page, select the first item automatically unless logical page
                // navigation is being used (see the logical page navigation #region below.)
                if (!this.UsingLogicalPageNavigation() && this.itemsViewSource.View != null)
                {
                    this.itemsViewSource.View.MoveCurrentToFirst();
                }
            }
            else
            {
                // Restore the previously saved state associated with this page
                if (pageState.ContainsKey("SelectedItem") && this.itemsViewSource.View != null)
                {
                    // TODO: Invoke this.itemsViewSource.View.MoveCurrentTo() with the selected
                    //       item as specified by the value of pageState["SelectedItem"]
                }
            }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            if (this.itemsViewSource.View != null)
            {
                var selectedItem = this.itemsViewSource.View.CurrentItem;
                // TODO: Derive a serializable navigation parameter and assign it to
                //       pageState["SelectedItem"]
            }
        }

        private void InitializeGroups()
        {
            AccountsGroup group = new AccountsGroup();
            group.Title = "My Accounts";
            this.DefaultViewModel["Group"] = group;

            InitializeAccountItems();
        }

        private void InitializeAccountItems()
        {
            ViewModel<Account> accountVM = new ViewModel<Account>();
            List<Account> accounts = accountVM.FetchAll();
            List<AccountsListItem> accountListItems = new List<AccountsListItem>(accounts.Count);

            foreach (Account account in accounts)
            {
                accountListItems.Add(new AccountsListItem(account));
            }

            this.DefaultViewModel["Items"] = accountListItems;
        }

        #endregion

        #region Logical page navigation

        // Visual state management typically reflects the four application view states directly
        // (full screen landscape and portrait plus snapped and filled views.)  The split page is
        // designed so that the snapped and portrait view states each have two distinct sub-states:
        // either the item list or the details are displayed, but not both at the same time.
        //
        // This is all implemented with a single physical page that can represent two logical
        // pages.  The code below achieves this goal without making the user aware of the
        // distinction.

        /// <summary>
        /// Invoked to determine whether the page should act as one logical page or two.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed, or null
        /// for the current view state.  This parameter is optional with null as the default
        /// value.</param>
        /// <returns>True when the view state in question is portrait or snapped, false
        /// otherwise.</returns>
        private bool UsingLogicalPageNavigation(ApplicationViewState? viewState = null)
        {
            if (viewState == null) viewState = ApplicationView.Value;
            return viewState == ApplicationViewState.FullScreenPortrait ||
                viewState == ApplicationViewState.Snapped;
        }

        /// <summary>
        /// Invoked when an item within the list is selected.
        /// </summary>
        /// <param name="sender">The GridView (or ListView when the application is Snapped)
        /// displaying the selected item.</param>
        /// <param name="e">Event data that describes how the selection was changed.</param>
        void ItemListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Invalidate the view state when logical page navigation is in effect, as a change
            // in selection may cause a corresponding change in the current logical page.  When
            // an item is selected this has the effect of changing from displaying the item list
            // to showing the selected item's details.  When the selection is cleared this has the
            // opposite effect.
            if (this.UsingLogicalPageNavigation()) this.InvalidateVisualState();            
        }

        /// <summary>
        /// Invoked when the page's back button is pressed.
        /// </summary>
        /// <param name="sender">The back button instance.</param>
        /// <param name="e">Event data that describes how the back button was clicked.</param>
        protected override void GoBack(object sender, RoutedEventArgs e)
        {
            if (this.UsingLogicalPageNavigation() && itemListView.SelectedItem != null)
            {
                // When logical page navigation is in effect and there's a selected item that
                // item's details are currently displayed.  Clearing the selection will return to
                // the item list.  From the user's point of view this is a logical backward
                // navigation.
                this.itemListView.SelectedItem = null;
            }
            else
            {
                // When logical page navigation is not in effect, or when there is no selected
                // item, use the default back button behavior.
                base.GoBack(sender, e);
            }
        }

        /// <summary>
        /// Invoked to determine the name of the visual state that corresponds to an application
        /// view state.
        /// </summary>
        /// <param name="viewState">The view state for which the question is being posed.</param>
        /// <returns>The name of the desired visual state.  This is the same as the name of the
        /// view state except when there is a selected item in portrait and snapped views where
        /// this additional logical page is represented by adding a suffix of _Detail.</returns>
        protected override string DetermineVisualState(ApplicationViewState viewState)
        {
            // Update the back button's enabled state when the view state changes
            var logicalPageBack = this.UsingLogicalPageNavigation(viewState) && this.itemListView.SelectedItem != null;
            var physicalPageBack = this.Frame != null && this.Frame.CanGoBack;
            this.DefaultViewModel["CanGoBack"] = logicalPageBack || physicalPageBack;

            // Determine visual states for landscape layouts based not on the view state, but
            // on the width of the window.  This page has one layout that is appropriate for
            // 1366 virtual pixels or wider, and another for narrower displays or when a snapped
            // application reduces the horizontal space available to less than 1366.
            if (viewState == ApplicationViewState.Filled ||
                viewState == ApplicationViewState.FullScreenLandscape)
            {
                var windowWidth = Window.Current.Bounds.Width;
                if (windowWidth >= 1366) return "FullScreenLandscapeOrWide";
                return "FilledOrNarrow";
            }

            // When in portrait or snapped start with the default visual state name, then add a
            // suffix when viewing details instead of the list
            var defaultStateName = base.DetermineVisualState(viewState);
            return logicalPageBack ? defaultStateName + "_Detail" : defaultStateName;
        }

        #endregion

        #region Button Events

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            double width = pageRoot.ActualWidth;

            ((Grid)AddAccountPopup.Child).Height = Window.Current.Bounds.Height;
            AddAccountPopup.HorizontalOffset = Window.Current.Bounds.Width - AddAccountPopup.ActualWidth;

            AddAccountPopup.IsOpen = true;
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (itemListView.SelectedItem != null)
            {
                AccountViewModel selectedAccount = (AccountViewModel)itemListView.SelectedItem;
                if (await selectedAccount.DeleteAccount(selectedAccount.Id))
                {
                    InitializeAccountItems();
                }
            }
        }

        private void AccountStartingAmount_GotFocus(object sender, RoutedEventArgs e)
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

        private void AccountStartingAmount_LostFocus(object sender, RoutedEventArgs e)
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

        private async void SaveRevenueButton_Click(object sender, RoutedEventArgs e)
        {
            AccountViewModel newAccount = new AccountViewModel();
            newAccount.Name = AccountName.Text;
            newAccount.Description = AccountNotes.Text;
            int accountTypeInt = Int32.Parse(((ListItem) AccountType.SelectedItem).Value);
            newAccount.Type = (Models.AccountType)accountTypeInt;

            string currentAmountString = AccountStartingAmount.Text.Replace("$", "").TrimStart();
            double amount;

            if (Double.TryParse(currentAmountString, out amount))
            {
                newAccount.CurrentBalance = amount;
            }

            if (newAccount.IsValid())
            {
                await newAccount.Save();
                AddAccountPopup.IsOpen = false;
                InitializeAccountItems();
            }            
        }

        private void CancelRevenueButton_Click(object sender, RoutedEventArgs e)
        {
            if (AddAccountPopup.IsOpen)
            {
                AddAccountPopup.IsOpen = false;
            }
        }

        #endregion

    }

    #region Helper classes

    public class AccountsGroup
    {
        public string Title { get; set; }
    }

    class AccountsListItem
    {
        private Account _account;

        public string Title { get; set; }
        public string Description { get; set; }
        public Account Account { get { return _account; } }

        public AccountsListItem(Account account)
        {
            Title = account.Name;
            Description = account.Description;
            _account = account;
        }
    }

    #endregion

}

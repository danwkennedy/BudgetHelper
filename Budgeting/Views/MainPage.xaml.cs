using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            //TestData[] testData = { new TestData("Test1", "Testing 1"), new TestData("Test2", "Testing 2") };
            //List<TestData> testData = new List<TestData>();
            //testData.Add(new TestData("Test1", "Testing 1"));
            //testData.Add(new TestData("Test2", "Testing 2"));
            //this.DefaultViewModel["Items"] = new ObservableCollection<TestData>(testData);
            //this.itemGridView.Items.Add("Create New Budget");
            //List<string> titles = new List<string>();
            //titles.Add("Test 1");
            //titles.Add("Test 2");

            //this.itemGridView.ItemsSource = titles;
            BudgetViewModel budgetViewModel = new BudgetViewModel();
            ObservableCollection<BudgetViewModel> collection = budgetViewModel.getBudgets();
            this.DefaultViewModel["Items"] = budgetViewModel.getBudgets();
        }

        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

    public class TestData
    {
        public TestData(string title, string description)
        {
            this.Title = title;
            this.Description = description;
        }

        public string Title { get; set; }
        public string Description { get; set; }
    }
}

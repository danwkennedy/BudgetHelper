using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetHelper.Models;
using Windows.UI.Xaml;
using System.Collections.ObjectModel;

namespace BudgetHelper.ViewModels
{
    class AccountViewModel
    {

        #region Properties

        protected int _id;
        protected DateTime _dateCreated;

        public int Id
        {
            get
            {
                return _id;
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return _dateCreated;
            }
        }

        public string Name { get; set; }
        public AccountType Type { get; set; }
        public string Description { get; set; }
        public double CurrentBalance { get; set; }

        public string Title
        {
            get
            {
                return this.Name;
            }
        }

        public AccountViewModel(int id, DateTime dateCreated)
        {
            _id = id;
            _dateCreated = dateCreated;
        }

        public AccountViewModel() { }

        #endregion

        #region Methods

        private App app = (Application.Current as App);

        public AccountViewModel getAccount(int accountId)
        {
            AccountViewModel account = new AccountViewModel();

            return account;
        }

        public ObservableCollection<AccountViewModel> getAccounts()
        {
            ObservableCollection<AccountViewModel> accounts = new ObservableCollection<AccountViewModel>();

            using (SQLite.SQLiteConnection db = new SQLite.SQLiteConnection(app.DBPath))
            {
                var results = db.Table<Account>().OrderBy(a => a.Id);

                foreach (Account _account in results)
                {
                    AccountViewModel account = new AccountViewModel(_account.Id, _account.DateCreated);
                    account.Name = _account.Name;
                    account.Type = _account.Type;
                    account.Description = _account.Description;
                    account.CurrentBalance = _account.CurrentBalance;

                    accounts.Add(account);
                }

            }

            return accounts;
        }

        public async Task<bool> Save()
        {

            var db = new SQLite.SQLiteAsyncConnection(app.DBPath);
            int success = 0;

            try
            {
                Account existingAccount = await (db.Table<Account>().Where(
                    a => a.Id == Id)).FirstOrDefaultAsync();

                if (existingAccount != null)
                {
                    existingAccount.Name = Name;
                    existingAccount.Description = Description;
                    existingAccount.CurrentBalance = CurrentBalance;
                    success = await db.UpdateAsync(existingAccount);
                }
                else
                {
                    success = await db.InsertAsync(new Account()
                    {
                        Name = this.Name,
                        Description = this.Name,
                        Type = this.Type,
                        CurrentBalance = this.CurrentBalance,
                        DateCreated = DateTime.Now
                    });
                }
            }
            catch
            {
                success = 0;
            }

            return success != 0;
        }

        public async Task<bool> DeleteAccount(int accountId)
        {
            var db = new SQLite.SQLiteAsyncConnection(app.DBPath);

            var existingAccount = await (db.Table<Account>().Where(
                a => a.Id == accountId)).FirstAsync();

            return await db.DeleteAsync(existingAccount) > 0;
        }
        
        #endregion

        #region Validation

        public bool IsValid()
        {
            bool isValid = true;

            if (String.IsNullOrWhiteSpace(Name))
            {
                isValid = false;
            }

            return isValid;
        }

        #endregion

    }
}

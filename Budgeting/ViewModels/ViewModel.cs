using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using BudgetHelper.Models;
using System.Linq.Expressions;

namespace BudgetHelper.ViewModels
{
    /// <summary>
    /// A generic wrapper class that handles most database functions for Model types.
    /// </summary>
    /// <typeparam name="T">Generic types must inherit Model and contain a default constructor.</typeparam>
    public class ViewModel<T> where T : Model, new()
    {

        private App _app;

        public ViewModel()
        {
            _app = (Application.Current as App);
        }

        /// <summary>
        /// Finds the database record for the given Id. 
        /// </summary>
        /// <param name="modelId">The Id to search for.</param>
        /// <returns>The found record if it exists. Throws an exception if no record is found.</returns>
        public T Find(int modelId)
        {

            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
                return db.Table<T>().Where(r => r.Id == modelId).Single<T>();
            }
        }

        /// <summary>
        /// Retrieves all records in a table.
        /// </summary>
        /// <returns>Returns a list of records.</returns>
        public List<T> FetchAll()
        {
            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
                return db.Table<T>().ToList<T>();
            }
        }

        /// <summary>
        /// Retrieves all records matching the given condition.
        /// </summary>
        /// <param name="where">A condition that all records in the returned list must observe.</param>
        /// <returns>The list of records that adhere to the given condition</returns>
        public List<T> FetchAll(Expression<Func<T, bool>> where)
        {
            return FetchAll(where, int.MaxValue);
        }

        /// <summary>
        /// Retrieves all records matching the given condition. The number of returned will not exceed the amount set.
        /// </summary>
        /// <param name="where">A condition that all records in the returned list must observe.</param>
        /// <param name="count">The maximum amount of records returned</param>
        /// <returns>The list of records that adhere to the given condition. The size of the list not exceeding count</returns>
        public List<T> FetchAll(Expression<Func<T, bool>> where, int count)
        {
            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
               
                if (count == int.MaxValue)
                {
                    return db.Table<T>().Where(where).ToList<T>();
                }
                else
                {
                    return db.Table<T>().Where(where).Take(count).ToList<T>();
                }
            }
        }

        /// <summary>
        /// Saves a record to the database.
        /// If the record does not exist, it is inserted into its table.
        /// If the record exists, the database record will be updated.
        /// </summary>
        /// <param name="model">The record to save.</param>
        /// <returns>The Id of the saved record.</returns>
        public int Save(T model)
        {

            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
                T currentRecord = null;

                if (db.Table<T>().Count() > 0)
                {
                    currentRecord = db.Table<T>().Where(r => r.Id == model.Id).Single<T>();
                }

                if (currentRecord != null)
                {
                    return Update(model);
                }
                else
                {
                    return Insert(model);
                }
            }
        }

        /// <summary>
        /// Inserts a record into the database.
        /// </summary>
        /// <param name="model">The record to insert.</param>
        /// <returns>The Id of the newly inserted record.</returns>
        public int Insert(T model)
        {
            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
                return db.Insert(model);
            }
        }

        /// <summary>
        /// Updates a record already existing in the database.
        /// </summary>
        /// <param name="model">The record to update. The Id property must be set.</param>
        /// <returns>The Id of the updated record.</returns>
        public int Update(T model)
        {
            using (var db = new SQLite.SQLiteConnection(_app.DBPath))
            {
                return db.Update(model);
            }
        }

    }
}

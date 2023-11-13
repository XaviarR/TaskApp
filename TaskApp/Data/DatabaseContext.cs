using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TaskApp.Models;

namespace TaskApp.Data
{
	public class DatabaseContext : IAsyncDisposable
	{
		private const string DbName = "MyDataBase.db3";

		private static string DbPath = Path.Combine(FileSystem.AppDataDirectory, DbName);

		private SQLiteAsyncConnection _connection;
		private SQLiteAsyncConnection Database => (_connection ??= new SQLiteAsyncConnection(DbPath, SQLiteOpenFlags.Create | SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.SharedCache));
		
		// Create table in database of type TTable if not exists
		private async Task CreateTableIfNotExists<TTable>() where TTable : class, new()
		{
			await Database.CreateTableAsync<TTable>();
		}

		// Checks if Table exists and returns table, calls function to check
		private async Task<AsyncTableQuery<TTable>> GetTableAsync<TTable>() where TTable: class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return Database.Table<TTable>();
		}

		// Gets table and converts to list, GetTableAsync has logic to check if table needs to be created
		public async Task<IEnumerable<TTable>> GetAllAsync<TTable>() where TTable : class, new()
		{
			var table = await GetTableAsync<TTable>();
			return await table.ToListAsync();
		}

		// Gets table converts to list where filter is applicable, GetTableAsync has logic to check if table needs to be created
		public async Task<IEnumerable<TTable>> GetFilteredAsync<TTable>(Expression<Func<TTable, bool>> predicate) where TTable : class, new()
		{
			var table = await GetTableAsync<TTable>();
			return await table.Where(predicate).ToListAsync();
		}

		// Checks if table exists, creates if not, Gets Item by primary in table
		public async Task<TTable> GetItemByKeyAync<TTable>(Object primarykey) where TTable : class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return await Database.GetAsync<TTable>(primarykey);
		}

		// Checks if table exists, creates if not, Inserts item into table if count is greater than 0
		public async Task<bool> AddItemAync<TTable>(TTable item) where TTable : class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return await Database.InsertAsync(item) > 0;
		}

		// Checks if table exists, creates if not, Updates item in table
		public async Task<bool> UpdateItemAync<TTable>(TTable item) where TTable : class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return await Database.UpdateAsync(item) > 0;
		}

		// Checks if table exists, creates if not, Deletes Item in table
		public async Task<bool> DeleteItemAync<TTable>(TTable item) where TTable : class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return await Database.DeleteAsync(item) > 0;
		}

		// Checks if table exists, creates if not, Deletes Item by primary key in table
		public async Task<bool> DeleteItemByKeyAync<TTable>(Object primarykey) where TTable : class, new()
		{
			await CreateTableIfNotExists<TTable>();
			return await Database.DeleteAsync<TTable>(primarykey) > 0;
		}

		// Dispose
		public async ValueTask DisposeAsync() => await _connection?.CloseAsync();
	}
}

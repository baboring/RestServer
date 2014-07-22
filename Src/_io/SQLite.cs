#undef SQL_SUPPORTED

using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;

namespace SQLite
{
	public class SqLiteDatabase
	{
    #if SQL_SUPPORTED
		private readonly SqliteConnection _dbConnection;
		

		public SqLiteDatabase()
		{
			_dbConnection = new SqliteConnection("Data Source=default.s3db");
		}
		
		public SqLiteDatabase(String datasource)
		{
			_dbConnection = new SqliteConnection(string.Format("Data Source={0}", datasource));
		}
		
		public SqLiteDatabase(Dictionary<String, String> connectionOpts)
		{
			String str = connectionOpts.Aggregate("",
			                                      (current, row) =>
			                                      current + String.Format("{0}={1}; ", row.Key, row.Value));
			str = str.Trim().Substring(0, str.Length - 1);
			_dbConnection = new SqliteConnection(str);
		}
		
		#region IDisposable Members
		
		public void Dispose()
		{
			if (_dbConnection != null)
				_dbConnection.Dispose();
			
			GC.Collect();
			GC.SuppressFinalize(this);
		}
		
		#endregion
		
		public bool OpenConnection()
		{
			try
			{
				if (_dbConnection.State == ConnectionState.Closed)
					_dbConnection.Open();
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}
		
		public bool CloseConnection()
		{
			try
			{
				_dbConnection.Close();
				_dbConnection.Dispose();
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}
		

		public DataTable GetDataTable(string sql)
		{
			var table = new DataTable();
			
			try
			{
				using (SqliteTransaction transaction = _dbConnection.BeginTransaction())
				{
					using (var cmd = new SqliteCommand(_dbConnection) {Transaction = transaction, CommandText = sql})
					{
						using (SqliteDataReader reader = cmd.ExecuteReader())
						{
							table.Load(reader);
							transaction.Commit();
						}
					}
				}
				
				return table;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			finally
			{
				table.Dispose();
			}
			
			return null;
		}

		public double? ExecuteNonQuery(string sql)
		{
			Stopwatch s = Stopwatch.StartNew();
			
			try
			{
				using (SqliteTransaction transaction = _dbConnection.BeginTransaction())
				{
					using (var cmd = new SqliteCommand(_dbConnection) {Transaction = transaction})
					{
						/*foreach (string line in new LineReader(() => new StringReader(sql)))
						{
							cmd.CommandText = line;
							cmd.ExecuteNonQuery();
						}*/
						
						transaction.Commit();
					}
				}
				s.Stop();
				
				return s.Elapsed.TotalMinutes;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return null;
		}

		public string ExecuteScalar(string sql)
		{
			try
			{
				using (SqliteTransaction transaction = _dbConnection.BeginTransaction())
				{
					using (var cmd = new SqliteCommand(_dbConnection) {Transaction = transaction, CommandText = sql})
					{
						object value = cmd.ExecuteScalar();
						transaction.Commit();
						return value != null ? value.ToString() : "";
					}
				}
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return null;
		}
		
		public bool Update(String tableName, Dictionary<String, String> data, String where)
		{
			string vals = "";
			if (data.Count >= 1)
			{
				vals = data.Aggregate(vals,
				                      (current, val) =>
				                      current +
				                      String.Format(" {0} = '{1}',", val.Key.ToString(CultureInfo.InvariantCulture),
				              val.Value.ToString(CultureInfo.InvariantCulture)));
				vals = vals.Substring(0, vals.Length - 1);
			}
			try
			{
				ExecuteNonQuery(String.Format("update {0} set {1} where {2};", tableName, vals, where));
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}
		
		public bool Delete(String tableName, String where)
		{
			try
			{
				ExecuteNonQuery(String.Format("delete from {0} where {1};", tableName, where));
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}
		
		public bool Insert(String tableName, Dictionary<String, String> data)
		{
			string columns = "";
			string values = "";
			foreach (var val in data)
			{
				columns += String.Format(" {0},", val.Key);
				values += String.Format(" '{0}',", val.Value);
			}
			columns = columns.Substring(0, columns.Length - 1);
			values = values.Substring(0, values.Length - 1);
			try
			{
				ExecuteNonQuery(String.Format("insert into {0}({1}) values({2});", tableName, columns, values));
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}
		
		public bool WipeDatabase()
		{
			DataTable tables = null;
			
			try
			{
				tables = GetDataTable("select NAME from SQLITE_MASTER where type='table' order by NAME;");
				foreach (DataRow table in tables.Rows)
				{
					WipeTable(table["NAME"].ToString());
				}
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			finally
			{
				if (tables != null) tables.Dispose();
			}
			
			return false;
		}
		
		public bool WipeTable(String table)
		{
			try
			{
				ExecuteNonQuery(String.Format("delete from {0};", table));
				return true;
			}
			catch (Exception e)
			{
				Console.WriteLine("SQLite Exception : {0}", e.Message);
			}
			
			return false;
		}

        public static DataTable CreateDatatable(Type myType)
        {
            DataTable dt = new DataTable();
            
            foreach (PropertyInfo info in myType.GetProperties())
            {
                dt.Columns.Add(new DataColumn(info.Name, info.PropertyType));
            }
            
            return dt;
        }

        #endif
	}
   
}
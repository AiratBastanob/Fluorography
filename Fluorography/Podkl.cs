using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fluorography
{
	class Podkl
	{
		public SqlConnection SqlConnection = new SqlConnection(@"Data Source=DINA_ILNUROVNA\SQLEXPRESS;Trusted_Connection=Yes;DataBase=AtnSRB");
		public DataTable OverallSelect(string selectSql) // функция подключения к базе данных и обработка запросов
		{
			DataTable dataTable = new DataTable("dataBase"); // создаём таблицу в приложении
			SqlConnection.Open(); // открываем базу данных
			SqlCommand sqlCommand = SqlConnection.CreateCommand(); // создаём команду
			sqlCommand.CommandText = selectSql; // присваиваем команде текст
			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand); // создаём обработчик
			sqlDataAdapter.Fill(dataTable); // возращаем таблицу с результатом
			SqlConnection.Close();
			return dataTable;
		}
		public bool adding_deleting_changing(string selectSql) // функция подключения к базе данных и обработка запросов
		{
			DataTable dataTable = new DataTable("dataBase"); // создаём таблицу в приложении
			SqlConnection.Open(); // открываем базу данных
			SqlCommand sqlCommand = SqlConnection.CreateCommand(); // создаём команду
			sqlCommand.CommandText = selectSql; // присваиваем команде текст
			try
			{
				sqlCommand.ExecuteNonQuery();
			}
			catch
			{
				SqlConnection.Close();
				return false;
			}
			SqlConnection.Close();
			return true;
		}
		public DataTable Select(string selectSql)
		{
			DataTable dataTable = new DataTable("dataBase");

			SqlConnection sqlConnection = new SqlConnection(@"Data Source=DINA_ILNUROVNA\SQLEXPRESS;Trusted_Connection=Yes;DataBase=AtnSRB");
			sqlConnection.Open();

			SqlCommand sqlCommand = sqlConnection.CreateCommand();
			sqlCommand.CommandText = selectSql;

			SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
			sqlDataAdapter.Fill(dataTable);
			return dataTable;
		}
	}
}

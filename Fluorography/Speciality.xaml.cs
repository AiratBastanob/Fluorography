using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Fluorography
{
	/// <summary>
	/// Логика взаимодействия для Speciality.xaml
	/// </summary>
	public partial class Speciality : System.Windows.Window
	{
		public AtnSRBEntities db = new AtnSRBEntities();
		private System.Data.DataTable _db;
		Podkl _podkl = new Podkl();
		public Speciality()
		{
			InitializeComponent();
			spetsList.ItemsSource = db.Users.ToList();
		}
		public void Clear()
		{
			login.Clear();
			pass.Clear();
			role.Clear();
			fio.Clear();
		}		
		private void exit_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
        }
		private void newSpets_Click(object sender, RoutedEventArgs e)
		{
			if ((login.Text != "") && (pass.Text != "") && (role.Text != string.Empty) && (fio.Text != string.Empty))
			{
				const string connectionString = (@"Data Source=DINA_ILNUROVNA\SQLEXPRESS;Trusted_Connection=Yes;DataBase=AtnSRB");
				var con = new SqlConnection(connectionString);
				con.Open();
				var cmd = new SqlCommand("INSERT INTO [dbo].[Users] (login, password, role, FIO)  VALUES (@log, @pass, @role, @fio)", con);
				cmd.Parameters.AddWithValue("@log", login.Text);
				cmd.Parameters.AddWithValue("@pass", pass.Text);
				cmd.Parameters.AddWithValue("@role", role.Text);
				cmd.Parameters.AddWithValue("@fio", fio.Text);
				try
				{
					cmd.ExecuteNonQuery();
					con.Close();
					MessageBox.Show("Специалист добавлен");
					spetsList.ItemsSource = db.Users.ToList();
					Clear();
				}
				catch
				{
					MessageBox.Show("Возможно вы что-то забыли ввести:)");
				}
			}
			else { MessageBox.Show("Вы забыли заполнить!"); }
		}
		private void delete_Click(object sender, RoutedEventArgs e)
		{
			if (spetsList.SelectedItem != null)
			{
				string cmd = "DELETE FROM [dbo].[Users] WHERE ID = '" + id.Text + "'";
				bool success = _podkl.adding_deleting_changing(cmd);
				if (success)
				{
					MessageBox.Show("Специалист удален!");
					spetsList.ItemsSource = db.Users.ToList();
				}
				else
				{
					MessageBox.Show("ОЙ! Что-то пошло не так");
				}
			}
			else
			{
				MessageBox.Show("Выберите пациента!");
			}
		}
		private void search_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (poisk.Text != "" && poisk.Text != " ")
				{
					_db = _podkl.OverallSelect("Select ID, login, password, role FROM Users WHERE FIO = '" + poisk.Text + "'");
					poisk.Clear();
					spetsList.ItemsSource = _db.DefaultView;
				}
				else { MessageBox.Show("Введите что-нибудь"); }
			}
			catch
			{
				MessageBox.Show("Такого cпециалиста нет в базе данных");
				poisk.Clear();
			}
		}
		private void spetsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			try
			{
				DataRowView drv = spetsList.SelectedItem as DataRowView;
				if (drv != null)
				{
					id.Text = drv[0].ToString();

					_db = _podkl.OverallSelect("SELECT login, password, role, FIO FROM [dbo].[Users] WHERE ID = '" + drv[0] + "'");
					for (int i = 0; i < _db.Rows.Count; i++)
					{
						login.Text = _db.Rows[0][0].ToString();
						pass.Text = _db.Rows[0][1].ToString();
						role.Text = _db.Rows[0][2].ToString();
						fio.Text = _db.Rows[0][3].ToString();
					}
				}
			}
			catch (NullReferenceException) { }
		}
		private void update_Click(object sender, RoutedEventArgs e)
		{
			if ((login.Text != "") && (pass.Text != "") && (role.Text != string.Empty) && (fio.Text != string.Empty))
			{
				string cmm = "UPDATE Users SET login = '" + login.Text + "', password = '" + pass.Text + "', role = '" + role.Text + "', FIO = '" + fio.Text + "' WHERE ID = " + Convert.ToInt32(id.Text) + "";
				bool success = _podkl.adding_deleting_changing(cmm);
				if (success)
				{
					Clear();
					MessageBox.Show("Успешно сохранено!");
				}
				else
				{
					MessageBox.Show("Может вы что-то забыли ввести?!");
				}
			}
			else { MessageBox.Show("Вы забыли заполнить!"); }
		}

		private void poisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			spetsList.ItemsSource = db.Users.ToList().Where(c => c.FIO.Contains(poisk.Text));
		}
	}
	
}

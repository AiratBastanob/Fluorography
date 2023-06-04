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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Fluorography
{
	/// <summary>
	/// Логика взаимодействия для MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		Podkl _podkl = new Podkl();
		private DataTable _db;
		public AtnSRBEntities db = new AtnSRBEntities();
		public MainWindow()
		{
			InitializeComponent();
		}
		private void logIn_Click(object sender, RoutedEventArgs e)
		{
			if (login.Text.Length > 0) // проверяем введён ли логин     
			{
				if (password.Password == (from ut in db.Users where ut.password == password.Password select ut.password).FirstOrDefault()) // проверяем введён ли пароль         
				{             // ищем в базе данных пользователя с такими данными         
					try
					{
						DataTable dtUser = _podkl.Select("SELECT * FROM [dbo].[Users] WHERE [login] = '" + login.Text + "' AND [password] = '" + password.Password + "'");
						if (dtUser.Rows.Count > 0) // если такая запись существует       
						{
							MessageBox.Show("Пользователь авторизовался"); // говорим, что авторизовался         
						}

						DataRow a = dtUser.Rows[0];
						int b = dtUser.Columns.IndexOf("role");
						if (a[b].Equals("admin"))
						{
							Admin admin = new Admin();
							admin.Show();
							_db = _podkl.OverallSelect("SELECT FIO FROM [dbo].[Users] WHERE login = '" + login.Text + "'");
							for (int i = 0; i < _db.Rows.Count; i++)
							{
								admin.privet.Content = "Приветствую, " + _db.Rows[0][0].ToString();
							}
							this.Close();
						}
						if (a[b].Equals("spets"))
						{
							Specialist specialist = new Specialist();
							specialist.Show();
							_db = _podkl.OverallSelect("SELECT FIO FROM [dbo].[Users] WHERE login = '" + login.Text + "'");
							for (int i = 0; i < _db.Rows.Count; i++)
							{
								specialist.privet.Content = "Удачной работы, " + _db.Rows[0][0].ToString();
							}
							this.Close();
						}
					}
					catch
					{
						MessageBox.Show("Пользователь не найден"); // выводим ошибку  
					}
				}
				else MessageBox.Show("Неверный пароль"); // выводим ошибку    
			}
			else MessageBox.Show("Введите логин"); // выводим ошибку 

		}
		public string LogIn(string a, string b, string c)
		{

			string mes = "";
			DataTable dt_user = _podkl.OverallSelect("SELECT * FROM [dbo].[Users] WHERE [login] = '" + a + "' AND [password] = '" + b + "' AND [role] = '" + c + "'");
			if (dt_user.Rows.Count > 0) // если такая запись существует       
			{
				mes = "Успешная авторизация"; // говорим, что  авторизовался              
			}
			return mes;
		}
		public string NewPatient(string a, string b, string c, string d, string e, string f)
		{
			string mes = "";
			System.Data.DataTable dt_user = _podkl.OverallSelect("SELECT * FROM [dbo].[Patients] WHERE [Surname] = '" + b + "' AND [LastName] = '" + a + "'");

			string connetionString = null;
			SqlCommand cmd;
			SqlConnection con;
			connetionString = @"Data Source=DINA_ILNUROVNA\SQLEXPRESS;Trusted_Connection=Yes;DataBase=AtnSRB";
			con = new SqlConnection(connetionString);
			con.Open();
			cmd = new SqlCommand("INSERT INTO [dbo].[Patients] (LastName, Surname, Patronymic, DateOfBirth, Enp, Info)  VALUES (@lastName, @surname, @patronymic, @db, @enp, @info)", con);
			cmd.Parameters.AddWithValue("@lastName", a);
			cmd.Parameters.AddWithValue("@surname", b);
			cmd.Parameters.AddWithValue("@patronymic", c);
			cmd.Parameters.AddWithValue("@db", d);
			cmd.Parameters.AddWithValue("@enp", e);
			cmd.Parameters.AddWithValue("@info", f);
			try
			{
				cmd.ExecuteNonQuery();
				mes = "Успешное добавление";// говорим, что зарегистрирован   
			}
			catch
			{ MessageBox.Show("Облом!"); }
			finally { con.Close(); }
			return mes;
		}
		
	}
}

using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Fluorography
{
	/// <summary>
	/// Логика взаимодействия для PatientCD.xaml
	/// </summary>
	public partial class PatientCD : System.Windows.Window
	{
		Specialist sp = new Specialist();
		private System.Data.DataTable _db;
		Podkl _podkl = new Podkl();
		private string _fio;
		private readonly System.Windows.Window _window;

		public PatientCD(System.Windows.Window window = null, string fio = null)
		{
			InitializeComponent();
			_window = window;
			_fio = fio;
		}

		public bool Proverka()
		{
			if (lastName.Text == null || name.Text == null || patronymic.Text == null || dateOfBirth.Text == null || enp.Text == null) 
			{
				MessageBox.Show("Вы забыли что-то ввести");
			}
			return true;
		}
		public void Clear()
		{
			name.Clear();
			lastName.Clear();
			patronymic.Clear();
			dateOfBirth.Clear();
			enp.Clear();
			fluo.Text = "не пройдена";
		}
		private void fluographyOk_Click_1(object sender, RoutedEventArgs e)
		{

			if (fluographyOk.Content.ToString() == "проверить")
			{
				var flu = new Flu(_window, _fio);
				flu.Show();
				flu.getFluor.IsEnabled = false;
				Hide();
				_db = _podkl.OverallSelect("SELECT ID FROM [dbo].[Patients] WHERE ID = '" + idd.Text + "'");
				for (int i = 0; i < _db.Rows.Count; i++)
				{
					flu.patient.Content = _db.Rows[0][0].ToString();
				}
				_db = _podkl.OverallSelect("SELECT Street, Selo, DateSurvay FROM [dbo].[Registr] WHERE ID_patient = '" + idd.Text + "'");
				for (int i = 0; i < _db.Rows.Count; i++)
				{
					flu.street.Text = _db.Rows[0][0].ToString();
					flu.selo.Text = _db.Rows[0][1].ToString();
					flu.data.Text = _db.Rows[0][2].ToString();
				}
			}
			else
			{
				var flu = new Flu(_window, _fio);
				flu.Show();
				flu.updating.IsEnabled= false;
				Hide();
				_db = _podkl.OverallSelect("SELECT ID FROM [dbo].[Patients] WHERE ID = '" + idd.Text + "'");
				for (int i = 0; i < _db.Rows.Count; i++)
				{
					flu.patient.Content = _db.Rows[0][0].ToString();
				}
			}	
		}

		private void exit_Click(object sender, RoutedEventArgs e)
		{
			_window.Show();
			
			this.Close();
			sp.Obnov();
		}

		private void savePatient_Click(object sender, RoutedEventArgs e)
		{
			if ((lastName.Text != string.Empty) && (name.Text != string.Empty) && (patronymic.Text != string.Empty) && (dateOfBirth.Text != string.Empty) && (enp.Text != string.Empty))
			{
				string cmm = "UPDATE Patients SET LastName = '" + name.Text + "', Surname = '" + lastName.Text + "', Patronymic = '" + patronymic.Text + "', DateOfBirth = '" + dateOfBirth.Text + "', Info = '" + fluo.Text + "', Enp = '" + enp.Text + "' WHERE ID = " + Convert.ToInt32(idd.Text) + "";
				bool success = _podkl.adding_deleting_changing(cmm);
				if (success)
				{
					MessageBox.Show("Успешно сохранено!");
				}
				else
				{
					MessageBox.Show("Поробуйте снова");
				}
			}
			else { MessageBox.Show("Вы забыли заполнить!"); }

		}
		public System.Windows.Window MyWindow { get; set; }		
		private void newPatient_Click(object sender, RoutedEventArgs e)
		{
			if ((lastName.Text != string.Empty) && (name.Text != string.Empty) && (patronymic.Text != string.Empty) && (dateOfBirth.Text != string.Empty) && (enp.Text != string.Empty))
			{
				const string connectionString = (@"Data Source=DINA_ILNUROVNA\SQLEXPRESS;Trusted_Connection=Yes;DataBase=AtnSRB");
				var con = new SqlConnection(connectionString);
				con.Open();
				var cmd = new SqlCommand("INSERT INTO [dbo].[Patients] (LastName, Surname, Patronymic, DateOfBirth, Enp, Info)  VALUES (@lastName, @surname, @patronymic, @db, @enp, @info)", con);
				cmd.Parameters.AddWithValue("@lastName", name.Text);
				cmd.Parameters.AddWithValue("@surname", lastName.Text);
				cmd.Parameters.AddWithValue("@patronymic", patronymic.Text);
				cmd.Parameters.AddWithValue("@db", dateOfBirth.Text);
				cmd.Parameters.AddWithValue("@enp", enp.Text);
				cmd.Parameters.AddWithValue("@info", fluo.Text);
				try
				{
					cmd.ExecuteNonQuery();
					con.Close();
					sp.Obnov();
					MessageBox.Show("Пациент добавлен");
					Clear();
				}
				catch
				{
					MessageBox.Show("Возможно вы что-то забыли ввести:)");
				}
			}
			else { MessageBox.Show("Вы забыли заполнить!"); }
		}
	}
}

﻿using Microsoft.Vbe.Interop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
//using Org.BouncyCastle.Ocsp;

namespace Fluorography
{
	public class InfoPatientToColorConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			string infoAboutFluorography = (string)value;
			if (infoAboutFluorography == "не пройдена")
			{
				return Brushes.Green;
			}
			else
			{
				return Brushes.White;
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	/// <summary>
	/// Логика взаимодействия для Specialist.xaml
	/// </summary>
	public partial class Specialist 
	{
		public AtnSRBEntities db = new AtnSRBEntities();
		private DataTable _db;
		Podkl _podkl = new Podkl();
		public Specialist()
		{
			InitializeComponent();
			patientsList.ItemsSource = db.Patients.ToList();
		}

		/// <summary>
		/// Логика закрашивание пациента
		/// </summary>
		private void patientsList_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			Patients patients = e.Row.Item as Patients;
			if (patients != null)
			{
				DateTime currentDate = DateTime.Now.AddYears(-1);
				int checkDate = db.Registr
					.Where(ps => ps.ID_patient == patients.ID && ps.DateSurvay < currentDate)
					.Count();

				if (checkDate == 1)
				{
					e.Row.Background = Brushes.Red;
				}
			}
		}

		public void Obnov()
		{
			_db = _podkl.OverallSelect("Select ID,  LastName, Surname, Patronymic FROM [dbo].[Patients]");
			patientsList.ItemsSource = _db.DefaultView;
		}
		private void newPatient_Click(object sender, RoutedEventArgs e)
		{
			PatientCD patient = new PatientCD();
			patient.Show();
		}
		private void exit_Click(object sender, RoutedEventArgs e)
		{
			if (exit.Content.ToString() == "ВЫХОД")
			{
				MainWindow mainWindow = new MainWindow();
				mainWindow.Show();
				this.Close();
			}
			else
			{
				this.Close();
			}
		}
		private void patientsList_Click(object sender, RoutedEventArgs e)
		{
			PatientCD patient = new PatientCD();
			patient.Show();
		}
		private void patientsList_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
		{
			try
			{
				DataRowView drv = patientsList.SelectedItem as DataRowView;
				if (drv != null)
				{
					id.Text = drv[0].ToString();

					_db = _podkl.OverallSelect("SELECT LastName, Surname, Patronymic, Info FROM [dbo].[Patients] WHERE ID = '" + drv[0] + "'");
					for (int i = 0; i < _db.Rows.Count; i++)
					{
						name.Text = _db.Rows[0][0].ToString();
						surname.Text = _db.Rows[0][1].ToString();
						patronymic.Text = _db.Rows[0][2].ToString();
						info.Text = _db.Rows[0][3].ToString();
					}
				}
			}
			catch (NullReferenceException) { }
		}
		private void patientsList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
		{
			var cd = new PatientCD(this, privet.Content.ToString());
			try
			{
				var pat = (Patients)patientsList.SelectedItem;
				if (pat != null)
				{
					cd.idd.Text = pat.ID.ToString();
					cd.name.Text = pat.Surname.ToString();
					cd.lastName.Text = pat.LastName.ToString();
					cd.patronymic.Text = pat.Patronymic.ToString();
					cd.dateOfBirth.Text = pat.DateOfBirth.ToString();
					cd.enp.Text = pat.Enp.ToString();
					if (pat.Info.ToString() == "пройдена" || pat.Info.ToString() == "да")
					{
						cd.fluo.Text = "пройдена";
						cd.fluographyOk.Content = "проверить";
					}
					else { cd.fluo.Text = "не пройдена"; }
				}
			} 
			catch (NullReferenceException) { }
			cd.Show();
			Hide();
		}
		private void delete_Click(object sender, RoutedEventArgs e)
		{
			if (patientsList.SelectedItem != null)
			{
				string cmd = "DELETE FROM [dbo].[Patients] WHERE ID = '" + id.Text + "'";
				string cm = "DELETE FROM [dbo].[Registr] WHERE ID_patient = '" + id.Text + "'";
				bool success = _podkl.adding_deleting_changing(cmd);
				bool success1 = _podkl.adding_deleting_changing(cm);
				if (success && success1)
				{
					MessageBox.Show("Пациент удален!");
					Obnov();
				}
				else
				{
					MessageBox.Show("Не хочет удалятся");
				}
			}
			else
			{
				MessageBox.Show("Выберите пациента!");
			}
		}	
		private void poisk_TextChanged(object sender, TextChangedEventArgs e)
		{
			patientsList.ItemsSource = db.Patients.ToList().Where(c => c.LastName.Contains(poisk.Text));
		}
	}
}

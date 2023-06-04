using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
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
using Window = System.Windows.Window;
using Word = Microsoft.Office.Interop.Word;

namespace Fluorography
{
	/// <summary>
	/// Логика взаимодействия для Admin.xaml
	/// </summary>
	public partial class Admin : Window
	{
		public Admin()
		{
			InitializeComponent();
		}
		/// <summary>
		/// Переход к пациентам
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void patientiki_Click(object sender, RoutedEventArgs e)
		{
			Specialist sp = new Specialist();
			sp.Show();
			sp.privet.Visibility = Visibility.Hidden;
			sp.exit.Content = "назад";
        }
		/// <summary>
		/// Выход
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exit_Click(object sender, RoutedEventArgs e)
		{
			MainWindow mainWindow = new MainWindow();
			mainWindow.Show();
			this.Close();
		}
		/// <summary>
		/// контекст данных
		/// </summary>
		private AtnSRBEntities _context = new AtnSRBEntities();
		/// <summary>
		/// оформление отчета
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void otchotik_Click(object sender, RoutedEventArgs e)
		{
			if (month.Text != "")
			{
				var allRegistr = _context.Registr.ToList();
				var allPatients = _context.Patients.ToList();
				var application = new Word.Application();
				Word.Document document = application.Documents.Add();

				
					Word.Paragraph paragraph = document.Paragraphs.Add();
					Word.Range range = paragraph.Range;
					range.Text = "Прохождение флюорографии в Атнинской ЦРБ";
					paragraph.set_Style("Заголовок 1");
					range.InsertParagraphAfter();

					Word.Paragraph tablePar = document.Paragraphs.Add();
					Word.Range tablRan = tablePar.Range;
					Word.Table table = document.Tables.Add(tablRan, allRegistr.Count() + 1, 4);
					table.Borders.InsideLineStyle = table.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
					table.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

					Word.Range cellRange;

					cellRange = table.Cell(1, 1).Range;
					cellRange.Text = "Пациент";
					cellRange = table.Cell(1, 2).Range;
					cellRange.Text = "Дата прохождения";
					cellRange = table.Cell(1, 3).Range;
					cellRange.Text = "Улица";
					cellRange = table.Cell(1, 4).Range;
					cellRange.Text = "Село";

					table.Rows[1].Range.Bold = 1;
					table.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
				
				for (int i = 0; i < allRegistr.Count; i++)
				{
					var curReg = allRegistr[i];
					var curPati = allPatients[i];
					if (curReg.DateSurvay.Month.ToString() == month.Text)
					{
						cellRange = table.Cell(i + 2, 1).Range;
						cellRange.Text = curPati.LastName + " " + curPati.Surname + " " + curPati.Patronymic;

						cellRange = table.Cell(i + 2, 2).Range;
						cellRange.Text = curReg.DateSurvay.ToString();

						cellRange = table.Cell(i + 2, 3).Range;
						cellRange.Text = curReg.Street;

						cellRange = table.Cell(i + 2, 4).Range;
						cellRange.Text = curReg.Selo;

					}
					else if (month.Text == "все")
					{
						
						cellRange = table.Cell(i + 2, 1).Range;
						cellRange.Text = curPati.LastName + " " + curPati.Surname + " " + curPati.Patronymic;
						

						cellRange = table.Cell(i + 2, 2).Range;
						cellRange.Text = curReg.DateSurvay.ToString();

						cellRange = table.Cell(i + 2, 3).Range;
						cellRange.Text = curReg.Street;

						cellRange = table.Cell(i + 2, 4).Range;
						cellRange.Text = curReg.Selo;

					}
				}
				
				application.Visible = true;
				document.SaveAs2(@"C:\Users\latdi\source\repos\Fluorography\Atn.pdf", Word.WdExportFormat.wdExportFormatPDF);

			}
			else
			{
				MessageBox.Show("Введите период оформления отчета");
			}
		}

		private void spetsi_Click(object sender, RoutedEventArgs e)
		{
			Speciality sp = new Speciality();	
			sp.Show();
			sp.privet.Content = privet.Content;
		}
	}
}

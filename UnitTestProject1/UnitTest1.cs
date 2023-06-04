using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Fluorography;

namespace UnitTestProject1
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestAuthorization()
		{
			MainWindow mw = new MainWindow();
			string expected = "Успешная авторизация";
			string a = "geg";
			string b = "123";
			string c = "spets";
			string actual = mw.LogIn(a, b, c);
			Assert.AreEqual(expected, actual);
		}

		[TestMethod]
		public void TestDobavleniye()
		{
			MainWindow mw = new MainWindow();
			string expected = "Успешное добавление";
			string a = "Остап";
			string b = "Михайлов";
			string c = "Валерьевич";
			string d = "12.05.1953";
			string e = "12456789";
			string f = "не пройдена";
			string actual = mw.NewPatient(a, b, c, d, e, f);
			Assert.AreEqual(expected, actual);
		}
	}
}

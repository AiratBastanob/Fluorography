using Fluorography;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework.Internal;
using Assert = Xunit.Assert;

namespace TestProject1
{
	
	public class UnitTest1
	{
		[Fact]
		public void Test1()
		{
			MainWindow mw = new MainWindow();
			string expected = "Успешная авторизация";
			string a = "geg";
			string b = "123";
			string c = "spets";
			string actual = mw.LogIn(a,b,c);
			Assert.Equal(expected, actual);
		}
	}
}